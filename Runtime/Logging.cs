/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

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