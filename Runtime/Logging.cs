using UnityEngine;

namespace Voxell
{
  public enum LogImportance
  {
    Debug,
    Info,
    Important,
    Crucial,
    Critical
  }

  [System.Serializable]
  public class Logger
  {
    public LogImportance debugLevel;

    public Logger(LogImportance debugLevel) => this.debugLevel = debugLevel;

    /// <summary>
    /// Conditionally log message based on LogImportance and LogStyle
    /// </summary>
    /// <param name="message">message to log</param>
    /// <param name="importance">importance level of the message</param>
    /// <param name="logStyle">style of logging</param>
    public void ConditionalLog(object message, LogImportance importance, LogType logType)
    {
      if (importance >= debugLevel)
        UnityEngine.Debug.unityLogger.Log(logType, message);
    }

    public void Debug(object message, LogType logType=LogType.Log)
    {
      if (debugLevel >= LogImportance.Debug)
        UnityEngine.Debug.unityLogger.Log(logType, message);
    }

    public void Info(object message, LogType logType=LogType.Log)
    {
      if (debugLevel >= LogImportance.Info)
        UnityEngine.Debug.unityLogger.Log(logType, message);
    }

    public void Information(object message, LogType logType=LogType.Log)
    {
      if (debugLevel >= LogImportance.Info)
        UnityEngine.Debug.unityLogger.Log(logType, message);
    }

    public void Important(object message, LogType logType=LogType.Log)
    {
      if (debugLevel >= LogImportance.Important)
        UnityEngine.Debug.unityLogger.Log(logType, message);
    }

    public void Error(object message) => UnityEngine.Debug.LogError(message);
    public void Warning(object message) => UnityEngine.Debug.LogWarning(message);
  }
}