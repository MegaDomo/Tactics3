using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [Header("Unity References")]
    public Transform cameraRotationPoint;

    [Header("Attributes")]
    public float cameraMoveSpeed;
    public float cameraRotateSpeed;
    public float cameraRotateAngle;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Vector2 move = controls.Player.Move.ReadValue<Vector2>();
        MoveCamera(move.x, move.y);
        //RotateCamera();
    }

    private void MoveCamera(float x, float z)
    {
        if (x == 0 && z == 0)
            return;

        Vector3 dir = new Vector3(x, 0, z);

        transform.Translate(dir * cameraMoveSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (controls.Player.RotateLeft.IsPressed())
            transform.RotateAround(cameraRotationPoint.position, Vector3.up, cameraRotateAngle);
    }
}
