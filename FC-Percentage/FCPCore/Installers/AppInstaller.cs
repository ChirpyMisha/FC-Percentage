using Zenject;

namespace FCPercentage.Installers
{
	class AppInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
		}
	}
}
