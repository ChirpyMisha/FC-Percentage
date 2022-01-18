using IPA;
using SiraUtil.Zenject;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using Zenject;
using FCPercentage.FCPCore;
using FCPercentage.FCPResults.Configuration;
using FCPercentage.FCPResults;
using FCPercentage.FCPCore.Configuration;

namespace FCPercentage
{
	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin
	{
#pragma warning disable CS8618
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }
#pragma warning restore CS8618

		internal static string PluginName = "FCPercentage";

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Zenjector zenjector)
		{
			Instance = this;
			Log = logger;

			zenjector.Install(Location.App, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
			});
			zenjector.Install(Location.GameCore, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<NoteRatingTracker>().AsSingle();
				Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
			});
			zenjector.Install(Location.Menu, (DiContainer Container) =>
			{
				Container.BindInterfacesAndSelfTo<ResultsConfigController>().AsCached();
				Container.BindInterfacesAndSelfTo<ResultsConfigManager>().AsSingle();
				Container.BindInterfacesAndSelfTo<FCPResultsViewController>().AsSingle();
			});

			Log.Info($"{PluginName} initialized.");
		}

		[Init]
		public void InitWithConfig(Config conf)
		{
			PluginConfig.Instance = conf.Generated<PluginConfig>();
		}

		[OnEnable, OnDisable]
		public void OnApplicationStateChange()
		{
			// Just here to avoid BSIPA compaining~
		}
	}
}
