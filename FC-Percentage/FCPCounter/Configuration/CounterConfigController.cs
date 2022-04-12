using BeatSaberMarkupLanguage.Attributes;
using FCPercentage.FCPCore.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FCPercentage.FCPCounter.Configuration
{
	class CounterConfigController
	{
		private CounterSettings settings => PluginConfig.Instance.CounterSettings;

		[UIValue("PercentageMode")]
		public virtual CounterPercentageModes PercentageMode
		{
			get => settings.PercentageMode;
			set => settings.PercentageMode = value;
		}

		[UIValue("EnableLabel")]
		public virtual CounterLabelOptions EnableLabel
		{
			get => settings.EnableLabel;
			set => settings.EnableLabel = value;
		}

		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get => settings.DecimalPrecision;
			set => settings.DecimalPrecision = value;
		}

		[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get => settings.SplitPercentageUseSaberColorScheme;
			set => settings.SplitPercentageUseSaberColorScheme = value;
		}

		[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get => settings.KeepTrailingZeros;
			set => settings.KeepTrailingZeros = value;
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get => PluginConfig.Instance.IgnoreMultiplier;
			set => PluginConfig.Instance.IgnoreMultiplier = value;
		}

		// Advanced Settings
		[UIValue("CounterOffset")]
		public virtual float CounterOffset
		{
			get => settings.Advanced.CounterOffset;
			set =>  settings.Advanced.CounterOffset = value;
		}

		[UIValue("LabelAboveCounterTextOffset")]
		public virtual float LabelAboveCounterTextOffset
		{
			get => settings.Advanced.LabelAboveCounterTextOffset;
			set => settings.Advanced.LabelAboveCounterTextOffset = value;
		}

		[UIValue("LabelAboveCounterTextSize")]
		public virtual float LabelAboveCounterTextSize
		{
			get => settings.Advanced.LabelAboveCounterTextSize;
			set => settings.Advanced.LabelAboveCounterTextSize = value;
		}

		[UIValue("PercentageSize")]
		public virtual float PercentageSize
		{
			get => settings.Advanced.PercentageSize;
			set => settings.Advanced.PercentageSize = value;
		}

		[UIValue("PercentageTotalAndSplitLineHeight")]
		public virtual float PercentageTotalAndSplitLineHeight
		{
			get => settings.Advanced.PercentageTotalAndSplitLineHeight;
			set => settings.Advanced.PercentageTotalAndSplitLineHeight = value;
		}

		[UIValue("LabelAboveCounterText")]
		public virtual string LabelAboveCounterText
		{
			get => settings.Advanced.LabelAboveCounterText;
			set => settings.Advanced.LabelAboveCounterText = value;
		}

		[UIValue("LabelPrefixText")]
		public virtual string LabelPrefixText
		{
			get => settings.Advanced.LabelPrefixText;
			set => settings.Advanced.LabelPrefixText = value;
		}

		[UIValue("PercentageSplitSaberAPrefixText")]
		public virtual string PercentageSplitSaberAPrefixText
		{
			get => settings.Advanced.PercentageSplitSaberAPrefixText;
			set => settings.Advanced.PercentageSplitSaberAPrefixText = value;
		}

		[UIValue("PercentageSplitSaberBPrefixText")]
		public virtual string PercentageSplitSaberBPrefixText
		{
			get => settings.Advanced.PercentageSplitSaberBPrefixText;
			set => settings.Advanced.PercentageSplitSaberBPrefixText = value;
		}




		[UIValue(nameof(CounterPercentageModeList))]
		public List<object> CounterPercentageModeList => CounterPercentageModesToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(CounterPercentageModesFormat))]
		public string CounterPercentageModesFormat(CounterPercentageModes mode) => CounterPercentageModesToNames[mode];

		private static Dictionary<CounterPercentageModes, string> CounterPercentageModesToNames = new Dictionary<CounterPercentageModes, string>()
		{
			{CounterPercentageModes.Total, "FC Percentage" },
			{CounterPercentageModes.Split, "Split FC Percentage" },
			{CounterPercentageModes.TotalAndSplit, "Both" }
		};

		[UIValue(nameof(CounterLabelOptionList))]
		public List<object> CounterLabelOptionList => CounterLabelOptionsToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(CounterLabelOptionsFormat))]
		public string CounterLabelOptionsFormat(CounterLabelOptions option) => CounterLabelOptionsToNames[option];

		private static Dictionary<CounterLabelOptions, string> CounterLabelOptionsToNames = new Dictionary<CounterLabelOptions, string>()
		{
			{CounterLabelOptions.AsPrefix, "Label As Prefix" },
			{CounterLabelOptions.AboveCounter, "Label Above Counter" },
			{CounterLabelOptions.Off, "Label Off" }
		};
	}
}
