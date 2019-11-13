using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraUI : MonoBehaviour
{
    private Transform playerCamera;
    private void Awake()
    {
        playerCamera = FindObjectOfType<Camera>().GetComponent<Transform>();
    }

    // Update is called once per frameplayerCamera
    void Update()
    {
        transform.LookAt(playerCamera);
        transform.Rotate(0, 180, 0);
    }
}
