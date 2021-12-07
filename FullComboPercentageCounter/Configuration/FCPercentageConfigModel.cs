using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FullComboPercentageCounter.Configuration
{
	public class FCPercentageConfigModel
	{
		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return PluginConfig.Instance.DecimalPrecision; }
			set { PluginConfig.Instance.DecimalPrecision = value; }
		}

		[UIValue("PercentageSize")]
		public virtual float PercentageSize
		{
			get { return PluginConfig.Instance.PercentageSize; }
			set { PluginConfig.Instance.PercentageSize = value; }
		}

		[UIValue("EnableLabel")]
		public virtual bool EnableLabel
		{
			get { return PluginConfig.Instance.EnableLabel; }
			set { PluginConfig.Instance.EnableLabel = value; }
		}
		
		[UIValue("LabelAboveCount")]
		public virtual bool LabelAboveCount
		{
			get { return PluginConfig.Instance.LabelAboveCount; }
			set { PluginConfig.Instance.LabelAboveCount = value; }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
		}

		[UIValue("ResultsViewMode")]
		public virtual ResultsViewModes ResultsViewMode { 
			get { return PluginConfig.Instance.ResultsViewMode; }
			set { PluginConfig.Instance.ResultsViewMode = value; }
		}

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
