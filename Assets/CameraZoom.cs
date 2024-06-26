using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cameraToZoom; // Assign this in the inspector
    public float targetFOV = 30f; // Target field of view
    public float zoomDuration = 2f; // Duration of the zoom effect

    // Call this method to start zooming
    public void StartZoom()
    {
        StartCoroutine(ZoomCamera(targetFOV, zoomDuration));
    }

    IEnumerator ZoomCamera(float targetFOV, float duration)
    {
        float startFOV = cameraToZoom.fieldOfView;
        float time = 0;

        while (time < duration)
        {
            // Smoothly interpolate the camera's field of view
            cameraToZoom.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the target FOV is set after the loop
        cameraToZoom.fieldOfView = targetFOV;
    }
}