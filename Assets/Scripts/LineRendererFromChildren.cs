using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineRendererFromChildren : MonoBehaviour
{
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        UpdatePositions();
    }


    void UpdatePositions()
    {
        int count = transform.childCount;
        Vector3[] positions = new Vector3[count];

        lr.positionCount = count;
        for (int c = 0; c < count; c++)
        {
            positions[c] = transform.GetChild(c).localPosition;
        }
        lr.SetPositions(positions);
    }
}
