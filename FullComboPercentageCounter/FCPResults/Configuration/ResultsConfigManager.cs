using System;
using BeatSaberMarkupLanguage.Settings;
using Zenject;

namespace FullComboPercentageCounter.Configuration
{
	class ResultsConfigManager : IInitializable, IDisposable
	{
		private ResultsConfigController configHost;

		[Inject]
		public ResultsConfigManager(ResultsConfigController configHost)
		{
			this.configHost = configHost;
		}

		public void Initialize()
		{
			BSMLSettings.instance.AddSettingsMenu(Plugin.PluginName, "FullComboPercentageCounter.FCPResults.Configuration.BSML.ResultsSettings.bsml", configHost);
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
