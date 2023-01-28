using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]
    private float _speed;
    private PlayerActions _playerActions;
    private Rigidbody2D _rbody;
    private Vector2 _moveInput;
    private Vector3 saved_scale;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody2D>();

        _playerActions = new PlayerActions();

        _playerActions.Player_Map.Sprint.started += ctx =>
        {
            _speed = 6;
        };

        _playerActions.Player_Map.Sprint.canceled += ctx =>
        {
            _speed = 1;
        };

        saved_scale = transform.localScale;
    }

    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }

    private void FixedUpdate()
    {
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
        _rbody.velocity = _moveInput * _speed;

        if (_rbody.velocity.x == 0)
            transform.localScale = saved_scale;
        else 
            {
                transform.localScale = new Vector3(_rbody.velocity.normalized.x, transform.localScale.y, transform.localScale.z);
                saved_scale =transform.localScale;
            }
        int abs_speed = Mathf.CeilToInt(Mathf.Abs(_rbody.velocity.x));
        _anim.SetFloat("speed", abs_speed);
    }
}
