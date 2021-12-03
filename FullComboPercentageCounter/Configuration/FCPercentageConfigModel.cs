using BeatSaberMarkupLanguage.Attributes;

namespace FullComboPercentageCounter.Configuration
{
	public class FCPercentageConfigModel
	{
		[UIValue("DecimalPrecision")]
		public virtual int DecimalPrecision
		{
			get { return PluginConfig.Instance.DecimalPrecision; }
			set { PluginConfig.Instance.DecimalPrecision = value; }
		}

		[UIValue("PercentageSize")]
		public virtual float PercentageSize
		{
			get { return PluginConfig.Instance.PercentageSize; }
			set { PluginConfig.Instance.PercentageSize = value; }
		}

		[UIValue("EnableLabel")]
		public virtual bool EnableLabel
		{
			get { return PluginConfig.Instance.EnableLabel; }
			set { PluginConfig.Instance.EnableLabel = value; }
		}
		
		[UIValue("LabelAboveCount")]
		public virtual bool LabelAboveCount
		{
			get { return PluginConfig.Instance.LabelAboveCount; }
			set { PluginConfig.Instance.LabelAboveCount = value; }
		}

		[UIValue("IgnoreMultiplier")]
		public virtual bool IgnoreMultiplier
		{
			get { return PluginConfig.Instance.IgnoreMultiplier; }
			set { PluginConfig.Instance.IgnoreMultiplier = value; }
		}
	}
}
