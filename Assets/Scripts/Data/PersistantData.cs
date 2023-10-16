using UnityEngine;

namespace GillBates.Data
{
    public static class PersistantData
    {
        public static class IsLoadingFromCache
        {
            const string Key = "gb_is_loading_from_cache";

            public static bool Value
            {
                get => PlayerPrefs.GetInt(Key) == 1;
                set => PlayerPrefs.SetInt(Key, value ? 1 : 0);
            }
        }
        
        public static class IsFirstLoad
        {
            const string Key = "gb_is_first_load";

            public static bool Value
            {
                get => PlayerPrefs.GetInt(Key) == 1;
                set => PlayerPrefs.SetInt(Key, value ? 1 : 0);
            }
        }
        
        public static class CachedMaze
        {
            const string Key = "gb_cached_maze";

            public static string Value
            {
                get => PlayerPrefs.GetString(Key);
                set => PlayerPrefs.SetString(Key, value);
            }
        }

        public static class CheesePower
        {
            const string Key = "gb_cheese_power";

            public static int Value
            {
                get => PlayerPrefs.GetInt(Key);
                set => PlayerPrefs.SetInt(Key, value);
            }
        }
    }
}