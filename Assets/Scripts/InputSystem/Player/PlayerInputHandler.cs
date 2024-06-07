using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviourPun
{
    private InputActionAsset inputAsset;

    private string actionMapName = "PlayerActionMap";

    private string moveName = "Move";
    private string lookName = "Look";
    private string sprintName = "Sprint";
    private string jumpName = "Jump";
    private string fireName = "Fire";
    private string reloadName = "Reload";
    private string escMenuName = "Menu";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction fireAction;
    private InputAction reloadAction;
    private InputAction escAction;

    //For ESC button
    public Action onEscPressed;

    public Vector2 moveInput { get; private set; }

    public Vector2 lookInput { get; private set; }
    public bool sprintInput { get; private set; }

    public bool jumpInput { get; private set; }

    public bool fireInput { get; private set; }

    public bool reloadInput { get; private set; }

    public bool escInput { get; private set; }

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        inputAsset = Resources.Load<InputActionAsset>("Input/PlayerInputActions");

        moveAction = inputAsset.FindActionMap(actionMapName).FindAction(moveName);
        sprintAction = inputAsset.FindActionMap(actionMapName).FindAction(sprintName);
        lookAction = inputAsset.FindActionMap(actionMapName).FindAction(lookName);
        jumpAction = inputAsset.FindActionMap(actionMapName).FindAction(jumpName);
        fireAction = inputAsset.FindActionMap(actionMapName).FindAction(fireName);
        reloadAction = inputAsset.FindActionMap(actionMapName).FindAction(reloadName);
        escAction = inputAsset.FindActionMap(actionMapName).FindAction(escMenuName);

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

        reloadAction.performed += context => reloadInput = true;
        reloadAction.canceled += context => reloadInput = false;

        escAction.performed += context => onEscPressed?.Invoke();
    }

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        moveAction.Enable();
        sprintAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        fireAction.Enable();
        reloadAction.Enable();
        escAction.Enable();

    }

    private void OnDisable()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        moveAction.Disable();
        sprintAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        fireAction.Disable();
        reloadAction.Disable();
        escAction.Disable();
    }
}
