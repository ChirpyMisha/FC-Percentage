namespace FullComboPercentageCounter.Configuration
{
	class ResultsSettings
	{
		// Results Settings (Results View)
		public virtual ResultsViewModes PercentageTotalMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewModes PercentageSplitMode { get; set; } = ResultsViewModes.Off;
		public virtual ResultsViewModes ScoreTotalMode { get; set; } = ResultsViewModes.OffWhenFC;
		public virtual ResultsViewLabelOptions EnableLabel { get; set; } = ResultsViewLabelOptions.BothOn;
		public virtual int DecimalPrecision { get; set; } = 2;
		public virtual bool EnableScorePercentageDifference { get; set; } = true;
		public virtual bool SplitPercentageUseSaberColorScheme { get; set; } = true;
		public virtual bool KeepTrailingZeros { get; set; } = false;

		public virtual ResultsAdvancedSettings Advanced { get; set; } = new ResultsAdvancedSettings();
	}

	public enum ResultsViewModes { On, OffWhenFC, Off }
	public enum ResultsViewLabelOptions { BothOn, ScoreOn, PercentageOn, BothOff }
}
