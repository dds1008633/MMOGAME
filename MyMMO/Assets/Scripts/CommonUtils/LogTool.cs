using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools
{
    public enum ConsoleColor
    {
        White,
        Yellow,
        Cyan,
        Purple,
        Green,
        Orange,
        Red,
        Gray,
    }
    public class LogTool
    {
        static string getColorCode(ConsoleColor? color = null)
        {
            string code;
            switch (color)
            {
                case ConsoleColor.White:
                    code = "FFFFFF";
                    break;
                case ConsoleColor.Yellow:
                    code = "AAAA00";
                    break;
                case ConsoleColor.Cyan:
                    code = "00AAAA";
                    break;
                case ConsoleColor.Purple:
                    code = "6666AA";
                    break;
                case ConsoleColor.Green:
                    code = "00AA00";
                    break;
                case ConsoleColor.Orange:
                    code = "FFA04D";
                    break;
                case ConsoleColor.Red:
                    code = "FF0000";
                    break;
                case ConsoleColor.Gray:
                    code = "666666";
                    break;
                default:
                    return null;
            }
            return code;
        }
        public static void Log(object message, ConsoleColor color)
        {
#if UNITY_EDITOR
            var src = message.ToString();
            var arr = src.Split('\n');
            var str1 = arr[0];
            var idx2 = str1.Length;
            var str2 = idx2 > src.Length - 1 ? "" : src.Substring(idx2);
            Debug.Log(string.Format("<color=#{0}>{1}</color>{2}", getColorCode(color), str1, str2));
#else
            Debug.Log(message);
#endif
        }
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }
    }
}

