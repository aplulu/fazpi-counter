using System;
using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil;
using SiraUtil.Zenject;


namespace FaZPiCounter
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPA.Logging.Logger logger, IPA.Config.Config config, Zenjector injector, PluginMetadata metadata)
        {
            var conf = config.Generated<PluginConfig>();
            injector.On<PCAppInit>().Pseudo(container =>
            {
                container.BindLoggerAsSiraLogger(logger);
                container.BindInstance(conf).AsSingle();
            });
        }
    }
}