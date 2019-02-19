using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour {

    private Vector3 lastFrameMousePos;
    private Vector3 offset;
    private bool firstUpdate = true;

    void Update () {
        MoveCamera();
        ZoomCamera();
	}

    /// <summary>
    /// Zooms camera when scrolling
    /// </summary>
    private void ZoomCamera()
    {
        // Scroll in
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 50);
        }
        // Scroll out
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 1);
        }
    }

    /// <summary>
    /// Moves the camera by holding right click and moving mouse
    /// </summary>
    private void MoveCamera()
    {
        if (Input.GetMouseButton(1))
        {
            if (firstUpdate)
            {
                firstUpdate = false;
            }
            else
            {
                // Get distance and direction the mouse have moved since last update and offset the camera by that amount 
                offset = lastFrameMousePos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position += offset;
            }

            lastFrameMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Save current mouse position for next update
        }
        else
        {
            firstUpdate = true;
        }
    }
}
