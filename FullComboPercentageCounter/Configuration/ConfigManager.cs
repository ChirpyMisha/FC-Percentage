using System;
using BeatSaberMarkupLanguage.Settings;
using Zenject;

namespace FullComboPercentageCounter.Configuration
{
	class ConfigManager : IInitializable, IDisposable
	{
		private ConfigController configHost;

		[Inject]
		public ConfigManager(ConfigController configHost)
		{
			this.configHost = configHost;
		}

		public void Initialize()
		{
			BSMLSettings.instance.AddSettingsMenu(Plugin.PluginName, "FullComboPercentageCounter.Configuration.BSML.SettingsFCScorePercentage.bsml", configHost);
		}

		public void Dispose()
		{
			if (configHost == null)
				return;

			BSMLSettings.instance.RemoveSettingsMenu(configHost);
			configHost = null!;
		}
	}
}
