using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;

using UnityEngine.UIElements;
using Zenject;
using UnityEngine;

namespace FCPercentage.Configuration
{
	class ResultsConfigController : INotifyPropertyChanged
	{
		private ResultsSettings settings => PluginConfig.Instance.ResultsSettings;

		public event PropertyChangedEventHandler PropertyChanged;

		private ResultsViewModes percentageTotalMode;
		private ResultsViewModes percentageSplitMode;
		private ResultsViewModes scoreTotalMode;

		public ResultsConfigController()
		{
			Plugin.Log.Notice("Init results config. Setting local percentage & score modes from settings.");
			LoadSettingsIntoLocals();
		}

		private void LoadSettingsIntoLocals()
		{
			percentageTotalMode = settings.PercentageTotalMode;
			percentageSplitMode = settings.PercentageSplitMode;
			scoreTotalMode = settings.ScoreTotalMode;

			UpdateInteractability();
		}

		private void RevertChanges()
		{
            PercentageTotalMode = percentageTotalMode;
            PercentageSplitMode = percentageSplitMode;
            ScoreTotalMode = scoreTotalMode;
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

		private string enabledColor = "#" + ColorUtility.ToHtmlStringRGB(Color.white);
		private string disabledColor = "#" + ColorUtility.ToHtmlStringRGB(Color.grey);
		[UIValue("is-any-on-color")]
		private string IsAnyOnColor => IsAnyOn ? enabledColor : disabledColor;
		[UIValue("is-any-percent-on-color")]
		private string IsAnyPercentOnColor => IsAnyPercentOn ? enabledColor : disabledColor;
		[UIValue("is-percentage-total-on-color")]
		private string IsPercentageTotalOnColor => IsPercentageTotalOn ? enabledColor : disabledColor;
		[UIValue("is-percentage-split-on-color")]
		private string IsPercentageSplitOnColor => IsPercentageSplitOn ? enabledColor : disabledColor;
		[UIValue("is-score-total-on-color")]
		private string IsScoreTotalOnColor => IsScoreTotalOn ? enabledColor : disabledColor;

		private void UpdateInteractability()
		{
			RaisePropertyChanged(nameof(IsAnyOn));
			RaisePropertyChanged(nameof(IsAnyPercentOn));
			RaisePropertyChanged(nameof(IsPercentageTotalOn));
			RaisePropertyChanged(nameof(IsPercentageSplitOn));
			RaisePropertyChanged(nameof(IsScoreTotalOn));

			UpdateTextColors();
		}

		private void UpdateTextColors()
		{
			RaisePropertyChanged(nameof(IsAnyOnColor));
			RaisePropertyChanged(nameof(IsAnyPercentOnColor));
			RaisePropertyChanged(nameof(IsPercentageTotalOnColor));
			RaisePropertyChanged(nameof(IsPercentageSplitOnColor));
			RaisePropertyChanged(nameof(IsScoreTotalOnColor));
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

		//[UIValue("EnableLabelActive")]
		//private bool EnableLabelActive => IsAnyOn;
		[UIValue("EnableLabel")]
		public virtual ResultsViewLabelOptions EnableLabel
		{
			get { return settings.EnableLabel; }
			set { settings.EnableLabel = value; }
		}

		//[UIValue("DecimalPrecisionActive")]
		//private bool DecimalPrecisionActive => IsAnyPercentOn;
		//[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return settings.DecimalPrecision; }
			set { settings.DecimalPrecision = value; }
		}

		//[UIValue("EnableScorePercentageDifferenceActive")]
		//private bool EnableScorePercentageDifferenceActive => IsPercentageSplitOn;
		//[UIValue("EnableScorePercentageDifference")]
		public virtual bool EnableScorePercentageDifference
		{
			get { return settings.EnableScorePercentageDifference; }
			set { settings.EnableScorePercentageDifference = value; }
		}

		//[UIValue("SaberColorSchemeActive")]
		//private bool SaberColorSchemeActive => IsPercentageSplitOn;
		//[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get { return settings.SplitPercentageUseSaberColorScheme; }
			set { settings.SplitPercentageUseSaberColorScheme = value; }
		}

		//[UIValue("KeepTrailingZerosActive")]
		//private bool KeepTrailingZerosActive => IsAnyPercentOn;
		//[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get { return settings.KeepTrailingZeros; }
			set { settings.KeepTrailingZeros = value; }
		}

		//[UIValue("IgnoreMultiplierActive")]
		//private bool IgnoreMultiplierActive => IsAnyOn;
		//[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
		}

		// Advanced Settings
		[UIValue("ScorePrefixTextActive")]
		private bool ScorePrefixTextActive => IsScoreTotalOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.ScoreOn);
		[UIValue("ScorePrefixText")]
		public virtual string ScorePrefixText
		{
			get { return settings.Advanced.ScorePrefixText; }
			set { settings.Advanced.ScorePrefixText = value; }
		}

		[UIValue("PercentagePrefixTextActive")]
		private bool PercentagePrefixTextActive => IsAnyPercentOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.PercentageOn);
		[UIValue("PercentagePrefixText")]
		public virtual string PercentagePrefixText
		{
			get { return settings.Advanced.PercentagePrefixText; }
			set { settings.Advanced.PercentagePrefixText = value; }
		}

		//[UIValue("PercentageTotalPrefixTextActive")]
		//private bool PercentageTotalPrefixTextActive => IsPercentageTotalOn;
		//[UIValue("PercentageTotalPrefixText")]
		public virtual string PercentageTotalPrefixText
		{
			get { return settings.Advanced.PercentageTotalPrefixText; }
			set { settings.Advanced.PercentageTotalPrefixText = value; }
		}

		//[UIValue("PercentageSplitSaberAPrefixTextActive")]
		//private bool PercentageSplitSaberAPrefixTextActive => IsPercentageSplitOn;
		//[UIValue("PercentageSplitSaberAPrefixText")]
		public virtual string PercentageSplitSaberAPrefixText
		{
			get { return settings.Advanced.PercentageSplitSaberAPrefixText; }
			set	{ settings.Advanced.PercentageSplitSaberAPrefixText = value; }
		}

		//[UIValue("PercentageSplitSaberBPrefixTextActive")]
		//private bool PercentageSplitSaberBPrefixTextActive => IsPercentageSplitOn;
		//[UIValue("PercentageSplitSaberBPrefixText")]
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
