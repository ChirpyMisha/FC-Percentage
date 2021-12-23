using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FullComboPercentageCounter.Configuration
{
	class ResultsConfigController
	{
		private ResultsSettings settings => PluginConfig.Instance.ResultsSettings;

		[UIValue("PercentageTotalMode")]
		public virtual ResultsViewModes PercentageTotalMode
		{
			get { return settings.PercentageTotalMode; }
			set { settings.PercentageTotalMode = value; }
		}

		[UIValue("PercentageSplitMode")]
		public virtual ResultsViewModes PercentageSplitMode
		{
			get { return settings.PercentageSplitMode; }
			set { settings.PercentageSplitMode = value; }
		}

		[UIValue("ScoreTotalMode")]
		public virtual ResultsViewModes ScoreTotalMode
		{
			get { return settings.ScoreTotalMode; }
			set { settings.ScoreTotalMode = value; }
		}

		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return settings.DecimalPrecision; }
			set { settings.DecimalPrecision = value; }
		}

		[UIValue("EnableScorePercentageDifference")]
		public virtual bool EnableScorePercentageDifference
		{
			get { return settings.EnableScorePercentageDifference; }
			set { settings.EnableScorePercentageDifference = value; }
		}

		[UIValue("EnableLabel")]
		public virtual ResultsViewLabelOptions EnableLabel
		{
			get { return settings.EnableLabel; }
			set { settings.EnableLabel = value; }
		}

		[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get { return settings.SplitPercentageUseSaberColorScheme; }
			set { settings.SplitPercentageUseSaberColorScheme = value; }
		}

		[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get { return settings.KeepTrailingZeros; }
			set { settings.KeepTrailingZeros = value; }
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
			{ResultsViewModes.On, "Show Always" },
			{ResultsViewModes.OffWhenFC, "Hide When FC" },
			{ResultsViewModes.Off, "Hide Always" }
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
