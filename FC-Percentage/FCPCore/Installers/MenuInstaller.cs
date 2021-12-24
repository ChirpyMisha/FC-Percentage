using FCPercentage.Configuration;
using Zenject;

namespace FCPercentage.Installers
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
