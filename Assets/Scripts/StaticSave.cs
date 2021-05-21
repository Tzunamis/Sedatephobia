using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSave : MonoBehaviour
{
    public static int InRoom = 2;
    public static Vector3 RotateZero = new Vector3(0, 0, 0);
    public static Vector3 Position1 = new Vector3(0, 1.2f, -100);
    public Vector3[] WarpPositions;
    void Start()
    {
        WarpPositions = new Vector3[11];
        WarpPositions[1] = new Vector3(0, 1.2f, -106.5f);
        WarpPositions[2] = new Vector3(0, 1.2f, 6.5f);
        WarpPositions[3] = new Vector3(-6.4f, 1.2f, 130.1f);
        WarpPositions[4] = new Vector3(7.8f, 1.2f, 268.1f);
        WarpPositions[5] = new Vector3(10, 1.2f, 420.2f);
        WarpPositions[6] = new Vector3(24.5f, 1.2f, 523.8f);
        WarpPositions[7] = new Vector3(25.7f, 1.2f, 572.7f);
        WarpPositions[8] = new Vector3(29.8f, 1.2f, 719.7f);
        WarpPositions[9] = new Vector3(39.3f, 1.2f, 815.8f);
        WarpPositions[10] = new Vector3(50.3f, 1.2f, 1155.9f);
    }
}
