using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using FCPercentage.FCPCore.Configuration;

namespace FCPercentage.FCPResults.Configuration
{
	class ResultsConfigController : INotifyPropertyChanged
	{
		private static string enabledTextColor = "#" + ColorUtility.ToHtmlStringRGB(Color.white);
		private static string disabledTextColor = "#" + ColorUtility.ToHtmlStringRGB(Color.grey);

		private ResultsSettings settings;
		private ResultsSettings soloSettings => PluginConfig.Instance.ResultsSettings;
		private ResultsSettings missionSettings => PluginConfig.Instance.MissionResultsSettings;
		private ResultsSettings Settings
		{
			get { return settings; }
			set
			{
				settings = value;
				oldSettings = settings.Clone();
				oldIgnoreMultiplier = PluginConfig.Instance.IgnoreMultiplier;
				RaisePropertyChangedAllProperties();
			}
		}

		private ResultsSettings oldSettings;
		private bool oldIgnoreMultiplier;

#pragma warning disable CS8618
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618

		public ResultsConfigController()
		{
			Settings = PluginConfig.Instance.ResultsSettings;
		}

		private void RevertChanges()
		{
			Plugin.Log.Info("ResultsConfigController, RevertChanges: Reverting changes.");

			ResultsSettings.RevertChanges(settings, oldSettings);
			PluginConfig.Instance.IgnoreMultiplier = oldIgnoreMultiplier;

			RaisePropertyChangedAllProperties();
		}

		private void RaisePropertyChangedAllProperties()
		{
			RaisePropertyChanged(nameof(PercentageTotalMode));
			RaisePropertyChanged(nameof(PercentageSplitMode));
			RaisePropertyChanged(nameof(ScoreTotalMode));
			RaisePropertyChanged(nameof(EnableLabel));
			RaisePropertyChanged(nameof(DecimalPrecision));
			RaisePropertyChanged(nameof(EnableScorePercentageDifference));
			RaisePropertyChanged(nameof(ScorePercentageDiffModel));
			RaisePropertyChanged(nameof(SplitPercentageUseSaberColorScheme));
			RaisePropertyChanged(nameof(KeepTrailingZeros));
			RaisePropertyChanged(nameof(IgnoreMultiplier));

			RaisePropertyChanged(nameof(ScorePercentageDiffPositiveColor));
			RaisePropertyChanged(nameof(ScorePercentageDiffNegativeColor));

			RaisePropertyChanged(nameof(ScorePrefixText));
			RaisePropertyChanged(nameof(PercentagePrefixText));
			RaisePropertyChanged(nameof(PercentageTotalPrefixText));
			RaisePropertyChanged(nameof(PercentageSplitSaberAPrefixText));
			RaisePropertyChanged(nameof(PercentageSplitSaberBPrefixText));

			RaisePropertyChanged(nameof(IsAnyOn));
			RaisePropertyChanged(nameof(IsAnyPercentOn));
			RaisePropertyChanged(nameof(IsPercentageTotalOn));
			RaisePropertyChanged(nameof(IsPercentageSplitOn));
			RaisePropertyChanged(nameof(IsScoreTotalOn));
			RaisePropertyChanged(nameof(IsScorePrefixOn));
			RaisePropertyChanged(nameof(IsPercentagePrefixOn));
			RaisePropertyChanged(nameof(IsScorePercentageDiffOn));

			RaisePropertyChanged(nameof(IsAnyOnColor));
			RaisePropertyChanged(nameof(IsAnyPercentOnColor));
			RaisePropertyChanged(nameof(IsPercentageTotalOnColor));
			RaisePropertyChanged(nameof(IsPercentageSplitOnColor));
			RaisePropertyChanged(nameof(IsScoreTotalOnColor));
			RaisePropertyChanged(nameof(IsScorePrefixOnColor));
			RaisePropertyChanged(nameof(IsPercentagePrefixOnColor));
			RaisePropertyChanged(nameof(IsScorePercentageDiffOnColor));
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
		private bool IsPercentageTotalOn => Settings.PercentageTotalMode != ResultsViewModes.Off;
		[UIValue("is-percentage-split-on")]
		private bool IsPercentageSplitOn => Settings.PercentageSplitMode != ResultsViewModes.Off;
		[UIValue("is-score-total-on")]
		private bool IsScoreTotalOn => Settings.ScoreTotalMode != ResultsViewModes.Off;
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
			get { return Settings.PercentageTotalMode; }
			set 
			{
				Settings.PercentageTotalMode = value;
				RaisePropertyChanged();
				UpdateInteractabilityPercentageTotal();
			}
		}

		[UIValue("PercentageSplitMode")]
		public virtual ResultsViewModes PercentageSplitMode
		{
			get { return Settings.PercentageSplitMode; }
			set 
			{
				Settings.PercentageSplitMode = value;
				RaisePropertyChanged();
				UpdateInteractabilityPercentageSplit();
			}
		}

		[UIValue("ScoreTotalMode")]
		public virtual ResultsViewModes ScoreTotalMode
		{
			get { return Settings.ScoreTotalMode; }
			set 
			{
				Settings.ScoreTotalMode = value;
				RaisePropertyChanged();
				UpdateInteractabilityScoreTotal();
			}
		}

