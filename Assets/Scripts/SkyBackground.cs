using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkyBackground : MonoBehaviour
{
    TilemapRenderer tilemap;
    Material atmosphere;

    [SerializeField]
    [Range(1, 1000)]
    float x;
    [SerializeField]
    [Range(1, 250)]
    float y;

    private void Awake()
    {
        tilemap = GetComponentInChildren<TilemapRenderer>();
        atmosphere = tilemap.material;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float pos_sin = Mathf.Sin(Time.frameCount * 1 / x) + 1 / 2;
        atmosphere.SetFloat("_Scalar", (pos_sin) + y);
        tilemap.material = atmosphere;
    }
}
