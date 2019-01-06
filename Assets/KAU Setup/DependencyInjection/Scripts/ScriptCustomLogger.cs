
using UnityEngine;
namespace Services.DependencyInjection
{
    public static class ScriptCustomLogger
    {

        public static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<Color=Green> {0} </Color> ", message));
#endif
        }

        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<Color=Blue> {0} </Color> ", message));
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<Color=Red> {0} </Color> ", message));
#endif
        }


    }
}
