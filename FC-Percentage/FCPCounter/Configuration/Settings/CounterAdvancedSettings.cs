namespace FCPercentage.FCPCounter.Configuration
{
	class CounterAdvancedSettings
	{
		// Advanced settings available from config file
		public virtual float CounterOffset { get; set; } = 0.0f;
		public virtual float LabelAboveCounterTextOffset { get; set; } = 0.32f;
		public virtual float LabelAboveCounterTextSize { get; set; } = 0.75f;
		public virtual float PercentageSize { get; set; } = 0.65f;
		public virtual float PercentageTotalAndSplitLineHeight { get; set; } = -0.55f;
		public virtual string LabelAboveCounterText { get; set; } = "FC Percent";
		public virtual string LabelPrefixText { get; set; } = "FC : ";
		public virtual string PercentageSplitSaberAPrefixText { get; set; } = "";
		public virtual string PercentageSplitSaberBPrefixText { get; set; } = "";
	}
}
