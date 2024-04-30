using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;


public class CameraSetup : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}
