using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Transform aimDir;
    [SerializeField] float aimSensitivity = 1f;
    [SerializeField][Range(1,50)] float tempShootForce = 15;

    Rigidbody rb;
    InputManager input;

    bool shoot;
    bool stop;

    enum PlayerState {
        MOVING,
        AIMING,
        SHOOTING
    };

    PlayerState state;

    void Awake() {
        rb = GetComponent<Rigidbody>();

        #region input
        input = new InputManager();
        input.Player.Shoot.performed += ctx => {
            shoot = true;
        };
        input.Player.Shoot.canceled += ctx => {
            shoot = false;
        };
        input.Player.Stop.performed += ctx => {
            stop = true;
        };
        input.Player.Stop.canceled += ctx => {
            stop = false;
        };
        #endregion
    }

    void Start() {
        state = PlayerState.AIMING;
    }

    void FixedUpdate() {
        rb.freezeRotation = false;
        switch(state) {
            // ==== MOVING ====
            case PlayerState.MOVING:

                if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude < 0.3f) {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    rb.freezeRotation = true;
                }
                if(rb.velocity == Vector3.zero) {
                    state = PlayerState.AIMING;
                }
                if(stop) {
                    rb.velocity = Vector3.zero;
                }
                Debug.DrawRay(transform.position, rb.velocity, Color.green, Time.deltaTime);

                break;

            // ==== AIMING ====
            case PlayerState.AIMING:
                if(rb.velocity != Vector3.zero) {
                    state = PlayerState.MOVING;
                }
                if(shoot) {
                    rb.AddForce(aimDir.forward * tempShootForce, ForceMode.Impulse);
                }
                
                break;

            // ==== SHOOTING ====
            case PlayerState.SHOOTING:

                break;

            // if the states somehow get broken, set back to aiming
            default:
                state = PlayerState.AIMING;
                break;
        }
    }

    void Update() {
        float aimDelta = input.Player.Aim.ReadValue<float>();
        aimDir.Rotate(Vector3.up, aimDelta * aimSensitivity * 0.5f);
        Debug.DrawRay(aimDir.position, aimDir.forward, Color.red, Time.deltaTime);
    }

    public string GetCurrentState() {
        switch(state) {
            case PlayerState.MOVING:
                return "Moving";
            case PlayerState.AIMING:
                return "Aiming";
            case PlayerState.SHOOTING:
                return "Shooting";
            default:
                return "Unknown";
        }
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }
}
