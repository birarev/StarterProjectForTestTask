public static class EventsProvider
{
    public class OpenScreenEvent
    {
        public readonly string ScreenId;

        public OpenScreenEvent(string screenId)
        {
            ScreenId = screenId;
        }
    }

    public class LoadSceneEvent
    {
        public readonly string SceneName;

        public LoadSceneEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}