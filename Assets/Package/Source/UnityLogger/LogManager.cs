using System;
using System.Diagnostics;
using UnityEngine;

namespace log4net
{
    public static class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new ILog(type);
        }
    }

    // ReSharper disable once InconsistentNaming
    public class ILog
    {
        // ReSharper disable once InconsistentNaming
        private readonly string _logFormat;
        
        public ILog(Type type)
        {
            _logFormat = "[" + type.FullName + "] {0}";
        }
        
        [Conditional("UNITY_EDITOR")]
        public void DebugFormat(string format, params object[] logObjects)
        {
            var final = string.Format(format, logObjects);
            UnityEngine.Debug.Log(string.Format(_logFormat,final));
        }

        public void Debug(string message)
        { 
            UnityEngine.Debug.Log(string.Format(_logFormat,message));
        }

        public void Warn(string message)
        {
            UnityEngine.Debug.LogWarning(string.Format(_logFormat,message));
        }

        public void Info(string message)
        {
            UnityEngine.Debug.Log(string.Format(_logFormat,message));
        }

        public void InfoFormat(string format, params object[] logObjects)
        {
            var final = string.Format(format, logObjects);
            UnityEngine.Debug.Log(string.Format(_logFormat,final));
        }

        public void ErrorFormat(string format, params object[] logObjects)
        {
            var final = string.Format(format, logObjects);
            UnityEngine.Debug.LogError(string.Format(_logFormat,final));
        }

        public void WarnFormat(string format, params object[] logObjects)
        {
            var final = string.Format(format, logObjects);
            UnityEngine.Debug.LogWarning(string.Format(_logFormat,final));
        }

        public void Error(string message, Exception exception)
        {
            UnityEngine.Debug.LogError(string.Format(_logFormat,message) + " err:" + exception);
        }
    }
}
