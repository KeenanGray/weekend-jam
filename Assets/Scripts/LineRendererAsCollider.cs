using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererAsCollider : MonoBehaviour
{
    GameObject line;
    // Start is called before the first frame update
    void Start()
    {
        line = gameObject;

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        PolygonCollider2D meshCollider = line.AddComponent<PolygonCollider2D>();

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);

        Vector3[] line_renderer_pts = new Vector3[lineRenderer.positionCount];

        Vector2[] collider_pts = new Vector2[lineRenderer.positionCount * 2];
        meshCollider.SetPath(0, collider_pts);

        lineRenderer.GetPositions(line_renderer_pts);

        //to make a box collider out of each line renderer
        //make a polygon using the vertices and an offset.
        for (int i = 0; i < line_renderer_pts.Length; i++)
        {
            Vector2 lr_pt_as_Vector2 = line_renderer_pts[i];
            //offset the point slightly in x and y directions
            //this should handle rotated lines, and generate
            //a rhombus-like collider following the path of the line

            float offset = lineRenderer.startWidth / 2;
            Vector2 lr_pt_offset = lr_pt_as_Vector2 + new Vector2(offset, offset);


            //add the points to the collider array in opposite orders 
            //to create a loops
            collider_pts[i] = lr_pt_as_Vector2;
            collider_pts[(collider_pts.Length - 1) - i] = lr_pt_offset;
        }
        meshCollider.SetPath(0, collider_pts);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
