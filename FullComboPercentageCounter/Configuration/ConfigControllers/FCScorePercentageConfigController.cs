using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullComboPercentageCounter.Configuration
{
	class FCScorePercentageConfigController
	{
		private SettingsFCScorePercentage FcScorePercentageSettings => PluginConfig.Instance.FcScorePercentageSettings;
		//private FormatSettingsFCScorePercentage FormatSettings => FcScorePercentageSettings.FormatSettings;

		[UIValue("TotalPercentageMode")]
		public virtual ResultsViewModes TotalPercentageMode
		{
			get { return FcScorePercentageSettings.TotalPercentageMode; }
			set { FcScorePercentageSettings.TotalPercentageMode = value; }
		}

		[UIValue("SplitPercentageMode")]
		public virtual ResultsViewModes SplitPercentageMode
		{
			get { return FcScorePercentageSettings.SplitPercentageMode; }
			set { FcScorePercentageSettings.SplitPercentageMode = value; }
		}

		[UIValue("TotalScoreMode")]
		public virtual ResultsViewModes TotalScoreMode
		{
			get { return FcScorePercentageSettings.TotalScoreMode; }
			set { FcScorePercentageSettings.TotalScoreMode = value; }
		}

		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return FcScorePercentageSettings.DecimalPrecision; }
			set { FcScorePercentageSettings.DecimalPrecision = value; }
		}

		[UIValue("EnableScorePercentageDifference")]
		public virtual bool EnableScorePercentageDifference
		{
			get { return FcScorePercentageSettings.EnableScorePercentageDifference; }
			set { FcScorePercentageSettings.EnableScorePercentageDifference = value; }
		}

		[UIValue("EnableLabel")]
		public virtual ResultsViewLabelOptions EnableLabel
		{
			get { return FcScorePercentageSettings.EnableLabel; }
			set { FcScorePercentageSettings.EnableLabel = value; }
		}

		[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get { return FcScorePercentageSettings.SplitPercentageUseSaberColorScheme; }
			set { FcScorePercentageSettings.SplitPercentageUseSaberColorScheme = value; }
		}

		[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get { return FcScorePercentageSettings.KeepTrailingZeros; }
			set { FcScorePercentageSettings.KeepTrailingZeros = value; }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
		}




		[UIValue(nameof(ResultsViewModeList))]
		public List<object> ResultsViewModeList => ResultsViewModesToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewModesFormat))]
		public string ResultsViewModesFormat(ResultsViewModes mode) => ResultsViewModesToNames[mode];

		private static Dictionary<ResultsViewModes, string> ResultsViewModesToNames = new Dictionary<ResultsViewModes, string>()
		{
			{ResultsViewModes.On, "On" },
			{ResultsViewModes.OffWhenFC, "Off When FC" },
			{ResultsViewModes.Off, "Off" }
		};

		[UIValue(nameof(ResultsViewLabelOptionList))]
		public List<object> ResultsViewLabelOptionList => ResultsViewLabelOptionsToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewLabelOptionsFormat))]
		public string ResultsViewLabelOptionsFormat(ResultsViewLabelOptions option) => ResultsViewLabelOptionsToNames[option];

		private static Dictionary<ResultsViewLabelOptions, string> ResultsViewLabelOptionsToNames = new Dictionary<ResultsViewLabelOptions, string>()
		{
			{ResultsViewLabelOptions.BothOn, "Both Labels" },
			{ResultsViewLabelOptions.ScoreOn, "Only Score Label" },
			{ResultsViewLabelOptions.PercentageOn, "Only Percentage Label" },
			{ResultsViewLabelOptions.BothOff, "No Labels" }
		};
	}
}
