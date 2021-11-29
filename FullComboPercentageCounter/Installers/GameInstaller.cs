using FullComboPercentageCounter.Configuration;
using Zenject;

namespace FullComboPercentageCounter.Installers
{
	class GameInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
			Container.BindInterfacesAndSelfTo<FCPercentageConfigModel>().AsSingle();
		}
	}
}
