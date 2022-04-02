using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using FCPercentage.FCPCounter.Configuration;
using FCPercentage.FCPResults.Configuration;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace FCPercentage.FCPCore.Configuration
{
	internal class PluginConfig
	{
#pragma warning disable CS8618
		public static PluginConfig Instance { get; set; }
#pragma warning restore CS8618

		// Settings Category Objects
		public virtual CounterSettings CounterSettings { get; set; } = new CounterSettings();
		public virtual ResultsSettings ResultsSettings { get; set; } = new ResultsSettings();
		public virtual ResultsSettings MissionResultsSettings { get; set; } = new ResultsSettings();


		// Shared Settings (Custom Counters+ Counter & Results View)
		public virtual bool IgnoreMultiplier { get; set; } = false;
		


		/// <summary>
		/// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
		/// </summary>
		public virtual void OnReload()
		{
			// Do stuff after config is read from disk.
		}

		/// <summary>
		/// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
		/// </summary>
		public virtual void Changed()
		{
			// Do stuff when the config is changed.
		}

		/// <summary>
		/// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
		/// </summary>
		public virtual void CopyFrom(PluginConfig other)
		{
			// This instance's members populated from other
		}
	}
}
