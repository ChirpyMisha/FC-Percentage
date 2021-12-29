using IPA;
using SiraUtil.Zenject;
using FCPercentage.Installers;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using FCPercentage.Configuration;

namespace FCPercentage
{
	[Plugin(RuntimeOptions.SingleStartInit)]
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

			zenjector.Install<AppInstaller>(Location.App);
			zenjector.Install<MenuInstaller>(Location.Menu);
			zenjector.Install<GameInstaller>(Location.GameCore);

			Log.Info($"{PluginName} initialized.");
		}

		[Init]
		public void InitWithConfig(Config conf)
		{
			PluginConfig.Instance = conf.Generated<PluginConfig>();
			Log.Debug("Config loaded");
		}

		[OnStart]
		public void OnApplicationStart()
		{
			Log.Debug("OnApplicationStart");
		}

		[OnExit]
		public void OnApplicationQuit()
		{
			Log.Debug("OnApplicationQuit");
		}
	}
}
