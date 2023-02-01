using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour
{
    [SerializeField]
    float t;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("UpdateMaterialShader");
    }

    IEnumerator UpdateMaterialShader()
    {
        while (true)
        {
            float val = Random.Range(0.25f, .75f);
            spriteRenderer.material.SetFloat("_Twinkle", val);

            var wfs = new WaitForSeconds(Random.Range(0.0f, t));
            yield return wfs;

        }
    }
}
