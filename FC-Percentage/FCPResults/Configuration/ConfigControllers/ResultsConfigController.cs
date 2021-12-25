using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FCPercentage.Configuration
{
	class ResultsConfigController : INotifyPropertyChanged
	{
		private static string enabledTextColor = "#" + ColorUtility.ToHtmlStringRGB(Color.white);
		private static string disabledTextColor = "#" + ColorUtility.ToHtmlStringRGB(Color.grey);

		private ResultsSettings settings => PluginConfig.Instance.ResultsSettings;

		public event PropertyChangedEventHandler PropertyChanged;

		private ResultsViewModes percentageTotalMode;
		private ResultsViewModes percentageSplitMode;
		private ResultsViewModes scoreTotalMode;
		private ResultsViewLabelOptions enableLabel;

		public ResultsConfigController()
		{
			LoadSettingsIntoLocals();
		}

		private void LoadSettingsIntoLocals()
		{
			percentageTotalMode = settings.PercentageTotalMode;
			percentageSplitMode = settings.PercentageSplitMode;
			scoreTotalMode = settings.ScoreTotalMode;
			enableLabel = settings.EnableLabel;
			//UpdateInteractability();
		}

		private void RevertChanges()
		{
            PercentageTotalMode = percentageTotalMode;
            PercentageSplitMode = percentageSplitMode;
            ScoreTotalMode = scoreTotalMode;
			EnableLabel = enableLabel;
        }

		private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[UIValue("is-any-on")]
		private bool IsAnyOn => (IsAnyPercentOn || IsScoreTotalOn);
		[UIValue("is-any-percent-on")]
		private bool IsAnyPercentOn => (IsPercentageTotalOn || IsPercentageSplitOn);
		[UIValue("is-percentage-total-on")]
		private bool IsPercentageTotalOn => settings.PercentageTotalMode != ResultsViewModes.Off;
		[UIValue("is-percentage-split-on")]
		private bool IsPercentageSplitOn => settings.PercentageSplitMode != ResultsViewModes.Off;
		[UIValue("is-score-total-on")]
		private bool IsScoreTotalOn => settings.ScoreTotalMode != ResultsViewModes.Off;
		[UIValue("score-prefix-text-active")]
		private bool ScorePrefixTextActive => IsScoreTotalOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.ScoreOn);
		[UIValue("percentage-prefix-text-active")]
		private bool PercentagePrefixTextActive => IsAnyPercentOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.PercentageOn);


		[UIValue("is-any-on-color")]
		private string IsAnyOnColor => IsAnyOn ? enabledTextColor : disabledTextColor;
		[UIValue("is-any-percent-on-color")]
		private string IsAnyPercentOnColor => IsAnyPercentOn ? enabledTextColor : disabledTextColor;
		[UIValue("is-percentage-total-on-color")]
		private string IsPercentageTotalOnColor => IsPercentageTotalOn ? enabledTextColor : disabledTextColor;
		[UIValue("is-percentage-split-on-color")]
		private string IsPercentageSplitOnColor => IsPercentageSplitOn ? enabledTextColor : disabledTextColor;
		[UIValue("is-score-total-on-color")]
		private string IsScoreTotalOnColor => IsScoreTotalOn ? enabledTextColor : disabledTextColor;
		[UIValue("score-prefix-text-active-color")]
		private string ScorePrefixTextActiveColor => ScorePrefixTextActive ? enabledTextColor : disabledTextColor;
		[UIValue("percentage-prefix-text-active-color")]
		private string PercentagePrefixTextActiveColor => PercentagePrefixTextActive ? enabledTextColor : disabledTextColor;

		private void UpdateInteractability()
		{
			RaisePropertyChanged(nameof(IsAnyOn));
			RaisePropertyChanged(nameof(IsAnyPercentOn));
			RaisePropertyChanged(nameof(IsPercentageTotalOn));
			RaisePropertyChanged(nameof(IsPercentageSplitOn));
			RaisePropertyChanged(nameof(IsScoreTotalOn));
			RaisePropertyChanged(nameof(ScorePrefixTextActive));
			RaisePropertyChanged(nameof(PercentagePrefixTextActive));

			UpdateTextColors();
		}

		private void UpdateTextColors()
		{
			RaisePropertyChanged(nameof(IsAnyOnColor));
			RaisePropertyChanged(nameof(IsAnyPercentOnColor));
			RaisePropertyChanged(nameof(IsPercentageTotalOnColor));
			RaisePropertyChanged(nameof(IsPercentageSplitOnColor));
			RaisePropertyChanged(nameof(IsScoreTotalOnColor));
			RaisePropertyChanged(nameof(ScorePrefixTextActiveColor));
			RaisePropertyChanged(nameof(PercentagePrefixTextActiveColor));
		}


        //[UIAction("#apply")]
		//public void OnApply()
		//{
		//	Plugin.Log.Notice("Saving percentage & score modes from local values.");
		//	LoadSettingsIntoLocals();
		//}

		[UIAction("#cancel")]
		public void OnCancel()
		{
			Plugin.Log.Notice("Reverting changes. Setting local percentage & score modes from settings.");
			RevertChanges();
        }

		


		[UIValue("PercentageTotalMode")]
		public virtual ResultsViewModes PercentageTotalMode
		{
			get { return settings.PercentageTotalMode; }
			set 
			{
				settings.PercentageTotalMode = value;
				RaisePropertyChanged();
				UpdateInteractability();
			}
		}

		[UIValue("PercentageSplitMode")]
		public virtual ResultsViewModes PercentageSplitMode
		{
			get { return settings.PercentageSplitMode; }
			set 
			{
				settings.PercentageSplitMode = value;
				RaisePropertyChanged();
				UpdateInteractability();
			}
		}

		[UIValue("ScoreTotalMode")]
		public virtual ResultsViewModes ScoreTotalMode
		{
			get { return settings.ScoreTotalMode; }
			set 
			{
				settings.ScoreTotalMode = value;
				RaisePropertyChanged();
				UpdateInteractability();
			}
		}

		[UIValue("EnableLabel")]
		public virtual ResultsViewLabelOptions EnableLabel
		{
			get { return settings.EnableLabel; }
			set
			{ 
				settings.EnableLabel = value;
				RaisePropertyChanged();
				UpdateInteractability();
			}
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

		// Advanced Settings
		[UIValue("ScorePrefixText")]
		public virtual string ScorePrefixText
		{
			get { return settings.Advanced.ScorePrefixText; }
			set { settings.Advanced.ScorePrefixText = value; }
		}

		[UIValue("PercentagePrefixText")]
		public virtual string PercentagePrefixText
		{
			get { return settings.Advanced.PercentagePrefixText; }
			set { settings.Advanced.PercentagePrefixText = value; }
		}

		[UIValue("PercentageTotalPrefixText")]
		public virtual string PercentageTotalPrefixText
		{
			get { return settings.Advanced.PercentageTotalPrefixText; }
			set { settings.Advanced.PercentageTotalPrefixText = value; }
		}

		[UIValue("PercentageSplitSaberAPrefixText")]
		public virtual string PercentageSplitSaberAPrefixText
		{
			get { return settings.Advanced.PercentageSplitSaberAPrefixText; }
			set	{ settings.Advanced.PercentageSplitSaberAPrefixText = value; }
		}

		[UIValue("PercentageSplitSaberBPrefixText")]
		public virtual string PercentageSplitSaberBPrefixText
		{
			get { return settings.Advanced.PercentageSplitSaberBPrefixText; }
			set { settings.Advanced.PercentageSplitSaberBPrefixText = value; }
		}




		[UIValue(nameof(ResultsViewModeList))]
		public List<object> ResultsViewModeList => ResultsViewModesToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewModesFormat))]
		public string ResultsViewModesFormat(ResultsViewModes mode) => ResultsViewModesToNames[mode];

		private static Dictionary<ResultsViewModes, string> ResultsViewModesToNames = new Dictionary<ResultsViewModes, string>()
		{
			{ResultsViewModes.On, "Show Always" },
			{ResultsViewModes.OffWhenFC, "Hide When FC" },
			{ResultsViewModes.Off, "Show Never" }
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
