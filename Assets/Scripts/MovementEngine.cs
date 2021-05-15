using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEngine : MonoBehaviour
{
    public bool controllable;
    public float hScrollSpeed;
    public float vScrollSpeed;
    float xRot;
    float yRot;
    float mousePosX;
    float mousePosY;
    GameObject Camera;
    CharacterController Controller;
    public float MoveSpeed;
    float velocity;
    float StartHeight;
    void Start()
    {
        StartHeight = gameObject.transform.position.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera = GameObject.Find("Camera");
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controllable)
        {
            // Mouse movement dictates camera angle
            mousePosX = Input.GetAxis("Mouse X") * hScrollSpeed;
            mousePosY = Input.GetAxis("Mouse Y") * vScrollSpeed;
            xRot += mousePosX;
            yRot -= mousePosY;
           // xRot = Mathf.Clamp(xRot, -90, 90);
            Camera.transform.eulerAngles = new Vector3(yRot, xRot, 0);
            // Keyboard input dictates movement
            float Xvel = Input.GetAxis("LeftRight") * MoveSpeed;
            float Yvel = Input.GetAxis("UpDown") * MoveSpeed;
            Controller.Move((Camera.transform.right * Xvel + Camera.transform.forward * Yvel));
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, StartHeight, gameObject.transform.position.z);
        }
    }
}