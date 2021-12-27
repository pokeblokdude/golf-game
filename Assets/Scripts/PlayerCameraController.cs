using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCameraController : MonoBehaviour {
    
    [SerializeField] Transform camTarget;
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed = 1;

    InputManager input;

    Quaternion rotation;

    float rotationDir;
    bool rotCCl = false, rotCl = false;
    bool rotating;
    bool holdingRot = false;
    float rotationAmount;
    float zoom = -10;
    float zoomChange;

    void Awake() {
        input = new InputManager();
        input.Player.LookCCl.performed += ctx => {
            rotCCl = true;
        };
        input.Player.LookCCl.canceled += ctx => {
            rotCCl = false;
        };
        input.Player.LookCl.performed += ctx => {
            rotCl = true;
        };
        input.Player.LookCl.canceled += ctx => {
            rotCl = false;
        };
        
    }

    void Start() {
        DOTween.Init();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotation = camTarget.rotation;
    }

    void Update() {
        bool rotateBothWays = rotCCl && rotCl;
        if(rotCCl) {
            rotationAmount = -90;
        }
        else if(rotCl) {
            rotationAmount = 90;
        }
        if((rotCCl || rotCl) && !rotateBothWays && !rotating) {
            camTarget.DORotate(new Vector3(0, camTarget.rotation.eulerAngles.y + rotationAmount, 0), rotationSpeed).OnComplete(
                () => {
                    rotating = false;
                    rotation = camTarget.rotation;
                }
            );
            rotating = true;
        }
        camTarget.rotation = Quaternion.Euler(0, camTarget.rotation.eulerAngles.y, 0);
        if(!rotating) {
            camTarget.rotation = rotation;
        }
        
        
        //lookDir = input.Player.Look.ReadValue<Vector2>();
        // mouseY -= lookDir.y * 0.1f * rotationSpeed;
        // mouseY = Mathf.Clamp(mouseY, -89, 35); 
        transform.LookAt(camTarget);

        zoomChange = Mathf.Clamp(input.Player.Zoom.ReadValue<float>(), -1, 1);
        zoom += zoomChange;
        zoom = Mathf.Clamp(zoom, -30, -2);
        //Debug.Log(zoomChange);
        //transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, zoom), 0.5f);
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }

}
