using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcVFXController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject pseudoObject;

    Material arcMaterial;

    public float arcWidth;

    void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        pseudoObject = transform.parent.Find("PseudoObject").gameObject;
    }

    private void Start() {
        arcMaterial = Instantiate<Material>(Resources.Load<Material>("VFX/AbilityVFX/ArcVFX/ArcIndicator/ArcMaterial"));
        arcMaterial.SetFloat("_Angle", -90f);
        arcMaterial.SetFloat("_Arc1", 180 - arcWidth / 2);
        arcMaterial.SetFloat("_Arc2", 180 - arcWidth / 2);
        spriteRenderer.material = arcMaterial;
    }

    private void Update() {
        var rot = pseudoObject.transform.rotation.eulerAngles;
        rot.z += 180;
        // spriteRenderer.transform.rotation = Quaternion.Euler(rot);
        arcMaterial.SetFloat("_Angle", rot.z);
    }
}