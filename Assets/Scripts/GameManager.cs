using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    [SerializeField] PlayerController player;
    [SerializeField] Transform spawnpoint;
    [SerializeField] Text fpsText;
    [SerializeField][Range(0,1)] float timescale = 1;
    [SerializeField] int targetFramerate = 60;
    [SerializeField] bool capFramerate = false;

    InputManager input;

    void Awake() {
        input = new InputManager();
        input.Game.Reset.performed += ctx => {
            resetPlayer();
        };
    }

    void Start() {
        Time.timeScale = timescale;
    }

    void Update() {
        if(capFramerate) {
            Application.targetFrameRate = targetFramerate;
        }
        else {
            Application.targetFrameRate = -1;
        }

        fpsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F1") + "\n"
            + "Current State: " + player.GetCurrentState();
    }

    void resetPlayer() {
        player.transform.position = spawnpoint.position;
    }

    void OnEnable() {
        input.Enable();
    }
    void OnDisable() {
        input.Disable();
    }
}
