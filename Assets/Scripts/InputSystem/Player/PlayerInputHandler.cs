using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviourPun
{
    //Singleton pattern for Input System

    [SerializeField] private InputActionAsset inputAsset;

    private string actionMapName = "PlayerActionMap";

    private string moveName = "Move";
    private string lookName = "Look";
    private string sprintName = "Sprint";
    private string jumpName = "Jump";
    private string fireName = "Fire";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction fireAction;

    public Vector2 moveInput {  get; private set; }

    public Vector2 lookInput { get; private set; }
    public bool sprintInput { get; private set; }

    public bool jumpInput { get; private set; }

    public bool fireInput { get; private set; }

    public static PlayerInputHandler Instance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Instance = this;
        }

        moveAction = inputAsset.FindActionMap(actionMapName).FindAction(moveName);
        sprintAction = inputAsset.FindActionMap(actionMapName).FindAction(sprintName);
        lookAction = inputAsset.FindActionMap(actionMapName).FindAction(lookName);
        jumpAction = inputAsset.FindActionMap(actionMapName).FindAction(jumpName);
        fireAction = inputAsset.FindActionMap(actionMapName).FindAction(fireName);

        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;

        sprintAction.performed += context => sprintInput = true;
        sprintAction.canceled += context => sprintInput = false;

        jumpAction.performed += context => jumpInput = true;
        jumpAction.canceled += context => jumpInput = false;

        fireAction.performed += context => fireInput = true;
        fireAction.canceled += context => fireInput = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        fireAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        fireAction.Disable();
    }
}
