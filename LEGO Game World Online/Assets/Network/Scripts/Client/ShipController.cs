using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var moveVec = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
            moveVec.x += 0.01f;

        if (Input.GetKeyDown(KeyCode.S))
            moveVec.x -= 0.01f;

        if (Input.GetKeyDown(KeyCode.A))
            moveVec.z += 0.01f;

        if (Input.GetKeyDown(KeyCode.D))
            moveVec.z -= 0.01f;

        transform.position += moveVec;
    }
}
