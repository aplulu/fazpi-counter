using BeatSaberMarkupLanguage.Attributes;
using Zenject;

namespace FaZPiCounter.UI
{
    public class SettingsController
    {
        [Inject] private PluginConfig _config = null;

        [UIValue("belowThreshold")]
        public int BelowThreshold
        {
            get => _config.belowThreshold;
            set => _config.belowThreshold = value;
        }
    }
}