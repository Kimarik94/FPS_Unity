using Photon.Pun;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerBody;

    public float mouseSensitivityX = 10f;
    public float mouseSensitivityY = 10f;

    private float xRotation = 0f;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rotateX = PlayerInputHandler.Instance.lookInput.x * mouseSensitivityX * Time.deltaTime;
        float rotateY = PlayerInputHandler.Instance.lookInput.y * mouseSensitivityY * Time.deltaTime;

        xRotation -= rotateY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Vertical rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Horizontal rotation
        playerBody.Rotate(Vector3.up * rotateX);
    }
}
