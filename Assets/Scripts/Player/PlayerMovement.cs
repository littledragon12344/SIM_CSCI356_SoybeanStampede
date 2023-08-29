using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float gravity = -9.8f;
    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private Animator animator;

    private CharacterController characterController;
    private bool isJumping;
    private bool FPSmovement = false;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isJumping = false;
        moveSpeed = speed;

        if (animator == null)
        {
            Debug.LogError("[ " + GetType() + " ] : Missing Animator reference!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // player's horizontal and vertical movements
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDir = Vector3.zero;
        if (FPSmovement)
        {
            moveDir = transform.forward * verticalInput + transform.right * horizontalInput;

        }
        else
        {
            moveDir = new Vector3(horizontalInput, 0.0f, verticalInput);

        }

        if (animator != null)
        {
            if (moveDir == Vector3.zero) animator.SetBool("Walking", false);
            else animator.SetBool("Walking", true);
        }

        // player's jump movement
        Vector3 jumpForce = Vector3.zero;
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            isJumping = true;
        }
        // limit player's jump height
        if (transform.position.y >= jumpHeight)
        {
            isJumping = false;
        }
        // set the jumpforce
        if (!isJumping) jumpForce.y += gravity;
        else jumpForce.y += -gravity * 1.5f;

        // player's run movement
        // increases speed when player pressed left shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = speed * 1.5f;
        }
        // set speed back to normal when player stop moving
        if (horizontalInput == 0 && verticalInput == 0)
        {
            moveSpeed = speed;
        }

        // pass the movement data to character controller
        characterController.Move((moveDir.normalized * moveSpeed + jumpForce) * Time.deltaTime);
    }

    public void ToggleFPSControls(bool isFPS)
    {
        FPSmovement = isFPS;
    }
}
