using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Canvas gameOver;
    public AudioSource music;
    public Transform cam;
    private CharacterController controller;
    [SerializeField] private Vector3 velocity;
    private Animator animator;
    private Damageable damageable;
    private TouchingDirections touching;

    public AudioSource audioSource;
    public AudioClip land;

    Vector2 moveInput;
    public float runSpeed = 5f;
    public float jumpForce = 2f;
    public float fallSpeed = -10f;
    public float gravity = -9.81f;
    private Vector2 currentDirVel;

    public float sensitivity = 3.5f;
    public float camPitch = 0.0f;
    public bool lockCursor = true;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    return runSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        set
        {

        }
    }
    private bool isMoving;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        velocity.x = controller.velocity.x;

        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        // Gravity application
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }
        else if (!isGrounded)
        {
            velocity.y = Mathf.Lerp(velocity.y, fallSpeed, 1.5f * Time.deltaTime);
        }


        animator.SetFloat(AnimationStrings.yVelocity, controller.velocity.y);
        animator.SetBool(AnimationStrings.isGrounded, isGrounded);

        if (!damageable.IsAlive)
        {
            Time.timeScale = 0f;
            gameOver.gameObject.SetActive(true);
            music.Stop();
        }

        UpdateMouseAim();
        UpdateMovement();
    }

    private void FixedUpdate()
    {

    }

    public void UpdateMouseAim()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);

        camPitch -= mouseDelta.y * sensitivity;
        camPitch = Mathf.Clamp(camPitch, -90f, 90f);

        cam.localEulerAngles = Vector3.right * camPitch;
    }

    public void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        moveInput = Vector2.SmoothDamp(moveInput, targetDir, ref currentDirVel, .35f);

        Vector3 velocity = (transform.forward * moveInput.y + transform.right * moveInput.x) * CurrentMoveSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(-2f * jumpForce * gravity);
                animator.SetTrigger(AnimationStrings.jump);
                Debug.Log("Jump!");
            }
        }
    }

    public void Land()
    {
        audioSource.PlayOneShot(land, 0.15f);
    }
}
