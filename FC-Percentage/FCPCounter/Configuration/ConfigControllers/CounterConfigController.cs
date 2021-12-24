using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FCPercentage.Configuration
{
	class CounterConfigController
	{
		private CounterSettings FcCounterSettings => PluginConfig.Instance.CounterSettings;


		[UIValue("PercentageMode")]
		public virtual CounterPercentageModes PercentageMode
		{
			get { return FcCounterSettings.PercentageMode; }
			set { FcCounterSettings.PercentageMode = value; }
		}

		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return FcCounterSettings.DecimalPrecision; }
			set { FcCounterSettings.DecimalPrecision = value; }
		}

		[UIValue("PercentageSize")]
		public virtual float PercentageSize
		{
			get { return FcCounterSettings.PercentageSize; }
			set { FcCounterSettings.PercentageSize = value; }
		}

		[UIValue("EnableLabel")]
		public virtual CounterLabelOptions EnableLabel
		{
			get { return FcCounterSettings.EnableLabel; }
			set { FcCounterSettings.EnableLabel = value; }
		}

		[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get { return FcCounterSettings.SplitPercentageUseSaberColorScheme; }
			set { FcCounterSettings.SplitPercentageUseSaberColorScheme = value; }
		}

		[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get { return FcCounterSettings.KeepTrailingZeros; }
			set { FcCounterSettings.KeepTrailingZeros = value; }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
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
