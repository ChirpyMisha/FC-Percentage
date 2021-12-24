using Zenject;

namespace FCPercentage.Installers
{
	class GameInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<NoteRatingTracker>().AsSingle();
			Container.BindInterfacesAndSelfTo<ScoreTracker>().AsSingle();
		}
	}
}
