using Core.Audio;
using Core.Ui;
using Game.UI;
using Zenject;

namespace DefaultNamespace.Game
{
    public class GameEnterPoint
    {
        [Inject]
        private void Init(UiComponent uiComponent, AudioComponent audioComponent)
        {
            var gameWindow = uiComponent.GetWindow<GameWindow>();
            gameWindow.Open();
            
            audioComponent.PlayMusic("background");
            audioComponent.SetMusicVolume(.5f);
        }
    }
}