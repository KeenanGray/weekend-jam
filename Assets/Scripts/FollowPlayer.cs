using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float y_offset = 9.0f;

    private float init_y_offset;

    [SerializeField]
    SpriteRenderer Background;

    [SerializeField]
    TelescopeSettings telescopeSettings;

    //edges.x will be the left bound of the camera
    //edges.y will be the right bound of the camera;
    Vector2 edges;

    public Vector2 camEdges;

    // Start is called before the first frame update
    void Start()
    {
        init_y_offset = y_offset;
        edges = new Vector2(-Background.bounds.extents.x, Background.bounds.extents.x);
    }

    // Update is called once per frame
    void Update()
    {
        var height = Mathf.Clamp(telescopeSettings.time.ConstantValue.Remap(0, .5f, init_y_offset, init_y_offset + 8), 0, 300 - (2 * Camera.main.orthographicSize));
        y_offset = height;

        var cam_size = Mathf.Clamp(telescopeSettings.time.ConstantValue.Remap(0, 8, 15, 100), 15, 100);
        Camera.main.orthographicSize = cam_size;

        //keep the camera in bounds while following the player.
        //subtract the width of the ortho camera from the current position
        //to get the left edge of the frame.
        float xPos = player.transform.position.x;
        float w = GetOrthoWidth(GetComponent<Camera>());

        camEdges.x = edges.x + w / 2;
        camEdges.y = edges.y - w / 2;

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
