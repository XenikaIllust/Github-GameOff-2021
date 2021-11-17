using System.Collections;
using UnityEngine;

public class UnitVFXManager : MonoBehaviour
{
    private SpriteRenderer unitSprite;
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    // Start is called before the first frame update
    void Start()
    {
        unitSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = unitSprite.material;
    }

    // Update is called once per frame
    void Update()
    {
        ClearEffects();
    }

    public void HighlightOutline()
    {
        unitSprite.material = highlightMaterial;
    }

    public void ClearEffects()
    {
        unitSprite.material = defaultMaterial;
    }
}
