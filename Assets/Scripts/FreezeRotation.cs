using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour{

    void Update() {
        transform.rotation = Quaternion.identity;
    }
}
