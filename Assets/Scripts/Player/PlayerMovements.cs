using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovements : MonoBehaviourPunCallbacks
{
    private CharacterController playerController;

    //Player Movement Values
    private float playerSpeed;
    public float playerMoveSpeed = 100.0f;
    public float playerSprintSpeed = 145.0f;
    public float speedSmoothRate = 100f;
    private Vector3 moveDirection;
    private Vector3 velocity;

    private LayerMask ground;
    public GameObject groundCheckObject;
    public float groudCheckRadius = 0.4f;
    public float jumpHeight = 25f;
    private bool isGrounded;

    private void Start()
    {
        if (photonView.IsMine)
        {
            playerController = GetComponent<CharacterController>();
            ground = LayerMask.GetMask("Ground");
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            JumpAndGravity();
        }
    }

    private void Move()
    {
        float targetSpeed = PlayerInputHandler.Instance.sprintInput ? playerSprintSpeed : playerMoveSpeed;

        if (PlayerInputHandler.Instance.moveInput == Vector2.zero) targetSpeed = 0.0f;

        float currentSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;

        float smoothSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedSmoothRate);

        Vector3 inputDirection = new Vector3(PlayerInputHandler.Instance.moveInput.x, 0f, PlayerInputHandler.Instance.moveInput.y).normalized;

        moveDirection = (transform.right * inputDirection.x + transform.forward * inputDirection.z).normalized;

        playerController.Move(moveDirection * smoothSpeed * Time.deltaTime);
    }

    private void JumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheckObject.transform.position, groudCheckRadius, ground);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded && PlayerInputHandler.Instance.jumpInput)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GlobalGravity.Instance.gravity);
        }

        velocity.y += GlobalGravity.Instance.gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }
}
