using UnityEngine;
using Zenject;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup _gameOverScreen;
    [Inject] private EventManager _eventManager;

    private void ScreenDisplay()
    {
        _gameOverScreen.alpha = _gameOverScreen.alpha == 1 ? 0 : 1;
        _gameOverScreen.blocksRaycasts = !_gameOverScreen.blocksRaycasts;
        _gameOverScreen.interactable = !_gameOverScreen.interactable;
    }

    public void OnRestartButton()
    {
        _eventManager.onRestartLevel?.Invoke();
        ScreenDisplay();
    }   

    private void OnEnable() => _eventManager.onGameOver += ScreenDisplay;
    private void OnDisable() => _eventManager.onGameOver -= ScreenDisplay;
}
