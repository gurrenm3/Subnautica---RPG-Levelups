using UnityEngine;

namespace RPG_Levelups
{
    public static class Guard
    {
        public static bool IsGamePaused() => Time.timeScale == 0;
    }
}
