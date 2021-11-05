using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float turnRate = 1f;

    protected Transform CharacterTransform;
    private GameObject _compass;

    protected virtual void Awake()
    {
        CharacterTransform = transform;

        _compass = new GameObject("CharacterControllerCompass")
        {
            transform =
            {
                parent = CharacterTransform
            }
        };
    }

    public void Stop()
    {
        CharacterTransform.DOKill();
        _compass.transform.DOKill();
    }

    public void Move(Vector3 destination)
    {
        Stop();
        CharacterTransform.DOMove(destination, movementSpeed).SetSpeedBased().SetEase(Ease.Linear);
    }

    public void TurnAndMove(Vector3 destination)
    {
        Stop();

        _compass.transform
            .DORotate(new Vector3(float.Epsilon, float.Epsilon, AngleToDestination(destination)),
                turnRate * 360)
            .SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Move(destination));
    }

    private float AngleToDestination(Vector3 destination)
    {
        Vector2 angle = destination - CharacterTransform.position;

        return Mathf.Atan2(angle.y, angle.x) * 180 / Mathf.PI;
    }
}