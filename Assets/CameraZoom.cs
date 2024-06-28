using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cameraToZoom; 
    public float targetFOV = 30f; 
    public float zoomDuration = 2f; 


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
       
            cameraToZoom.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cameraToZoom.fieldOfView = targetFOV;
    }
}