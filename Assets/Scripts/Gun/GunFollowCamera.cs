using UnityEngine;

public class GunFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    public float transformSmoothness = 1.0f;
    public float rotationSmoothness = 1.0f;

    private void LateUpdate()
    {
        SyncWithCamera();
    }

    private void SyncWithCamera()
    {
        if (cameraTransform != null)
        {
            // Обновляем позицию пушки с учетом смещения
            transform.position = Vector3.Lerp(transform.position, cameraTransform.position + cameraTransform.TransformVector(positionOffset), transformSmoothness);

            // Обновляем поворот пушки с учетом смещения
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraTransform.rotation * Quaternion.Euler(rotationOffset), rotationSmoothness);
        }
    }
}
