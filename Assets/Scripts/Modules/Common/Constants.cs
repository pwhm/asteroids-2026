namespace Modules.Common
{
    public static class Constants
    {
        public static class Scenes
        {
            public const string GAMEPLAY = "gameplay";
        }
        
        public static class Addressables
        {
            public const string SERVICE_KEY_FORMAT = "service/{0}";
            public const string SCENE_KEY_FORMAT = "{0}/scene";

            public static class Tags
            {
                public const string GLOBAL = "global";
                public const string GAMEPLAY = "gameplay";
            }
        }

        public static class Gameplay
        {
            public const string PLAYER_TAG = "Player";
        }
    }
}