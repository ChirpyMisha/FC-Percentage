using IPA;
using SiraUtil.Zenject;
using FullComboPercentageCounter.Installers;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using IPA.Loader;
using FullComboPercentageCounter.Configuration;
using System.Reflection;

namespace FullComboPercentageCounter
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }

		private static PluginMetadata metadata;
		private static string name;
		internal static string PluginName => name ??= metadata?.Name ?? Assembly.GetExecutingAssembly().GetName().Name;

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, PluginMetadata metaData, Zenjector zenjector)
		{
			Instance = this;
			Log = logger;

			metadata = metaData;

			zenjector.OnApp<AppInstaller>();
			zenjector.OnMenu<MenuInstaller>();
			zenjector.OnGame<GameInstaller>(false);

			Log.Info("FullComboPercentageCounter initialized.");
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
