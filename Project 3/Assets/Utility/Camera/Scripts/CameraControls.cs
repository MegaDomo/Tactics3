using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [Header("Unity References")]
    public Transform cameraRotationPoint;
    public Transform cameraTransform;

    [Header("Attributes")]
    public float cameraMoveSpeed;
    public float cameraRotateSpeed;
    public float zoomDistance;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();

    private void Update()
    {
        transform.LookAt(cameraRotationPoint);
        Vector2 move = controls.Player.Move.ReadValue<Vector2>();
        MoveCamera(move.x, move.y);
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera(float xAxis, float zAxis)
    {
        if (xAxis == 0 && zAxis == 0)
            return;
        Vector3 move = cameraRotationPoint.right * xAxis + cameraRotationPoint.forward * zAxis;
        cameraTransform.Translate(move * cameraMoveSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float speed = cameraRotateSpeed * Time.deltaTime;
        if (controls.Player.RotateLeft.IsPressed())
            cameraRotationPoint.Rotate(Vector3.up, speed);
        if (controls.Player.RotateRight.IsPressed())
            cameraRotationPoint.Rotate(Vector3.up, -speed);
    }

    private void ZoomCamera()
    {
        float axis = controls.Player.Scroll.ReadValue<float>();
        if (axis == 0)
            return;

        if (axis > 0)
            transform.Translate(Vector3.up * zoomDistance);
        if (axis < 0)
            transform.Translate(-Vector3.up * zoomDistance);
    }
}
