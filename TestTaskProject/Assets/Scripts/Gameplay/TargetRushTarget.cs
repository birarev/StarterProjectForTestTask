using UnityEngine;
using UnityEngine.UI;

public class TargetRushTarget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _movementArea;
    [SerializeField] private Image _targetImage;

    private TargetRushGame _game;
    private RectTransform _rectTransform;
    private bool _isDangerous;

    public void Initialize(TargetRushGame game)
    {
        _game = game;
        _rectTransform = GetComponent<RectTransform>();

        _button.onClick.AddListener(OnClicked);

        MoveToRandomPosition();
    }

    public void SetDangerous(bool isDangerous)
    {
        _isDangerous = isDangerous;

        if (_targetImage == null)
            return;

        _targetImage.color = isDangerous
            ? Color.red
            : Color.green;
    }

    private void OnClicked()
    {
        if (_game == null)
            return;

        if (_isDangerous)
        {
            _game.LoseLife();
        }
        else
        {
            _game.AddScore();
        }

        MoveToRandomPosition();
    }

    private void MoveToRandomPosition()
    {
        if (_movementArea == null || _rectTransform == null)
            return;

        Rect area = _movementArea.rect;

        float halfWidth = _rectTransform.rect.width * 0.5f;
        float halfHeight = _rectTransform.rect.height * 0.5f;

        float x = Random.Range(
            area.xMin + halfWidth,
            area.xMax - halfWidth);

        float y = Random.Range(
            area.yMin + halfHeight,
            area.yMax - halfHeight);

        _rectTransform.anchoredPosition = new Vector2(x, y);

        bool dangerous = Random.value < 0.3f;
        SetDangerous(dangerous);

        Debug.Log(dangerous
            ? "DANGEROUS TARGET"
            : "SAFE TARGET");
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnClicked);
        }
    }
}