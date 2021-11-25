using UnityEngine;

public class UnitVFXManager : MonoBehaviour
{
    private EventProcessor _unitEventHandler; // Internal event handler
    private SpriteRenderer _unitSprite;
    private Material _defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float dissolveAmount;
    [SerializeField] private bool someoneDead;

    // Start is called before the first frame update
    private void Awake()
    {
        _unitEventHandler = GetComponentInParent<UnitEventManager>().UnitEventHandler;
    }

    private void Start()
    {
        _unitSprite = GetComponent<SpriteRenderer>();
        _defaultMaterial = _unitSprite.material;
        if (damageMaterial) _unitSprite.material = damageMaterial;
        dissolveAmount = 1;
        someoneDead = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //ClearEffects();
        if (someoneDead)
        {
            Fading();
        }
    }

    public void HighlightOutline()
    {
        _unitSprite.material = highlightMaterial;
    }

    public void ClearEffects()
    {
        _unitSprite.material = _defaultMaterial;
    }

    private void OnEnable()
    {
        _unitEventHandler.StartListening("OnHealthChanged", UnitDamageRateVfx);
        _unitEventHandler.StartListening("OnDied", OnDied);
    }

    private void OnDisable()
    {
        _unitEventHandler.StopListening("OnHealthChanged", UnitDamageRateVfx);
        _unitEventHandler.StopListening("OnDied", OnDied);
    }

    private void OnDied(object @null)
    {
        _unitSprite.material = dissolveMaterial;
        someoneDead = true;
    }

    private void Fading()
    {
        dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
        _unitSprite.material.SetFloat("Fade", dissolveAmount);
    }

    private void UnitDamageRateVfx(object damageRate)
    {
        _unitSprite.material.SetFloat("damageRate", (float)damageRate);
    }
}