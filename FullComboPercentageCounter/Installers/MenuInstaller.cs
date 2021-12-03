using Zenject;

namespace FullComboPercentageCounter.Installers
{
	class MenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FCPercentageResultsViewHandler>().AsSingle();
		}
	}
}
