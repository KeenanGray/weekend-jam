using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]
    private float _init_speed;
    [SerializeField]
    private float _init_sprintSpeed;

    private PlayerActions _playerActions;
    private Rigidbody2D _rbody;
    private Vector2 _moveInput;
    private Vector3 _init_scale;

    private float _speed;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody2D>();

        _playerActions = new PlayerActions();

         _speed = _init_speed;
        
        _playerActions.Player_Map.Sprint.started += ctx =>
        {
            _speed = _init_sprintSpeed;
        };

        _playerActions.Player_Map.Sprint.canceled += ctx =>
        {
            _speed = _init_speed;
        };

        _init_scale = transform.localScale;
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
        _rbody.velocity = new Vector3(_moveInput.x * _speed, _rbody.velocity.y);

        if (_rbody.velocity.x == 0)
            transform.localScale = _init_scale;
        else
        {
            transform.localScale = new Vector3(Mathf.Sign(_rbody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _init_scale = transform.localScale;
        }
        int abs_speed = Mathf.CeilToInt(Mathf.Abs(_rbody.velocity.x));
        _anim.SetFloat("speed", abs_speed);
    }
}
