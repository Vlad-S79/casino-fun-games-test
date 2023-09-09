using DefaultNamespace.Game;
using Game.Coin;
using Game.SlotsLogic;
using Zenject;

namespace DefaultNamespace
{
    public class GameBootstrap : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CoinsComponent>().AsSingle().NonLazy();
            Container.Bind<SlotsLogicComponent>().AsSingle().NonLazy();
            
            Container.Bind<GameEnterPoint>().AsSingle().NonLazy();
        }
    }
}