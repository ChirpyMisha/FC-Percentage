using Zenject;

namespace FullComboPercentageCounter.Installers
{
	class AppInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
		}
	}
}
