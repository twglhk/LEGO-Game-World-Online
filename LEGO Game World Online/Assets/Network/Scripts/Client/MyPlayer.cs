using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MyPlayer : Player
{
    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>().GetComponent<NetworkManager>();
        var freelookCam = GameObject.FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineFreeLook>();
        freelookCam.LookAt = transform;
        freelookCam.Follow = transform;
        StartCoroutine("CoSendPacket");
    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = transform.position.x;
            movePacket.posY = transform.position.y;
            movePacket.posZ = transform.position.z;

            networkManager.Send(movePacket.Write());
        }
    }
}
