using Logger.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Logger
{
    public class LogHandler : ILogHandler
    {
        private readonly LableMaker_Log_DBContext _repoContext;

        public LogHandler(LableMaker_Log_DBContext repositoryContext)
        {
            _repoContext = repositoryContext;

        }
        public Exception SaveError(Exception ex, MethodBase methodInfo, params object[] parameterValues)
        {
            if (ex.Data[0] != null) return ex;
            var parameterNames = methodInfo.GetParameters();
            var parametersJs = SerializeParameters(parameterNames, parameterValues, methodInfo);
            ex.Data.Add(0, methodInfo);
            ex.Data.Add(1, parametersJs);
            ex.Data.Add(2, new StackTrace(ex, true));
            return ex;
        }

        public void LogError(Exception ex, MethodBase serviceMethodInfo, params object[] serviceParameterValues)
        {
            var option = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            try
            {

                var innerMethodBase = (MethodBase)ex.Data[0];
                var innerParameters = (string)ex.Data[1];
                var stackTrace = (StackTrace)ex.Data[2];
                int? lineNumber = null;
                if (stackTrace != null)
                {
                    lineNumber = stackTrace.GetFrame(0)?.GetFileLineNumber();
                }

                var serviceParameterNames = serviceMethodInfo.GetParameters();
                var serviceParametersJs = SerializeParameters(serviceParameterNames, serviceParameterValues, serviceMethodInfo);
                if (ex.GetType() == typeof(BusinessException))
                {
                    var itExc = (BusinessException)ex;
                    var errorLog = new HandledErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ErrorCode = itExc.Code,
                        ErrorMessage = itExc.Message,
                        ServiceName = serviceMethodInfo.DeclaringType?.FullName,
                        ServiceMethodName = serviceMethodInfo.Name,
                        ServiceParameters = serviceParametersJs,
                        InnerClassName = innerMethodBase?.DeclaringType?.FullName,
                        InnerMethodName = innerMethodBase?.Name,
                        InnerParameters = innerParameters,
                    };

                    _repoContext.HandledErrorLogs.Add(errorLog);
                    _repoContext.SaveChanges();
                }
                else // type of ex is Exception
                {

                    var errorLog = new SystemErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ExceptionStr = ex.ToString(),
                        ServiceName = serviceMethodInfo.DeclaringType?.FullName,
                        ServiceMethodName = serviceMethodInfo.Name,
                        ServiceParameters = serviceParametersJs,
                        InnerLineNumber = lineNumber,
                        InnerClassName = innerMethodBase?.DeclaringType?.FullName,
                        InnerMethodName = innerMethodBase?.Name,
                        InnerParameters = innerParameters,
                    };
                    _repoContext.SystemErrorLogs.Add(errorLog);
                    _repoContext.SaveChanges();
                }
            }
            catch (Exception e)
            {

                try
                {
                    var log = new SystemErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ExceptionStr = "********************** خطا در لاگ ارور***********" + ex.ToString(),
                        ServiceName = serviceMethodInfo.DeclaringType.FullName,
                        ServiceMethodName = serviceMethodInfo.Name,
                        ServiceParameters =System.Text.Json.JsonSerializer.Serialize(serviceParameterValues, option)
                    };

                    _repoContext.SystemErrorLogs.Add(log);
                    _repoContext.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        public void LogError(SystemErrorLog error)
        {
            _repoContext.SystemErrorLogs.Add(error);
            _repoContext.SaveChanges();

        }

        public void LogError(HandledErrorLog error)
        {
            _repoContext.HandledErrorLogs.Add(error);
            _repoContext.SaveChanges();

        }

        public void LogData(MethodBase methodInfo, object answer, TimeSpan? executeTime, params object[] parameterValues)
        {
            var option = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            try
            {

                var log = new OperationLog {Answer = answer == null ? null : JsonSerializer.Serialize(answer)};
                if (answer != null)
                {
                    object answerObj;
                    if (answer.GetType() == typeof(LogingInt))
                    {
                        answerObj = "BigData (Count: " + answer.ToString() + " )";
                    }
                    else if (answer.GetType() == typeof(LogingInt))
                    {
                        answerObj = "***";
                    }
                    else
                        answerObj = answer;

                 
           
                    log.Answer = System.Text.Json.JsonSerializer.Serialize(answerObj, option);
                }
                log.CreateDateTime = DateTime.Now;
                log.ExecuteTime = executeTime == null ? null : executeTime.ToString();
                log.MethodName = methodInfo.Name;
                log.ServiceName = methodInfo.DeclaringType?.FullName;
                log.Parameters = parameterValues != null ? SerializeParameters(methodInfo.GetParameters(), parameterValues, methodInfo) : null;

                _repoContext.OperationLogs.Add(log);
                _repoContext.SaveChanges();
            }
            catch (Exception e)
            {
                try
                {
                    try
                    {
                        var log = new SystemErrorLog()
                        {
                            CreateDateTime = DateTime.Now,
                            ExceptionStr = "********************** خطا در لاگ دیتا***********" + e.ToString(),
                            ServiceName = methodInfo.DeclaringType?.FullName,
                            ServiceMethodName = methodInfo.Name,
                           
                        ServiceParameters = System.Text.Json.JsonSerializer.Serialize(parameterValues, option)
                        };
                        _repoContext.SystemErrorLogs.Add(log);
                        _repoContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void CreateOperationLog(OperationLog operationLog)
        {
            try
            {
                _repoContext.OperationLogs.Add(operationLog);
                _repoContext.SaveChanges();
            }
            catch (Exception e)
            {
                try
                {
                    var log = new SystemErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ExceptionStr = "********************** خطا در لاگ دیتا***********" + e.ToString(),
                        ServiceName = operationLog.ServiceName,
                        ServiceMethodName = operationLog.MethodName,
                    };
                    _repoContext.SystemErrorLogs.Add(log);
                    _repoContext.SaveChanges();
                }
                catch (Exception)
                {
                }
            }
        }

        public void CreateSystemErrorLog(SystemErrorLog errorLog)
        {
            try
            {
                _repoContext.SystemErrorLogs.Add(errorLog);
                _repoContext.SaveChanges();
            }
            catch (Exception e)
            {
                try
                {
                    var log = new SystemErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ExceptionStr = "********************** خطا در لاگ ارور***********" + e.ToString(),
                        ServiceName = errorLog.ServiceName,
                        ServiceMethodName = errorLog.ServiceMethodName,
                    };
                    _repoContext.SystemErrorLogs.Add(log);
                    _repoContext.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        private string SerializeParameters(ParameterInfo[] parameterNames, object[] parameterValues, MethodBase methodInfo)
        {
            try
            {
                var parametersJs = string.Empty;
                if (parameterNames.Length == parameterValues.Length)
                {
                    Dictionary<string, Object> dictionary = new Dictionary<string, object>();
                    for (int i = 0; i < parameterValues.Length; i++)
                    {
                        if (parameterValues[i] != null && parameterValues[i].GetType() == typeof(LogingInt))
                            dictionary.Add(parameterNames[i].Name ?? string.Empty, "BigData (Count: " + parameterValues[i].ToString() + " )");
                        else if (parameterValues[i] != null && parameterValues[i].GetType() == typeof(LogingString))
                            dictionary.Add(parameterNames[i].Name ?? string.Empty, "***");
                        else
                            dictionary.Add(parameterNames[i].Name ?? string.Empty, parameterValues[i]);
                    }
                    var option = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    parametersJs = System.Text.Json.JsonSerializer.Serialize(dictionary, option);
                    //JsonConvert.SerializeObject(dictionary, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                {
                    try
                    {
                        var ss = "";
                        foreach (var item in parameterNames)
                        {
                            ss += item.Name;
                            ss += " - ";
                        }

                        var s = "";
                        foreach (var item in parameterValues)
                        {
                            s += item.ToString();
                            s += " - ";
                        }

                        SystemErrorLog log = new SystemErrorLog()
                        {
                            CreateDateTime = DateTime.Now,
                            ExceptionStr = "********************** ایراد در پارامترهای لاگ ***************(parameterNames.Length =" + ss + "parameterValues.Length= " + s,
                            ServiceName = methodInfo.DeclaringType?.FullName,
                            ServiceMethodName = methodInfo.Name,
                        };
                        _repoContext.SystemErrorLogs.Add(log);
                        _repoContext.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }
                }
                return parametersJs;
            }
            catch (Exception ex)
            {
                try
                {
                    SystemErrorLog log = new SystemErrorLog()
                    {
                        CreateDateTime = DateTime.Now,
                        ExceptionStr = "********************** خطا در سریالایز لاگ ***************" + ex.ToString(),
                        ServiceName = methodInfo.DeclaringType?.FullName,
                        ServiceMethodName = methodInfo.Name,
                    };
                    _repoContext.SystemErrorLogs.Add(log);
                    _repoContext.SaveChanges();
                }
                catch (Exception e)
                {

                }
                return "خطا";
            }
        }

        public class LogingInt
        {
            public int Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class LogingString
        {

        }
    }
}
