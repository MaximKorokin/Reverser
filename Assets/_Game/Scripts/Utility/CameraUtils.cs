using UnityEngine;

public static class CameraUtils
{
    public static float GetUnitScreenSize(this Camera camera, float scaleFactor)
    {
        //camera = camera == null ? Camera.main : camera;
        //return Screen.height / camera.orthographicSize / 2;
        Vector3 screenPoint1 = camera.WorldToScreenPoint(Vector2.zero);
        Vector3 screenPoint2 = camera.WorldToScreenPoint(camera.transform.right);
        return (screenPoint2 - screenPoint1).magnitude / scaleFactor;
    }
}
