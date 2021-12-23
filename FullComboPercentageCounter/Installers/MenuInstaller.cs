using FullComboPercentageCounter.Configuration;
using Zenject;

namespace FullComboPercentageCounter.Installers
{
	class MenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FCScorePercentageConfigController>().AsCached();
			Container.BindInterfacesAndSelfTo<ConfigManager>().AsSingle();
			Container.BindInterfacesAndSelfTo<FCPercentageResultsViewHandler>().AsSingle();
		}
	}
}
