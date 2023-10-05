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
    public GameObject sword;
    public GameObject gun;
    public GameObject rifle;
    private CharacterController controller;
    private Animator animator;
    private Damageable damageable;

    public AudioSource audioSource;
    public AudioClip land;

    [SerializeField]Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    public float runSpeed = 5f;
    public float jumpForce = 2f;
    public float fallSpeed = -10f;
    public float gravity = -9.81f;

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
        velocity.z = controller.velocity.z;

        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        // Gravity application
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = fallSpeed;
        }
        else if (!isGrounded)
        {
            velocity.y = Mathf.Lerp(velocity.y, gravity, 1.5f * Time.deltaTime);
        }


        animator.SetFloat(AnimationStrings.yVelocity, velocity.y);
        animator.SetBool(AnimationStrings.isGrounded, isGrounded);

        if (!damageable.IsAlive)
        {
            Time.timeScale = 0f;
            gameOver.gameObject.SetActive(true);
            music.Stop();
        }

        // Apply gravity
        controller.Move(velocity * Time.deltaTime);

        UpdateMouseAim();
    }

    private void FixedUpdate()
    {
        // Calculate movement direction based on camera rotation
        Vector3 moveDirection = (cam.forward * moveInput.y + cam.right * moveInput.x).normalized;

        // Apply the movement
        Vector3 moveVelocity = moveDirection * CurrentMoveSpeed;
        moveVelocity.y = velocity.y;
        controller.Move(moveVelocity * Time.deltaTime);
    }

    public void UpdateMouseAim()
    {
        Vector2 mouseDelta = new (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);

        camPitch -= mouseDelta.y * sensitivity;
        camPitch = Mathf.Clamp(camPitch, -90f, 90f);

        cam.localEulerAngles = Vector3.right * camPitch;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
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

        else if (context.canceled)
        {
            velocity.y *= .5f;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.started && sword.activeSelf == true)
        {
            animator.SetTrigger(AnimationStrings.atk);
        }
    }

    public void Land()
    {
        audioSource.PlayOneShot(land, 0.15f);
    }
}
