using UnityEngine;

public class DisableCanvasWhilePaused : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnGamePaused", OnGamePaused);
        EventManager.StartListening("OnGameResumed", OnGameResumed);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnGamePaused", OnGamePaused);
        EventManager.StopListening("OnGameResumed", OnGameResumed);
    }

    private void OnGamePaused(object arg0)
    {
        _canvas.enabled = false;
        _canvasGroup.alpha = 0;
    }

    private void OnGameResumed(object arg0)
    {
        _canvas.enabled = true;
        _canvasGroup.alpha = 1;
    }
}
