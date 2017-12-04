using System;
using Logger.enums;
using System.Configuration;

namespace Logger
{
    public interface ILogger:IDisposable
    {
        void Log(string logString, Level logLevel, DateTime dateTime, WriteFormat format);
    }
}