		[UIValue("EnableLabel")]
		public virtual ResultsViewLabelOptions EnableLabel
		{
			get { return Settings.EnableLabel; }
			set
			{ 
				Settings.EnableLabel = value;
				RaisePropertyChanged();
				UpdateInteractabilityLabels();
			}
		}

		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return Settings.DecimalPrecision; }
			set { Settings.DecimalPrecision = value; RaisePropertyChanged(); }
		}

		[UIValue("EnableScorePercentageDifference")]
		public virtual bool EnableScorePercentageDifference
		{
			get { return Settings.EnableScorePercentageDifference; }
			set 
			{ 
				Settings.EnableScorePercentageDifference = value;
				RaisePropertyChanged();
				UpdateInteractabilityScorePercentageDiff();
			}
		}

		[UIValue("ScorePercentageDiffModel")]
		public virtual ResultsViewDiffModels ScorePercentageDiffModel
		{
			get { return Settings.ScorePercentageDiffModel; }
			set { Settings.ScorePercentageDiffModel = value; RaisePropertyChanged(); }
		}

		[UIValue("SplitPercentageUseSaberColorScheme")]
		public virtual bool SplitPercentageUseSaberColorScheme
		{
			get { return Settings.SplitPercentageUseSaberColorScheme; }
			set { Settings.SplitPercentageUseSaberColorScheme = value; RaisePropertyChanged(); }
		}

		[UIValue("KeepTrailingZeros")]
		public virtual bool KeepTrailingZeros
		{
			get { return Settings.KeepTrailingZeros; }
			set { Settings.KeepTrailingZeros = value; RaisePropertyChanged(); }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; RaisePropertyChanged(); }
		}

		// Score/Percentage Diff Colors
		[UIValue("ScorePercentageDiffPositiveColor")]
		public virtual Color ScorePercentageDiffPositiveColor
		{
			get { return HexToColor(Settings.Advanced.DifferencePositiveColor); }
			set { Settings.Advanced.DifferencePositiveColor = ColorToHex(value); RaisePropertyChanged(); }
		}

		[UIValue("ScorePercentageDiffNegativeColor")]
		public virtual Color ScorePercentageDiffNegativeColor
		{
			get { return HexToColor(Settings.Advanced.DifferenceNegativeColor); }
			set { Settings.Advanced.DifferenceNegativeColor = ColorToHex(value); RaisePropertyChanged(); }
		}

		[UIValue("ApplyColorsToScorePercentageModDifference")]
		public virtual bool ApplyColorsToScorePercentageModDifference
		{
			get { return Settings.Advanced.ApplyColorsToScorePercentageModDifference; }
			set { Settings.Advanced.ApplyColorsToScorePercentageModDifference = value; RaisePropertyChanged(); }
		}

		// Prefix Strings
		[UIValue("ScorePrefixText")]
		public virtual string ScorePrefixText
		{
			get { return Settings.Advanced.ScorePrefixText; }
			set { Settings.Advanced.ScorePrefixText = value; RaisePropertyChanged(); }
		}

		[UIValue("PercentagePrefixText")]
		public virtual string PercentagePrefixText
		{
			get { return Settings.Advanced.PercentagePrefixText; }
			set { Settings.Advanced.PercentagePrefixText = value; RaisePropertyChanged(); }
		}

		[UIValue("PercentageTotalPrefixText")]
		public virtual string PercentageTotalPrefixText
		{
			get { return Settings.Advanced.PercentageTotalPrefixText; }
			set { Settings.Advanced.PercentageTotalPrefixText = value; RaisePropertyChanged(); }
		}

		[UIValue("PercentageSplitSaberAPrefixText")]
		public virtual string PercentageSplitSaberAPrefixText
		{
			get { return Settings.Advanced.PercentageSplitSaberAPrefixText; }
			set { Settings.Advanced.PercentageSplitSaberAPrefixText = value; RaisePropertyChanged(); }
		}

		[UIValue("PercentageSplitSaberBPrefixText")]
		public virtual string PercentageSplitSaberBPrefixText
		{
			get { return Settings.Advanced.PercentageSplitSaberBPrefixText; }
			set { Settings.Advanced.PercentageSplitSaberBPrefixText = value; RaisePropertyChanged(); }
		}


		[UIAction("#solo-results-settings-entered")]
		public void OnSoloResultsSettingsEntered() => Settings = soloSettings;

		[UIAction("#mission-results-settings-entered")]
		public void OnMissionResultsSettingsEntered() => Settings = missionSettings;

		[UIAction("#reset-score-percentage-colors")]
		public void OnResetScorePercentageColors() => RevertToDefault_Color();

		[UIAction("#reset-prefix-strings")]
		public void OnResetPrefixStrings() => RevertToDefault_Strings();

		[UIAction("#revert-settings")]
		public void OnRevertSettings() => RevertChanges();

		[UIAction("#cancel")]
		public void OnCancel() => RevertChanges();

		//[UIAction("#apply")]
		//public void OnApply()
		//{
		//
		//}


		#region ResultsViewModes Formatting
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
		#endregion

		#region ResultsViewLabelOptions Formatting
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
		#endregion

		#region ResultsViewDiffModels Formatting
		[UIValue(nameof(ResultsViewDiffModelList))]
		public List<object> ResultsViewDiffModelList => ResultsViewDiffModelsToNames.Keys.Cast<object>().ToList();

		[UIAction(nameof(ResultsViewDiffModelsFormat))]
		public string ResultsViewDiffModelsFormat(ResultsViewDiffModels model) => ResultsViewDiffModelsToNames[model];

		private static Dictionary<ResultsViewDiffModels, string> ResultsViewDiffModelsToNames = new Dictionary<ResultsViewDiffModels, string>()
		{
			{ ResultsViewDiffModels.CurrentResultDiff, "Mode 1" },
			{ ResultsViewDiffModels.UpdatedHighscoreDiff, "Mode 2" },
			{ ResultsViewDiffModels.OldHighscoreDiff, "Mode 3" }
		};
		#endregion

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
