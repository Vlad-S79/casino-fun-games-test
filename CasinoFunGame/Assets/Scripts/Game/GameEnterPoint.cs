using Core.Ui;
using Game.UI;
using Zenject;

namespace DefaultNamespace.Game
{
    public class GameEnterPoint
    {
        [Inject]
        private void Init(UiComponent uiComponent)
        {
            var gameWindow = uiComponent.GetWindow<GameWindow>();
            gameWindow.Open();
        }
    }
}