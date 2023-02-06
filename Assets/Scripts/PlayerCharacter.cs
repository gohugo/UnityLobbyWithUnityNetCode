using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class PlayerCharacter : NetworkBehaviour
{
    // The transform component for this GameObject
    private Transform m_Transform;
    [SerializeField]
    private float walkSpeed =5f;
    [SerializeField]
    private float runSpeed =8f;
    // The player's movement speed
    private float m_MovementSpeed = 10.0f;    
    // The player's movement speed
    public float rotationSpeed = 100.0f;

    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        // Cache the transform component for easy access
        m_Transform = this.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Debug.Log("OwnerClientId : " + OwnerClientId);


        StartCoroutine(Attack());

    }

    private void FixedUpdate()
    {
        if (!IsOwner /*|| !IsLocalPlayer*/) return;

        // Get input from the horizontal and vertical axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");



        // Calculate the movement vector based on the input and movement speed
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        //movementDirection = m_Transform.TransformDirection(movementDirection);
        Vector3 movement = movementDirection * m_MovementSpeed * Time.deltaTime;

        if (movementDirection == Vector3.zero)
        {
            Idle();
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else
            {
                Walk();
            }
        }
        //characterController.Move(movement);








        //// Move the player by the calculated amount
        m_Transform.position += movement;

        //handle rotation

        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            m_Transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

       

        //HandleAnimation();
    }
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk() {
        m_MovementSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run() {
        m_MovementSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }
    private IEnumerator Attack() {
        while (true)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(0.9f);
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
            yield return new WaitForSeconds(1f);
        }
    }

    private void HandleAnimation() {

        // Check if the player is moving
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("IsIdle", false);
            // Check if the player is pressing the run button
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // If the player is pressing the run button, set the "IsRunning" boolean to true in the Animator
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false);
            }
            else
            {
                // If the player is not pressing the run button, set the "IsWalking" boolean to true in the Animator
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            // If the player is not moving, set the "IsIdle" boolean to true in the Animator
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }
}

