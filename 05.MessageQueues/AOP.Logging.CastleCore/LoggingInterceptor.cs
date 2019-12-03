using Castle.DynamicProxy;
using Newtonsoft.Json;
using NLog;

namespace AOP.Logging.CastleCore
{
    public class LoggingInterceptor : IInterceptor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;

            string args = invocation.Arguments.Length > 0 ?
                $"with {invocation.Arguments.Length} arguments:" :
                "without arguments.";
            Logger.Trace($"Invoking method '{methodName}' {args}");
            foreach (var argument in invocation.Arguments)
            {
                var serialized = JsonConvert.SerializeObject(argument);
                Logger.Trace(serialized);
            }

            invocation.Proceed();

            Logger.Trace($"Method '{methodName}' completed.");
        }
    }
}
