using UnityEngine;

public static class CameraUtils
{
    public static float GetUnitScreenSize(this Camera camera)
    {
        camera = camera == null ? Camera.main : camera;
        return Screen.height / camera.orthographicSize / 2;
    }
}