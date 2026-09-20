using System;
using UnityEngine;

public class TargetRushGame : MonoBehaviour
{
    [SerializeField] private int _targetScore = 10;
    [SerializeField] private int _startLives = 3;
    [SerializeField] private float _gameDuration = 30f;

    public event Action<int, int, float, int> StateChanged;
    public event Action<bool> GameEnded;

    public bool IsGameOver => _isGameOver;
    public int Score => _score;
    public int Combo => _combo;

    private int _score;
    private int _lives;
    private int _combo;
    private float _timeLeft;
    private bool _isGameOver;
    private bool _isRunning;

    public void StartGame()
    {
        _score = 0;
        _lives = _startLives;
        _combo = 0;
        _timeLeft = _gameDuration;
        _isGameOver = false;
        _isRunning = true;

        NotifyStateChanged();
    }

    private void Update()
    {
        if (!_isRunning || _isGameOver)
            return;

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0f)
        {
            _timeLeft = 0f;
            NotifyStateChanged();
            LoseGame();
            return;
        }

        NotifyStateChanged();
    }

    public void AddScore()
    {
        if (!_isRunning || _isGameOver)
            return;

        _score++;
        _combo++;

        NotifyStateChanged();

        if (_score >= _targetScore)
            WinGame();
    }

    public void LoseLife()
    {
        if (!_isRunning || _isGameOver)
            return;

        _lives--;
        _combo = 0;

        NotifyStateChanged();

        if (_lives <= 0)
            LoseGame();
    }

    private void WinGame()
    {
        _isGameOver = true;
        _isRunning = false;

        GameEnded?.Invoke(true);
    }

    private void LoseGame()
    {
        _isGameOver = true;
        _isRunning = false;

        GameEnded?.Invoke(false);
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke(_score, _lives, _timeLeft, _combo);
    }
}