using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEngine : MonoBehaviour
{
    public Transform engineTr;
    public float enginePos;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            velocity.x += 0.01f;

        if (Input.GetKey(KeyCode.S))
            velocity.x -= 0.01f;

        if (Input.GetKey(KeyCode.A))
            velocity.z += 0.01f;

        if (Input.GetKey(KeyCode.D))
            velocity.z -= 0.01f;

        Debug.Log($"[MOVE VEC] {velocity}");
        transform.position += velocity;

        //transform.position = engineTr.position + new Vector3(0f, enginePos, 0f);
    }
}
