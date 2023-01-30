using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float y_offset = 9.0f;

    [SerializeField]
    SpriteRenderer Background;

    //edges.x will be the left bound of the camera
    //edges.y will be the right bound of the camera;
    Vector2 edges;

    public Vector2 camEdges;

    // Start is called before the first frame update
    void Start()
    {
        edges = new Vector2(-Background.bounds.extents.x, Background.bounds.extents.x);
    }

    // Update is called once per frame
    void Update()
    {
        //keep the camera in bounds while following the player.
        //subtract the width of the ortho camera from the current position
        //to get the left edge of the frame.
        float xPos = player.transform.position.x;
        float w = GetOrthoWidth(GetComponent<Camera>());

        camEdges.x = edges.x + w/2;
        camEdges.y = edges.y - w/2;

        if (player.transform.position.x < 0)
        {
            transform.position = new Vector3(Mathf.Max(xPos, camEdges.x), player.transform.position.y + y_offset, -10);
        }
        else
        {
            transform.position = new Vector3(Mathf.Min(xPos, camEdges.y), player.transform.position.y + y_offset, -10);
        }
    }

    float GetOrthoWidth(Camera cam)
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        return width;
    }
}
