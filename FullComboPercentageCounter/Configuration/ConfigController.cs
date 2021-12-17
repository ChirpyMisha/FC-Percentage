using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FullComboPercentageCounter.Configuration
{
	class ConfigController
	{
		#region Shared Settings (Custom Counters+ Counter & Results View)
		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return PluginConfig.Instance.DecimalPrecision; }
			set { PluginConfig.Instance.DecimalPrecision = value; }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
		}

		#endregion


		#region FCPercentageCounter Settings (Custom Counters+ Counter)
		[UIValue("PercentageSize")]
		public virtual float PercentageSize
		{
			get { return PluginConfig.Instance.PercentageSize; }
			set { PluginConfig.Instance.PercentageSize = value; }
		}

		[UIValue("EnableLabel_Counter")]
		public virtual bool EnableLabel_Counter
		{
			get { return PluginConfig.Instance.EnableLabel_Counter; }
			set { PluginConfig.Instance.EnableLabel_Counter = value; }
		}

		[UIValue("LabelAboveCount")]
		public virtual bool LabelAboveCount
		{
			get { return PluginConfig.Instance.LabelAboveCount; }
			set { PluginConfig.Instance.LabelAboveCount = value; }
		}

		#endregion


		#region FCScorePercentage Settings (Results View)
		[UIValue("ResultsViewMode")]
		public virtual ResultsViewModes ResultsViewMode
		{
			get { return PluginConfig.Instance.ResultsViewMode; }
			set { PluginConfig.Instance.ResultsViewMode = value; }
		}

		[UIValue("EnableScorePercentageDifference")]
		public virtual bool EnableScorePercentageDifference
		{
			get { return PluginConfig.Instance.EnableScorePercentageDifference; }
			set { PluginConfig.Instance.EnableScorePercentageDifference = value; }
		}

		[UIValue("EnableLabel_ScorePercentage")]
		public virtual bool EnableLabel_ScorePercentage
		{
			get { return PluginConfig.Instance.EnableLabel_ScorePercentage; }
			set { PluginConfig.Instance.EnableLabel_ScorePercentage = value; }
		}

		#endregion


		[UIValue(nameof(ResultsViewModeList))]
		public List<object> ResultsViewModeList => ResultsViewModesToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewModesFormat))]
		public string ResultsViewModesFormat(ResultsViewModes mode) => ResultsViewModesToNames[mode];

		private static Dictionary<ResultsViewModes, string> ResultsViewModesToNames = new Dictionary<ResultsViewModes, string>()
		{
			{ResultsViewModes.On, "On" },
			{ResultsViewModes.OffWhenFullCombo, "Off When FC" },
			{ResultsViewModes.Off, "Off" }
		};
	}
}
