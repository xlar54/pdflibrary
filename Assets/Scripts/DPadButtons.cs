using UnityEngine;
using System.Collections;

public class DPadButtons : MonoBehaviour
{
    public static bool up;
    public static bool down;
    public static bool left;
    public static bool right;

    public float xaxis;
    public float yaxis;

    float lastDpadX;
    float lastDpadY;

    void Start()
    {
        up = down = left = right = false;
        lastDpadX = Input.GetAxis("DPadX");
        lastDpadY = Input.GetAxis("DPadY");
    }

    void Update()
    {
        xaxis = Input.GetAxis("DPadX");
        yaxis = Input.GetAxis("DPadY");

        if (xaxis == 1 && lastDpadX != 1) { right = true; } else { right = false; }
        if (xaxis == -1 && lastDpadX != -1) { left = true; } else { left = false; }
        if (yaxis == 1 && lastDpadY != 1) { up = true; } else { up = false; }
        if (yaxis == -1 && lastDpadY != -1) { down = true; } else { down = false; }
    }
}