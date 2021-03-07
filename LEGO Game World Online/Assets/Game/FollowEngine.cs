using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEngine : MonoBehaviour
{
    public Transform engineTr;
    public float enginePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = engineTr.position + new Vector3(0f, enginePos, 0f);
    }
}
