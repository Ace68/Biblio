using System;

namespace Biblio.Logging.Abstracts
{
    public interface ILogService
    {
        void LoggerTrace(string message);
        void ErrorTrace(string procedureName, Exception ex);
        void WarnTrace(string message);
    }
}
