using Newtonsoft.Json;
using NLog;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Consumer
{
    [PSerializable]
    public class LoggingAspectAttribute : OnMethodBoundaryAspect
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodName = args.Method.Name;

            string arguments = args.Arguments.Count > 0 ?
                $"with {args.Arguments.Count} arguments:" :
                "without arguments.";
            Logger.Trace($"Invoking method '{methodName}' {arguments}");
            foreach (var arg in args.Arguments)
            {
                var serializedArg = JsonConvert.SerializeObject(arg);
                Logger.Trace(serializedArg);
            }
            
            args.FlowBehavior = FlowBehavior.Default;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var methodName = args.Method.Name;
            Logger.Trace($"Method '{methodName}' completed successfully.");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            var methodName = args.Method.Name;
            Logger.Trace($"Method '{methodName}' completed with Exception: {args.Exception.Message}.");
        }
    }
}
