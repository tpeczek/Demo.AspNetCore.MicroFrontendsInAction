using EsiNet;
using EsiNet.Logging;
using EsiNet.Pipeline;
using EsiNet.Fragments;
using EsiNet.Fragments.Try;
using EsiNet.Fragments.Text;
using EsiNet.Fragments.Vars;
using EsiNet.Fragments.Choose;
using EsiNet.Fragments.Ignore;
using EsiNet.Fragments.Composite;
using Microsoft.Extensions.DependencyInjection.Extensions;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEsi(this IServiceCollection services)
        {
            services.TryAddSingleton<EsiBodyParser>(CreateEsiBodyParser);
            services.TryAddSingleton<EsiFragmentExecutor>(CreateEsiFragmentExecutor);

            return services;
        }

        private static EsiBodyParser CreateEsiBodyParser(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

            var esiFragmentParsers = new Dictionary<string, IEsiFragmentParser>();
            var esiFragmentParsePipelines = Enumerable.Empty<IFragmentParsePipeline>();

            var esiFragmentParser = new EsiFragmentParser(esiFragmentParsers, esiFragmentParsePipelines);
            var esiBodyParser = new EsiBodyParser(esiFragmentParser);

            esiFragmentParsers["esi:text"] = new EsiTextParser();

            //esiFragmentParsers["esi:include"] = new EsiIncludeParser();
            esiFragmentParsers["esi:choose"] = new EsiChooseParser(esiBodyParser);
            esiFragmentParsers["esi:try"] = new EsiTryParser(esiBodyParser);
            esiFragmentParsers["esi:comment"] = new EsiIgnoreParser();
            esiFragmentParsers["esi:remove"] = new EsiIgnoreParser();
            esiFragmentParsers["esi:vars"] = new EsiVarsParser();

            return esiBodyParser;
        }

        public static EsiFragmentExecutor CreateEsiFragmentExecutor(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

            var esiExecutors = new Dictionary<Type, Func<IEsiFragment, EsiExecutionContext, Task<IEnumerable<string>>>>();
            var esiFragmentExecutor = new EsiFragmentExecutor(esiExecutors, serviceProvider.GetService);

            var esiCompositeExecutor = new EsiCompositeFragmentExecutor(esiFragmentExecutor);
            esiExecutors[typeof(EsiCompositeFragment)] = (f, ec) => esiCompositeExecutor.Execute((EsiCompositeFragment)f, ec);

            var esiTextExecutor = new EsiTextFragmentExecutor();
            esiExecutors[typeof(EsiTextFragment)] = (f, ec) => esiTextExecutor.Execute((EsiTextFragment)f, ec);

            var esiIgnoreExecutor = new EsiIgnoreFragmentExecutor();
            esiExecutors[typeof(EsiIgnoreFragment)] = (f, ec) => esiIgnoreExecutor.Execute((EsiIgnoreFragment)f, ec);

            var esiChooseExecutor = new EsiChooseFragmentExecutor(esiFragmentExecutor);
            esiExecutors[typeof(EsiChooseFragment)] = (f, ec) => esiChooseExecutor.Execute((EsiChooseFragment)f, ec);

            var esiTryExecutor = new EsiTryFragmentExecutor(esiFragmentExecutor, CreateEsiTryExecutorLog(serviceProvider));
            esiExecutors[typeof(EsiTryFragment)] = (f, ec) => esiTryExecutor.Execute((EsiTryFragment)f, ec);

            var esiVarsExecutor = new EsiVarsFragmentExecutor();
            esiExecutors[typeof(EsiVarsFragment)] = (f, ec) => esiVarsExecutor.Execute((EsiVarsFragment)f, ec);

            return esiFragmentExecutor;
        }

        private static Log CreateEsiTryExecutorLog(IServiceProvider serviceProvider)
        {
            var esiTryExecutorLogger = serviceProvider.GetRequiredService<ILogger<EsiTryFragmentExecutor>>();

            return (esiLogLevel, exception, message) =>
            {
                LogLevel logLevel = esiLogLevel switch
                {
                    EsiNet.Logging.LogLevel.Debug => LogLevel.Debug,
                    EsiNet.Logging.LogLevel.Information => LogLevel.Information,
                    EsiNet.Logging.LogLevel.Warning => LogLevel.Warning,
                    EsiNet.Logging.LogLevel.Error => LogLevel.Error,
                    _ => throw new NotSupportedException($"Unknown ESI log level '{esiLogLevel}'.")
                };

                esiTryExecutorLogger.Log(logLevel, exception, message());
            };
        }
    }
}
