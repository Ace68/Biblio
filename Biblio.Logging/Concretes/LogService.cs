using System;
using Biblio.Logging.Abstracts;
using Common.Logging;

namespace Biblio.Logging.Concretes
{
    public class LogService : ILogService
    {
        private readonly ILog _log = LogManager.GetLogger("Biblio");

        public void LoggerTrace(string message)
        {
            this._log.Trace(message);
        }

        public void ErrorTrace(string procedureName, Exception ex)
        {
            if (ex.InnerException != null)
                this.ErrorTrace(procedureName, ex.InnerException);

            this._log.Error(ex.Message);
        }

        public void WarnTrace(string message)
        {
            this._log.Warn(message);
        }
    }
}
