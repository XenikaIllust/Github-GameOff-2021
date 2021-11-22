using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class UnitVFXManager : MonoBehaviour
{
    EventProcessor UnitEventHandler; // Internal event handler
    private SpriteRenderer unitSprite;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material dissloveMaterial;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float dissolveAmount;
    [SerializeField] private bool someoneDead;


    // Start is called before the first frame update
    void Awake()
    {
        UnitEventHandler = GetComponentInParent<UnitEventManager>().UnitEventHandler;
    }
    void Start()
    {
        unitSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = unitSprite.material;
        if(damageMaterial) unitSprite.material = damageMaterial;
        dissolveAmount = 1;
        someoneDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ClearEffects();
        if(someoneDead){
            Fadding();
        }
    }

    public void HighlightOutline()
    {
        unitSprite.material = highlightMaterial;
    }

    public void ClearEffects()
    {
        unitSprite.material = defaultMaterial;
    }
    private void OnEnable()
    {
        UnitEventHandler.StartListening("OnUpdateDamageRate", UnitDamageRateVfx);
        UnitEventHandler.StartListening("OnDied", OnDied);
    }
    private void OnDisable()
    {
        UnitEventHandler.StopListening("OnUpdateDamageRate", UnitDamageRateVfx);
        UnitEventHandler.StopListening("OnDied", OnDied);
    }


    private void OnDied(object @null)
    {
        unitSprite.material = dissloveMaterial;
        someoneDead = true;
    }
    private void Fadding(){
        dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
        unitSprite.material.SetFloat("Fade", dissolveAmount);
    }
    private void UnitDamageRateVfx(object damageRate){
        unitSprite.material.SetFloat("damageRate" , (float)damageRate);
        print((float)damageRate * 100 +"%");
    }
}
