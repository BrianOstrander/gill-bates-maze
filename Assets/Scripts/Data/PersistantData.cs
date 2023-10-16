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
        
        public static class CachedMaze
        {
            const string Key = "gb_cached_maze";

            public static string Value
            {
                get => PlayerPrefs.GetString(Key);
                set => PlayerPrefs.SetString(Key, value);
            }
        }
    }
}