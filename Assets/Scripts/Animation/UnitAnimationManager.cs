using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
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

    Dictionary<string, AnimationClip[]> animationLibrary = new Dictionary<string, AnimationClip[]>();

    [SerializeField]
    private int degreeVariation;

    [SerializeField] 
    [Tooltip("All unique animation state names. Ensure that the corresponding folder has the same name.")]
    List<string> animationStateNames = new List<string>();

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
        // unitEventHandler.StartListening("OnPseudoObjectRotationChanged", OnMoveOrderIssued);

        foreach(AnimationClip c in _anim.runtimeAnimatorController.animationClips) {
            Debug.Log(c);
        }
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
        /*---------------------------------------------------------------------------------------------
        Loads all animations based on properly named folder into animationLibrary dictionary
        This is a long function and should potentially be delegated elsewhere and the result cached
        ---------------------------------------------------------------------------------------------*/
        string[] animationDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Animations/" + transform.parent.gameObject.name);

        foreach(string animationDirectory in animationDirectories) {
            // this is called animationStateName because it is expected that the folders are named the same name as the animation state
            string animationStateName = new DirectoryInfo(animationDirectory).Name; 

            string prefixPath = "Animations/" + transform.parent.gameObject.name;

            AnimationClip[] animationClips = new AnimationClip[degreeClipLength];
            for(int i = 0; i < degreeClipLength; i ++) {
                Debug.Log("Now processing folder: " + prefixPath + "/" + animationStateName + "/" + (i * 10));
                // each folder with degree as name would have only one animation clip
                animationClips[i] = Array.ConvertAll<UnityEngine.Object, AnimationClip>( Resources.LoadAll(prefixPath + "/" + animationStateName + "/" + (i * 10), typeof(AnimationClip)), item => (AnimationClip)item )[0];
                Debug.Log(animationClips[i].name);
            }
            
            animationLibrary[animationStateName] = animationClips;
            break; // run once because i only want to test one
        }

        // for(int i = 0 ; i < degreeClipLength ; i++) {
        //     _animIdleClip[i] = (AnimationClip)Resources.Load("Animations/Idle animation clip/"+ i * 10 +"_PlayerIdle");
        // }
    }

    private void Update() {
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        Debug.Log(GetFacingAngle( _camera.ScreenToWorldPoint(Input.mousePosition) ));
        // get animation state name
        string currentAnimationStatename = _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Split('_')[1];
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>( animationLibrary[currentAnimationStatename][0], animationLibrary[currentAnimationStatename][ GetFacingAngle((Vector3) _camera.ScreenToWorldPoint(Input.mousePosition)) / 10]) );
        // anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(OriginalClip, _animIdleClip[GetFacingAngle((Vector3) _camera.ScreenToWorldPoint(Input.mousePosition)) / 10]));
        aoc.ApplyOverrides(anims);
    }
}
