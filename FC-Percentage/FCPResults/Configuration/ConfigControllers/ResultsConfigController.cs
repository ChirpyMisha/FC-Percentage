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

#pragma warning disable CS8618
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618

		private ResultsViewModes percentageTotalMode;
		private ResultsViewModes percentageSplitMode;
		private ResultsViewModes scoreTotalMode;
		private ResultsViewLabelOptions enableLabel;
		private bool enableScorePercentageDifference;

		private Color scorePercentageDiffPositiveColor;
		private Color scorePercentageDiffNegativeColor;

		private string scorePrefixText;
		private string percentagePrefixText;
		private string percentageTotalPrefixText;
		private string percentageSplitSaberAPrefixText;
		private string percentageSplitSaberBPrefixText;

		public ResultsConfigController()
		{
			percentageTotalMode = settings.PercentageTotalMode;
			percentageSplitMode = settings.PercentageSplitMode;
			scoreTotalMode = settings.ScoreTotalMode;
			enableLabel = settings.EnableLabel;
			enableScorePercentageDifference = settings.EnableScorePercentageDifference;

			scorePercentageDiffPositiveColor = HexToColor(settings.Advanced.DifferencePositiveColor);
			scorePercentageDiffNegativeColor = HexToColor(settings.Advanced.DifferenceNegativeColor);

			scorePrefixText = settings.Advanced.ScorePrefixText;
			percentagePrefixText = settings.Advanced.PercentagePrefixText;
			percentageTotalPrefixText = settings.Advanced.PercentageTotalPrefixText;
			percentageSplitSaberAPrefixText = settings.Advanced.PercentageSplitSaberAPrefixText;
			percentageSplitSaberBPrefixText = settings.Advanced.PercentageSplitSaberBPrefixText;
		}

		private void RevertChanges()
		{
            PercentageTotalMode = percentageTotalMode;
            PercentageSplitMode = percentageSplitMode;
            ScoreTotalMode = scoreTotalMode;
			EnableLabel = enableLabel;
			EnableScorePercentageDifference = enableScorePercentageDifference;

			ScorePercentageDiffPositiveColor = scorePercentageDiffPositiveColor;
			ScorePercentageDiffNegativeColor = scorePercentageDiffNegativeColor;

			ScorePrefixText = scorePrefixText;
			PercentagePrefixText = percentagePrefixText;
			PercentageTotalPrefixText = percentageTotalPrefixText;
			PercentageSplitSaberAPrefixText = percentageSplitSaberAPrefixText;
			PercentageSplitSaberBPrefixText = percentageSplitSaberBPrefixText;
		}

		private void RevertToDefault_Color()
		{
			ScorePercentageDiffPositiveColor = HexToColor(ResultsAdvancedSettings.DefaultDifferencePositiveColor);
			RaisePropertyChanged(nameof(ScorePercentageDiffPositiveColor));
			ScorePercentageDiffNegativeColor = HexToColor(ResultsAdvancedSettings.DefaultDifferenceNegativeColor);
			RaisePropertyChanged(nameof(ScorePercentageDiffNegativeColor));
		}

		private void RevertToDefault_Strings()
		{
			ScorePrefixText = ResultsAdvancedSettings.DefaultScorePrefixText;
			RaisePropertyChanged(nameof(ScorePrefixText));
			PercentagePrefixText = ResultsAdvancedSettings.DefaultPercentagePrefixText;
			RaisePropertyChanged(nameof(PercentagePrefixText));
			PercentageTotalPrefixText = ResultsAdvancedSettings.DefaultPercentageTotalPrefixText;
			RaisePropertyChanged(nameof(PercentageTotalPrefixText));
			PercentageSplitSaberAPrefixText = ResultsAdvancedSettings.DefaultPercentageSplitSaberAPrefixText;
			RaisePropertyChanged(nameof(PercentageSplitSaberAPrefixText));
			PercentageSplitSaberBPrefixText = ResultsAdvancedSettings.DefaultPercentageSplitSaberBPrefixText;
			RaisePropertyChanged(nameof(PercentageSplitSaberBPrefixText));
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
		[UIValue("is-score-prefix-on")]
		private bool IsScorePrefixOn => IsScoreTotalOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.ScoreOn);
		[UIValue("is-percentage-prefix-on")]
		private bool IsPercentagePrefixOn => IsAnyPercentOn && (EnableLabel == ResultsViewLabelOptions.BothOn || EnableLabel == ResultsViewLabelOptions.PercentageOn);
		[UIValue("is-score-percentage-diff-on")]
		private bool IsScorePercentageDiffOn => IsAnyPercentOn && EnableScorePercentageDifference;


		[UIValue("is-any-on-color")]
		private string IsAnyOnColor => GetInteractabilityColor(IsAnyOn);
		[UIValue("is-any-percent-on-color")]
		private string IsAnyPercentOnColor => GetInteractabilityColor(IsAnyPercentOn);
		[UIValue("is-percentage-total-on-color")]
		private string IsPercentageTotalOnColor => GetInteractabilityColor(IsPercentageTotalOn);
		[UIValue("is-percentage-split-on-color")]
		private string IsPercentageSplitOnColor => GetInteractabilityColor(IsPercentageSplitOn);
		[UIValue("is-score-total-on-color")]
		private string IsScoreTotalOnColor => GetInteractabilityColor(IsScoreTotalOn);
		[UIValue("is-score-prefix-on-color")]
		private string IsScorePrefixOnColor => GetInteractabilityColor(IsScorePrefixOn);
		[UIValue("is-percentage-prefix-on-color")]
		private string IsPercentagePrefixOnColor => GetInteractabilityColor(IsPercentagePrefixOn);
		[UIValue("is-score-percentage-diff-on-color")]
		private string IsScorePercentageDiffOnColor => GetInteractabilityColor(IsScorePercentageDiffOn);

		private string GetInteractabilityColor(bool isEnabled)
		{
			return isEnabled ? enabledTextColor : disabledTextColor;
		}

		private void UpdateInteractabilityScorePercentage()
		{
			RaisePropertyChanged(nameof(IsAnyOn));
			RaisePropertyChanged(nameof(IsScorePercentageDiffOn));

			RaisePropertyChanged(nameof(IsAnyOnColor));
			RaisePropertyChanged(nameof(IsScorePercentageDiffOnColor));
		}

		private void UpdateInteractabilityPercentage()
		{
			UpdateInteractabilityScorePercentage();

			RaisePropertyChanged(nameof(IsAnyPercentOn));
			RaisePropertyChanged(nameof(IsPercentagePrefixOn));

			RaisePropertyChanged(nameof(IsAnyPercentOnColor));
			RaisePropertyChanged(nameof(IsPercentagePrefixOnColor));
		}

		private void UpdateInteractabilityPercentageTotal()
		{
			UpdateInteractabilityPercentage();
			
			RaisePropertyChanged(nameof(IsPercentageTotalOn));

			RaisePropertyChanged(nameof(IsPercentageTotalOnColor));
		}

		private void UpdateInteractabilityPercentageSplit()
		{
			UpdateInteractabilityPercentage();

			RaisePropertyChanged(nameof(IsPercentageSplitOn));

			RaisePropertyChanged(nameof(IsPercentageSplitOnColor));
		}

		private void UpdateInteractabilityScoreTotal()
		{
			UpdateInteractabilityScorePercentage();

			RaisePropertyChanged(nameof(IsScoreTotalOn));
			RaisePropertyChanged(nameof(IsScorePrefixOn));

			RaisePropertyChanged(nameof(IsScoreTotalOnColor));
			RaisePropertyChanged(nameof(IsScorePrefixOnColor));
		}

		private void UpdateInteractabilityLabels()
		{
			RaisePropertyChanged(nameof(IsPercentagePrefixOn));
			RaisePropertyChanged(nameof(IsScorePrefixOn));

			RaisePropertyChanged(nameof(IsPercentagePrefixOnColor));
			RaisePropertyChanged(nameof(IsScorePrefixOnColor));
		}

		private void UpdateInteractabilityScorePercentageDiff()
		{
			RaisePropertyChanged(nameof(IsScorePercentageDiffOn));

			RaisePropertyChanged(nameof(IsScorePercentageDiffOnColor));
		}


		[UIValue("PercentageTotalMode")]
		public virtual ResultsViewModes PercentageTotalMode
		{
			get { return settings.PercentageTotalMode; }
			set 
			{
				settings.PercentageTotalMode = value;
				RaisePropertyChanged();
				UpdateInteractabilityPercentageTotal();
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
				UpdateInteractabilityPercentageSplit();
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
				UpdateInteractabilityScoreTotal();
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
				UpdateInteractabilityLabels();
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
			set 
			{ 
				settings.EnableScorePercentageDifference = value;
				RaisePropertyChanged();
				UpdateInteractabilityScorePercentageDiff();
			}
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

		//[UIValue("BadCutThreshold")]
		//public virtual int BadCutThreshold
		//{
		//	get { return PluginConfig.Instance.BadCutThreshold; }
		//	set { PluginConfig.Instance.BadCutThreshold = value; }
		//}

		// Score/Percentage Diff Colors
		[UIValue("ScorePercentageDiffPositiveColor")]
		public virtual Color ScorePercentageDiffPositiveColor
		{
			get { return HexToColor(settings.Advanced.DifferencePositiveColor); }
			set 
			{ 
				settings.Advanced.DifferencePositiveColor = ColorToHex(value);
				RaisePropertyChanged();
			}
		}

		[UIValue("ScorePercentageDiffNegativeColor")]
		public virtual Color ScorePercentageDiffNegativeColor
		{
			get { return HexToColor(settings.Advanced.DifferenceNegativeColor); }
			set 
			{
				settings.Advanced.DifferenceNegativeColor = ColorToHex(value);
				RaisePropertyChanged();
			}
		}

		[UIValue("ApplyColorsToScorePercentageModDifference")]
		public virtual bool ApplyColorsToScorePercentageModDifference
		{
			get { return settings.Advanced.ApplyColorsToScorePercentageModDifference; }
			set { settings.Advanced.ApplyColorsToScorePercentageModDifference = value; }
		}

		// Prefix Strings
		[UIValue("ScorePrefixText")]
		public virtual string ScorePrefixText
		{
			get { return settings.Advanced.ScorePrefixText; }
			set 
			{ 
				settings.Advanced.ScorePrefixText = value;
				RaisePropertyChanged();
			}
		}

		[UIValue("PercentagePrefixText")]
		public virtual string PercentagePrefixText
		{
			get { return settings.Advanced.PercentagePrefixText; }
			set 
			{ 
				settings.Advanced.PercentagePrefixText = value;
				RaisePropertyChanged();
			}
		}

		[UIValue("PercentageTotalPrefixText")]
		public virtual string PercentageTotalPrefixText
		{
			get { return settings.Advanced.PercentageTotalPrefixText; }
			set 
			{ 
				settings.Advanced.PercentageTotalPrefixText = value;
				RaisePropertyChanged();
			}
		}

		[UIValue("PercentageSplitSaberAPrefixText")]
		public virtual string PercentageSplitSaberAPrefixText
		{
			get { return settings.Advanced.PercentageSplitSaberAPrefixText; }
			set	
			{ 
				settings.Advanced.PercentageSplitSaberAPrefixText = value;
				RaisePropertyChanged();
			}
		}

		[UIValue("PercentageSplitSaberBPrefixText")]
		public virtual string PercentageSplitSaberBPrefixText
		{
			get { return settings.Advanced.PercentageSplitSaberBPrefixText; }
			set 
			{ 
				settings.Advanced.PercentageSplitSaberBPrefixText = value;
				RaisePropertyChanged();
			}
		}



		[UIAction("#reset-score-percentage-colors")]
		public void OnResetScorePercentageColors() => RevertToDefault_Color();

		[UIAction("#reset-prefix-strings")]
		public void OnResetPrefixStrings() => RevertToDefault_Strings();

		[UIAction("#cancel")]
		public void OnCancel()
		{
			Plugin.Log.Notice("Reverting changes. Setting local percentage & score modes from settings.");
			RevertChanges();
		}

		//[UIAction("#apply")]
		//public void OnApply()
		//{
		//
		//}



		[UIValue(nameof(ResultsViewModeList))]
		public List<object> ResultsViewModeList => ResultsViewModesToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewModesFormat))]
		public string ResultsViewModesFormat(ResultsViewModes mode) => ResultsViewModesToNames[mode];

		private static Dictionary<ResultsViewModes, string> ResultsViewModesToNames = new Dictionary<ResultsViewModes, string>()
		{
			{ ResultsViewModes.On, "Show Always" },
			{ ResultsViewModes.OffWhenFC, "Hide When FC" },
			{ ResultsViewModes.Off, "Show Never" }
		};

		[UIValue(nameof(ResultsViewLabelOptionList))]
		public List<object> ResultsViewLabelOptionList => ResultsViewLabelOptionsToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewLabelOptionsFormat))]
		public string ResultsViewLabelOptionsFormat(ResultsViewLabelOptions option) => ResultsViewLabelOptionsToNames[option];

		private static Dictionary<ResultsViewLabelOptions, string> ResultsViewLabelOptionsToNames = new Dictionary<ResultsViewLabelOptions, string>()
		{
			{ ResultsViewLabelOptions.BothOn, "Both Labels" },
			{ ResultsViewLabelOptions.ScoreOn, "Only Score Label" },
			{ ResultsViewLabelOptions.PercentageOn, "Only Percentage Label" },
			{ ResultsViewLabelOptions.BothOff, "No Labels" }
		};

		private Color HexToColor(string hex)
		{
			Color color = new Color();
			ColorUtility.TryParseHtmlString(hex, out color);
			return color;
		}

		private string ColorToHex(Color value)
		{
			return $"#{ColorUtility.ToHtmlStringRGB(value)}";
		}
	}
}
