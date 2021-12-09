using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Aspects.Autofac.LogAspects
{
    public class LogAspect:MethodInterception
    {
        private LoggerService _loggerService;

        public LogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerService))
            {
                throw new System.Exception("");
            }

            _loggerService = (LoggerService)Activator.CreateInstance(loggerService);
        }
        protected override void OnBefore(IInvocation invocation)
        {
            _loggerService.Info(GetLogDetail(invocation));
        }
        private LogDetail GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name
                });
            }

            var logDetail = new LogDetail
            {
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                FullName=invocation.Method.DeclaringType==null?null:invocation.Method.DeclaringType.Name
            };

            return logDetail;
        }
    }
}
