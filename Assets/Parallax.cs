using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float parallax_speed;

    Vector3 _init_pos;
    Vector3 _player_init_pos;



    // Start is called before the first frame update
    void Start()
    {
        _init_pos = transform.position;
        _player_init_pos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x_pos = _init_pos.x - Mathf.Abs(_player_init_pos.x - player.transform.position.x);
        x_pos *= parallax_speed;
        transform.position = new Vector3(x_pos, _init_pos.y, _init_pos.z);
    }
}
