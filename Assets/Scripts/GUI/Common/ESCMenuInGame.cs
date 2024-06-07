using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ESCMenuInGame : MonoBehaviourPun
{
    private PlayerInputHandler inputHandler;
    private PlayerMovements playerMovements;
    private CameraMovement cameraMovement;
    private GunLogic gun;

    private RectTransform parentPan;

    private bool isOpenned = false;

    private Button resumeGameBtn;
    private Button optionsBtn;
    private Button leaveRoomBtn;
    private Button exitAppBtn;

    private void Start()
    {
        inputHandler = transform.root.root.root.GetComponent<PlayerInputHandler>();
        playerMovements = transform.root.root.root.GetComponent<PlayerMovements>();
        cameraMovement = transform.root.root.root.GetComponentInChildren<CameraMovement>();
        gun = transform.root.root.root.GetComponentInChildren<GunLogic>();

        parentPan = GetComponent<RectTransform>();
        
        resumeGameBtn = GameObject.Find("ResumeGame").GetComponent<Button>();
        optionsBtn = GameObject.Find("Options").GetComponent<Button>();
        leaveRoomBtn = GameObject.Find("LeaveRoom").GetComponent<Button>();
        exitAppBtn = GameObject.Find("ExitApp").GetComponent<Button>();

        resumeGameBtn.onClick.AddListener(ResumeGame);
        optionsBtn.onClick.AddListener(OptionsMenu);
        leaveRoomBtn.onClick.AddListener(LeaveRoom);
        exitAppBtn.onClick.AddListener(ExitApplication);

        parentPan.transform.localScale = Vector3.zero;

        inputHandler.onEscPressed += OpenClosePan;
    }

    private void OnDisable()
    {
        inputHandler.onEscPressed -= OpenClosePan;
    }

    private void OpenClosePan()
    {
        if (!isOpenned)
        {
            parentPan.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            playerMovements.enabled = false;
            cameraMovement.enabled = false;
            gun.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (isOpenned)
        {
            parentPan.transform.localScale = Vector3.zero;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovements.enabled = true;
            cameraMovement.enabled = true;
            gun.enabled = true;
        }

        isOpenned = !isOpenned;
    }

    private void ClosePan()
    {
        
    }

    private void ResumeGame()
    {
        OpenClosePan();
    }

    private void OptionsMenu()
    {
        Debug.Log("Options Menu Openned");
    }

    private void LeaveRoom()
    {
        if(photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom(this);
            PhotonNetwork.LoadLevel("Main_Menu");
        }
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
}
