using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationManager : MonoBehaviour
{
    EventProcessor unitEventHandler;
    private Animator _anim;
    private AnimationClip[] _animIdleClip;
    private AnimationClip OriginalClip ; // hardcode
    private AnimatorOverrideController aoc;
    private Camera _camera;
    private int degreeClipLength;
    [SerializeField]
    private int degreeVariation;

    private void Awake()
    {
        unitEventHandler = GetComponentInParent<UnitEventManager>().UnitEventHandler;
        degreeClipLength = 36;
        degreeVariation = 10;        
        _anim = GetComponent<Animator>();
        _camera = Camera.main;
        _animIdleClip = new AnimationClip[degreeClipLength];

        LoadResources();

        OriginalClip = _animIdleClip[0];// hardcode
        aoc = new AnimatorOverrideController(_anim.runtimeAnimatorController);
        _anim.runtimeAnimatorController = aoc;
    }
    private void Start()
    {
        unitEventHandler.StartListening("OnPseudoObjectRotationChanged", OnMoveOrderIssued);
    }

    private void OnMoveOrderIssued(object destination)
    {
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        float rotationAngle = (float) destination;
        Debug.Log((int) rotationAngle / 10);
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(OriginalClip, _animIdleClip[ (int) rotationAngle / 10 ]) );// every clip 10 degree 
        aoc.ApplyOverrides(anims);
    }

    private int GetFacingAngle(Vector3 mousePosition)
    {
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float anglefixed = angle < 0 ? (360.0f + angle) : angle;
        anglefixed = Mathf.Round((anglefixed/degreeVariation)) * degreeVariation;
        anglefixed = anglefixed < 360 ? anglefixed : 0;

        return (int)anglefixed;
    }

    private void LoadResources()
    {
        for(int i = 0 ; i < degreeClipLength ; i++) {
            _animIdleClip[i] = (AnimationClip)Resources.Load("Animations/Idle animation clip/"+ i * 10 +"_PlayerIdle");
        }
    }

    private void Update() {
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        Debug.Log(GetFacingAngle( _camera.ScreenToWorldPoint(Input.mousePosition) ));
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(OriginalClip, _animIdleClip[GetFacingAngle((Vector3) _camera.ScreenToWorldPoint(Input.mousePosition)) / 10]));
        aoc.ApplyOverrides(anims);
    }
}
