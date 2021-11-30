using UnityEngine;

public class DisableCanvasWhilePaused : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (GetComponent<Canvas>() != null) _canvas = GetComponent<Canvas>();
        if (GetComponent<CanvasGroup>() != null) _canvasGroup = GetComponent<CanvasGroup>();
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
        if (_canvas != null) _canvas.enabled = false;
        if (_canvasGroup != null) _canvasGroup.alpha = 0;
    }

    private void OnGameResumed(object arg0)
    {
        if (_canvas != null) _canvas.enabled = true;
        if (_canvasGroup != null) _canvasGroup.alpha = 1;
    }
}
