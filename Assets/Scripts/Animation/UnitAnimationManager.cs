using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitAnimationManager : MonoBehaviour
{
    private EventProcessor _unitEventHandler;
    private AnimationClip[] _animIdleClip;
    private AnimationClip _originalClip; // hardcode
    private AnimatorOverrideController _aoc;
    private Camera _camera;
    private int _degreeClipLength;
    private object _lastDestination;

    private float _azimuthRotation;

    private Animator Animator { get; set; }

    private readonly Dictionary<string, AnimationClip[]> _animationLibrary = new Dictionary<string, AnimationClip[]>();

    [SerializeField] private int degreeVariation;

    [SerializeField]
    [Tooltip("All unique animation state names. Ensure that the corresponding folder has the same name.")]
    private List<string> animationStateNames = new List<string>();

    // hash variables for animation state changing
    private int _isRunningHash;

    private void Awake()
    {
        _unitEventHandler = GetComponentInParent<UnitEventManager>().UnitEventHandler;
        _degreeClipLength = 36;
        degreeVariation = 10;
        Animator = GetComponent<Animator>();
        _camera = Camera.main;
        _animIdleClip = new AnimationClip[_degreeClipLength];

        LoadResources();

        _originalClip = _animIdleClip[0]; // hardcode
        _aoc = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = _aoc;

        _isRunningHash = Animator.StringToHash("isRunning");
    }

    private void Start()
    {
        _unitEventHandler.StartListening("OnPseudoObjectRotationChanged", OnPseudoObjectRotationChanged);
        _unitEventHandler.StartListening("OnStartMoveAnimation", delegate { SetMoveAnimation(true); });
        _unitEventHandler.StartListening("OnStopMoveAnimation", delegate { SetMoveAnimation(false); });
    }

    private void OnPseudoObjectRotationChanged(object destination)
    {
        _lastDestination = destination;
        UpdateAnimationRotation(_lastDestination);
    }

    private void UpdateAnimationRotation(object destination)
    {
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        float rotationAngle = (float)destination;
        // standard bearing to azimuth is required because the animations are saved in azimuth format
        _azimuthRotation = MathUtils.ConvertStandardToAzimuth(rotationAngle);
        string currentAnimationStateName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Split('_')[1];
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(_animationLibrary[currentAnimationStateName][0],
            _animationLibrary[currentAnimationStateName][(int)_azimuthRotation / 10]));
        _aoc.ApplyOverrides(anims);
    }

    private int GetFacingAngle(Vector3 mousePosition)
    {
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float angleFixed = angle < 0 ? 360.0f + angle : angle;
        angleFixed = Mathf.Round(angleFixed / degreeVariation) * degreeVariation;
        angleFixed = angleFixed < 360 ? angleFixed : 0;

        return (int)angleFixed;
    }

    private void LoadResources()
    {
        /*---------------------------------------------------------------------------------------------
        Loads all animations based on properly named folder into animationLibrary dictionary
        This is a long function and should potentially be delegated elsewhere and the result cached
        ---------------------------------------------------------------------------------------------*/

        foreach (string animationStateName in animationStateNames)
        {
            // it is expected that the folders are named the same name as
            // the animation state specified in the list of UnitAnimationManager
            string prefixPath
                = "Animations/"
                  + transform.parent.gameObject.name
                      .Split('(')[0]; // split the word "Clone" and take only the true name

            AnimationClip[] animationClips = new AnimationClip[_degreeClipLength];
            for (int i = 0; i < _degreeClipLength; i++)
            {
                // each folder with degree as name would have only one animation clip
                animationClips[i]
                    = Array.ConvertAll(
                        Resources.LoadAll(prefixPath + "/" + animationStateName + "/" + i * 10, typeof(AnimationClip)),
                        item => (AnimationClip)item)[0];
            }

            _animationLibrary[animationStateName] = animationClips;
        }
    }

    private void SetMoveAnimation(bool status)
    {
        Animator.SetBool(_isRunningHash, status);
        UpdateAnimationRotation(_lastDestination);
    }

    public void Play(string animationStateName)
    {
        Animator.Play(animationStateName, -1, 0f);

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        string currentAnimationStateName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Split('_')[1];
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(_animationLibrary[currentAnimationStateName][0],
            _animationLibrary[currentAnimationStateName][(int)_azimuthRotation / 10]));
        _aoc.ApplyOverrides(anims);
    }
}
