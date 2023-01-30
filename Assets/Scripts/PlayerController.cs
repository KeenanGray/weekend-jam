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

    //---- SERIALIZED OBJECTS  ----//
    [SerializeField]
    FloatVariable _parallax;
    [SerializeField]
    BoolVariable _is_stargazing;


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
            UseTelescope();

        _playerActions.Player_Map.Telescope.canceled += ctx =>
            StopTelescope();

        _init_scale = transform.localScale;

        _is_stargazing.Value = false;
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
        if (_is_stargazing.Value)
            return;

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

    void SetTelescopeDuration(float t)
    {
        _anim.SetFloat("telescope_hold_time", t);
    }

    void UseTelescope()
    {
        _anim.SetBool("is_stargazing", true);
    }

    void StopTelescope()
    {
        //cancel the stargaze if we release spacebar too early
        //the time can be controlled by adjusting the event
        //in the raise telescope animation
        if (!_is_stargazing.Value)
            _anim.SetTrigger("raise_cancel");

        _anim.SetBool("is_stargazing", false);
        SetTelescopeDuration(0);
    }

    void StartStargazing()
    {
        _is_stargazing.Value = true;
        _rbody.velocity = new Vector3(0, _rbody.velocity.y, 0);
        _anim.SetFloat("speed", 0);
        StartCoroutine("HoldTelescope");
    }

    void FinishStargazing()
    {
        _is_stargazing.Value = false;
        SetTelescopeDuration(0);
        StopCoroutine("HoldTelescope");
    }

    IEnumerator HoldTelescope()
    {
        var wfs = new WaitForSeconds(1.0f);
        while (true)
        {
            if (_is_stargazing.Value)
                _anim.SetFloat("telescope_hold_time", _anim.GetFloat("telescope_hold_time") + 1);
            yield return wfs;
        }
    }
}
