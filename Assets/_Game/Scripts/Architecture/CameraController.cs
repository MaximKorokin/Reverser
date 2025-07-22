using UnityEngine;

public class CameraController : MonoBehaviourBase
{
    public Camera Camera { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Camera = GetRequiredComponent<Camera>();
    }

    public void MoveCamera(Vector2 position)
    {
        Camera.transform.position = new Vector3(position.x, position.y, Camera.transform.position.z);
    }
}
