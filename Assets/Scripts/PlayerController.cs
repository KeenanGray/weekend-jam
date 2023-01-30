using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //---- MOVEMENT VARIABLES ----//
    [SerializeField]
    private float _init_speed;
    [SerializeField]
    private float _init_sprintSpeed;

    //---- COMPONENT VARIABLES ----//
    private PlayerActions _playerActions;
    private Rigidbody2D _rbody;
    private Animator _anim;

    //---- PRIVATE VARIABLES ----//
    private Vector2 _moveInput;
    private Vector3 _init_scale;
    private float _speed;
    private Vector3 _initPos;

    private bool no_movement = false;
    private bool anim_restricts_movement = false;

    //---- SERIALIZED OBJECTS  ----//
    [SerializeField]
    FloatVariable _parallax;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody2D>();

        _playerActions = new PlayerActions();

        _speed = _init_speed;

        _playerActions.Player_Map.Sprint.started += ctx =>
            _speed = _init_sprintSpeed;

        _playerActions.Player_Map.Sprint.canceled += ctx =>
            _speed = _init_speed;

        _playerActions.Player_Map.Telescope.started += ctx =>
           {
               _anim.SetBool("space_key_held", true);
           };

        _playerActions.Player_Map.Telescope.canceled += ctx =>
            {
                _anim.SetBool("space_key_held", false);
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

    private void Update()
    {
        anim_restricts_movement = _anim.GetBool("space_key_held") || _anim.GetBool("is_stargazing");
    }
    private void FixedUpdate()
    {

        if (no_movement || anim_restricts_movement)
        {
            _rbody.velocity = new Vector3(0, 0, 0);
            _anim.SetFloat("speed", 0);
            return;
        }

        int abs_speed = Mathf.CeilToInt(Mathf.Abs(_rbody.velocity.x));
        _anim.SetFloat("speed", abs_speed);

        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
        _rbody.velocity = new Vector3(_moveInput.x * _speed, _rbody.velocity.y);

        if (_rbody.velocity.x == 0)
            transform.localScale = _init_scale;
        else
        {
            transform.localScale = new Vector3(Mathf.Sign(_rbody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _init_scale = transform.localScale;
        }
    }

    public void SetNoMovement(bool b)
    {
        no_movement = b;
    }

    void BeginStargazing()
    {
        _anim.SetBool("is_stargazing", true);
        StartCoroutine("HoldTelescope");
    }

    public void EndStargazing()
    {
        _anim.SetBool("is_stargazing", false);
        StopCoroutine("HoldTelescope");
    }

    IEnumerator HoldTelescope()
    {
        var wfs = new WaitForSeconds(1.0f);
        while (true)
        {
            if (_anim.GetBool("is_stargazing"))
                _anim.SetFloat("telescope_hold_time", _anim.GetFloat("telescope_hold_time") + 1);
            else
                break;
            yield return wfs;
        }
        yield break;
    }
}
