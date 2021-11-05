using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 1f;

    protected Transform CharacterTransform;

    protected virtual void Awake()
    {
        CharacterTransform = transform;
    }

    protected void Move(Vector3 destination)
    {
        CharacterTransform.DOKill();
        CharacterTransform.DOMove(destination, TrueSpeed(destination)).SetEase(Ease.Linear);
    }

    protected void Stop()
    {
        CharacterTransform.DOKill();
    }

    private float TrueSpeed(Vector3 destination)
    {
        return Vector3.Distance(CharacterTransform.position, destination) / speed;
    }
}