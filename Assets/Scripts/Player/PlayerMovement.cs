using System.Collections;
using System.Collections.Generic;
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

    private CharacterController characterController;
    private bool isJumping;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isJumping = false;
        moveSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        // player's horizontal and vertical movements
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Vector3 moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 moveDir = new Vector3(horizontalInput, 0.0f, verticalInput);

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
}
