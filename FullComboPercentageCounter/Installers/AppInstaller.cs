using Zenject;

namespace FullComboPercentageCounter.Installers
{
	public class AppInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
		}
	}
}
