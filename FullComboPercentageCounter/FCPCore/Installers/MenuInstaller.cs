using FullComboPercentageCounter.Configuration;
using Zenject;

namespace FullComboPercentageCounter.Installers
{
	class MenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<ResultsConfigController>().AsCached();
			Container.BindInterfacesAndSelfTo<ResultsConfigManager>().AsSingle();
			Container.BindInterfacesAndSelfTo<FCPResultsViewController>().AsSingle();
		}
	}
}
