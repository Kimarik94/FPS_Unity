using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    public Transform playerBody;

    public float mouseSensitivityX = 10f;
    public float mouseSensitivityY = 10f;

    private float xRotation = 0f;

    private void Start()
    {
        inputHandler = transform.root.root.GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rotateX = inputHandler.lookInput.x * mouseSensitivityX * Time.deltaTime;
        float rotateY = inputHandler.lookInput.y * mouseSensitivityY * Time.deltaTime;

        xRotation -= rotateY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * rotateX);
    }
}
