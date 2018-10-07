using Exceptionless;
using Exceptionless.Logging;

namespace Core.Infrastructure
{
    public interface ILogger
    {
        void Info(string message, params string[] args);
    }

    public class ExceptionLessLogger : ILogger
    {
        public void Info(string message, params string[] args)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Info).AddTags(args).Submit();
        }
    }
}
