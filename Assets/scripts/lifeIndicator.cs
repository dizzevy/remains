using System;
using UnityEngine;

public class lifeIndicator : MonoBehaviour
{
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float decreaseRunDuration = 20f;

    public float increaseRunDuration = 40f;

    public float decreaseJumpStamina = 10f;

    public bool canRun = true;
    public bool canJump = true;

    private playerMovement playerMove;
    private CharacterController characterController;

    private bool wasGroundedLastFrame;


    void Awake()
    {
        playerMove = GetComponent<playerMovement>();
        characterController = GetComponent<CharacterController>();

    }



    void Update()
    {
        runStamina();
        jumpStamina();
        wasGroundedLastFrame = characterController.isGrounded;
        Debug.Log("Stamina: " + stamina);
    }


    void runStamina()
    {
        if (stamina <= 0)
        {
            stamina = 0;
            canRun = false;
        }
        else if (!canRun && stamina >= 30)
        {
            canRun = true;
        }




        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && canRun)
        {
            float decreasePerSecond = maxStamina / decreaseRunDuration;
            stamina -= decreasePerSecond * Time.deltaTime;

            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
        else
        {
            float increasePerSecond = maxStamina / increaseRunDuration;
            stamina += increasePerSecond * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
    }

    void jumpStamina()
    {
         if (Input.GetButtonDown("Jump") && playerMove.canMove && wasGroundedLastFrame)
         {
            stamina -= decreaseJumpStamina;
         }
    }




}
