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
	}
}
