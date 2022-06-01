using Logger.Models;
using System.Reflection;


namespace Logger
{
    public interface ILogHandler
    {
  
        Exception SaveError(Exception ex, MethodBase methodInfo, params object[] parameterValues);

        void LogError(Exception ex, MethodBase serviceMethodInfo, params object[] serviceParameterValues);
        void LogError(SystemErrorLog error);
        void LogError(HandledErrorLog error);

        void LogData(MethodBase methodInfo, object answer, TimeSpan? executeTime, params object[] parameterValues);

        void CreateOperationLog(OperationLog operationLog);

        void CreateSystemErrorLog(SystemErrorLog errorLog);


       
    }
}
