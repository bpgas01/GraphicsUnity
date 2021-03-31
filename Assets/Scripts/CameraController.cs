﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensVertical = 100.0f;

    public float maxRot = 90f;
    public float minRot = -90f;
    float mouseY = 0f;
    
    private void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");
        //negative to invert
        float Y = -mouseY * mouseSensVertical * Time.deltaTime;

        Y = Mathf.Clamp(Y, minRot, maxRot);
        transform.Rotate(new Vector3(1,0,0) * Y);
    }
}
