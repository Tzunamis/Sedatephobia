using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyBoySpin : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y + 1, 0);
    }
}
