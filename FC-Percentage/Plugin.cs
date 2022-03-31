using IPA;
using SiraUtil.Zenject;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using Zenject;
using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using FCPercentage.FCPCore.Configuration;
using FCPercentage.FCPResults.HUD;

namespace FCPercentage
{
	[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
	public class Plugin
	{
#pragma warning disable CS8618
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }
#pragma warning restore CS8618

		internal const string PluginName = "FCPercentage";

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Config conf, Zenjector zenjector)
		{
			logger.Notice("Starting FCPercentage");
			Instance = this;
			Log = logger;
			PluginConfig.Instance = conf.Generated<PluginConfig>();
			logger.Notice("PluginConfig Generated");

			zenjector.Install(Location.App, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
			});
			zenjector.Install(Location.GameCore, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
			});
			zenjector.Install(Location.Menu, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ResultsConfigController>().AsCached();
				Container.BindInterfacesAndSelfTo<ResultsConfigManager>().AsSingle();
				Container.BindInterfacesAndSelfTo<FCPResultsViewController>().AsSingle();
				Container.BindInterfacesAndSelfTo<FCPMissionResultsViewController>().AsSingle();
			});

			Log.Info($"{PluginName} initialized.");
		}
	}
}
