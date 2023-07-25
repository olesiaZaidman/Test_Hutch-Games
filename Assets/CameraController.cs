using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    Vector3 cameraPos = new Vector3(0, 1, -11.2f);
    void Start()
    {
        FindCamera();
        SetCameraBackgoundColor(Color.black);
        SetCameraPosition(cameraPos);
    }

    void FindCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.Log("Camera is missing");
        }
    }
    void SetCameraBackgoundColor(Color color)
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = color;
        }
    }


    void SetCameraPosition(Vector3 position)
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = position;
        }
    }
}
