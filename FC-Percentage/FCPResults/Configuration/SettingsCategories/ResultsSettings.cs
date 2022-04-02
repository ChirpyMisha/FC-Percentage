using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FCPercentage.FCPResults.Configuration
{
	//[Serializable]
	public class ResultsSettings
	{
		// Results Settings (Results View)
		public virtual ResultsViewModes PercentageTotalMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewModes PercentageSplitMode { get; set; } = ResultsViewModes.Off;
		public virtual ResultsViewModes ScoreTotalMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewLabelOptions EnableLabel { get; set; } = ResultsViewLabelOptions.BothOn;
		public virtual int DecimalPrecision { get; set; } = 2;
		public virtual bool EnableScorePercentageDifference { get; set; } = true;
		public virtual ResultsViewDiffModels ScorePercentageDiffModel { get; set; } = ResultsViewDiffModels.UpdatedHighscoreDiff;
		public virtual bool SplitPercentageUseSaberColorScheme { get; set; } = true;
		public virtual bool KeepTrailingZeros { get; set; } = false;

		public virtual ResultsAdvancedSettings Advanced { get; set; } = new ResultsAdvancedSettings();

		//public ResultsSettings Clone()
		//{
		//	IFormatter formatter = new BinaryFormatter();
		//	using (var stream = new MemoryStream())
		//	{
		//		formatter.Serialize(stream, this);
		//		stream.Seek(0, SeekOrigin.Begin);
		//		return (ResultsSettings)formatter.Deserialize(stream);
		//	}
		//}

		public static void RevertChanges(ResultsSettings settings, ResultsSettings oldSettings)
		{
			settings.PercentageTotalMode = oldSettings.PercentageTotalMode;
			settings.PercentageSplitMode = oldSettings.PercentageSplitMode;
			settings.ScoreTotalMode = oldSettings.ScoreTotalMode;
			settings.EnableLabel = oldSettings.EnableLabel;
			settings.DecimalPrecision = oldSettings.DecimalPrecision;
			settings.EnableScorePercentageDifference = oldSettings.EnableScorePercentageDifference;
			settings.ScorePercentageDiffModel = oldSettings.ScorePercentageDiffModel;
			settings.SplitPercentageUseSaberColorScheme = oldSettings.SplitPercentageUseSaberColorScheme;
			settings.KeepTrailingZeros = oldSettings.KeepTrailingZeros;
			//settings.IgnoreMultiplier = oldIgnoreMultiplier;

			settings.Advanced.DifferencePositiveColor = oldSettings.Advanced.DifferencePositiveColor;
			settings.Advanced.DifferenceNegativeColor = oldSettings.Advanced.DifferenceNegativeColor;
			settings.Advanced.ApplyColorsToScorePercentageModDifference = oldSettings.Advanced.ApplyColorsToScorePercentageModDifference;

			settings.Advanced.ScorePrefixText = oldSettings.Advanced.ScorePrefixText;
			settings.Advanced.PercentagePrefixText = oldSettings.Advanced.PercentagePrefixText;
			settings.Advanced.PercentageTotalPrefixText = oldSettings.Advanced.PercentageTotalPrefixText;
			settings.Advanced.PercentageSplitSaberAPrefixText = oldSettings.Advanced.PercentageSplitSaberAPrefixText;
			settings.Advanced.PercentageSplitSaberBPrefixText = oldSettings.Advanced.PercentageSplitSaberBPrefixText;
		}
	}

	public static class CloningService
	{
		public static T Clone<T>(this T source)
		{
			// Don't serialize a null object, simply return the default for that object
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}
			var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
			var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, serializeSettings), deserializeSettings);
		}
	}

	public enum ResultsViewModes { On, OffWhenFC, Off }
	public enum ResultsViewLabelOptions { BothOn, ScoreOn, PercentageOn, BothOff }
	public enum ResultsViewDiffModels { CurrentResultDiff, UpdatedHighscoreDiff, OldHighscoreDiff }
}
