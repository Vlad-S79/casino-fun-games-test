using Core.Audio;
using Core.Haptic;
using Core.Ui;
using Zenject;

public class CoreBootstrap : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<HapticComponent>().AsSingle().NonLazy();
        Container.Bind<AudioComponent>().AsSingle().NonLazy();
        Container.Bind<UiComponent>().AsSingle().NonLazy();
    }
}
