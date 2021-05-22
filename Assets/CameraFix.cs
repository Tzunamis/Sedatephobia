using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    Camera Camera;
    void Start()
    {
        Camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Camera.backgroundColor = Color.black;
        Camera.clearFlags = CameraClearFlags.SolidColor;
    }
}
