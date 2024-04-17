using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float gravity = 9.81f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;

    CharacterController characterController;
    float rotationX = 0;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float verticalVelocity = 0.0f;

    private float timer = 0.0f;
    private float bobbingSpeedMultiplier = 1.0f;
    private float midpoint = 1.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        getInput();
        playerMove();
        cameraLook();
        applyHeadbob();
    }

    private void getInput()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
        inputVector = transform.TransformDirection(inputVector);
        float movementDirectionY = inputVector.y;

        movementVector = (inputVector * walkingSpeed);

        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        movementVector.y = verticalVelocity;
    }

    private void playerMove()
    {
        characterController.Move(movementVector * Time.deltaTime);
    }

    private void cameraLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void applyHeadbob()
    {
        if (Mathf.Abs(inputVector.x) > 0.1f || Mathf.Abs(inputVector.z) > 0.1f)
        {
            float waveSlice = 0.0f;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveSlice = Mathf.Sin(timer);
                timer += bobbingSpeed * bobbingSpeedMultiplier;
                if (timer > Mathf.PI * 2)
                {
                    timer -= Mathf.PI * 2;
                }
            }

            if (waveSlice != 0)
            {
                float translateChange = waveSlice * bobbingAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                Vector3 newPosition = playerCamera.transform.localPosition;
                newPosition.y = midpoint + translateChange;
                playerCamera.transform.localPosition = newPosition;
            }
            else
            {
                Vector3 newPosition = playerCamera.transform.localPosition;
                newPosition.y = midpoint;
                playerCamera.transform.localPosition = newPosition;
            }
        }
    }
}
