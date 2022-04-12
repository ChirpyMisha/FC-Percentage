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
		internal const string PluginName = "FCPercentage";

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Config conf, Zenjector zenject)
		{
			zenject.UseLogger(logger);
			zenject.UseMetadataBinder<Plugin>();

			PluginConfig.Instance = conf.Generated<PluginConfig>();

			// Install zenject stuff
			zenject.Install(Location.App, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
			});
			zenject.Install(Location.GameCore, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
			});
			zenject.Install(Location.Menu, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ResultsConfigController>().AsCached();
				Container.BindInterfacesAndSelfTo<ResultsConfigManager>().AsSingle();
				Container.BindInterfacesAndSelfTo<FCPResultsViewController>().AsSingle();
				Container.BindInterfacesAndSelfTo<FCPMissionResultsViewController>().AsSingle();
			});
		}
	}
}
