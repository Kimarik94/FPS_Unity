using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public GameObject cameraHolder;
    private GameObject playerCamera;

    public GameObject gunPrefab;
    public GameObject gunHolder;

    public GameObject uiPrefab;

    private void Start()
    {
        if (photonView.IsMine)
        {
            IsLocalPlayer();
        }
    }

    public void IsLocalPlayer()
    {
        CreatePlayerCamera();
        EnablePlayerControls();
    }

    private void CreatePlayerCamera()
    {
        playerCamera = new GameObject("PlayerCamera");

        Camera cameraComponent = playerCamera.AddComponent<Camera>();
        CameraMovement cameraMovement = playerCamera.AddComponent<CameraMovement>();
        cameraMovement.playerBody = transform;
        playerCamera.tag = "MainCamera";

        cameraComponent.nearClipPlane = 0.01f;
        cameraComponent.fieldOfView = 65.0f;

        playerCamera.transform.SetParent(cameraHolder.transform);
        playerCamera.transform.localRotation = Quaternion.identity;
        playerCamera.transform.localPosition = Vector3.zero;

        playerCamera.AddComponent<AudioListener>();

        CreateUI(cameraComponent);

        ConnectTransformCameraToGun(playerCamera);
    }

    private void ConnectTransformCameraToGun(GameObject obj)
    {
        gunHolder.GetComponent<GunFollowCamera>().cameraTransform = obj.transform;
    }

    private void CreateUI(Camera camera)
    {
        GameObject temp = Instantiate(uiPrefab, cameraHolder.transform); // изменено с gameObject.transform на cameraHolder.transform
        Canvas canvas = temp.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = camera;
    }

    private void EnablePlayerControls()
    {
        // ¬ключить компоненты, отвечающие за управление игроком
        GetComponent<PlayerMovements>().enabled = true;
        GetComponentInChildren<CameraMovement>().enabled = true;
        GetComponentInChildren<GunLogic>().enabled = true;
    }
}
