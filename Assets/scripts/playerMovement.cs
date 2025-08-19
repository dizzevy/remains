using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public Camera playerCamera;

    [Header("Movement")]
    public float DefaultWalkSpeed = 6f;
    public float DefaultRunSpeed = 12f;
    public float walkSpeed;
    public float runSpeed;

    public float defaultPlayerRotation = 0f;

    [Header("Gravity")]
    public float jumpPower = 7f;

    public float gravity = 10f;

    [Header("Sensi")]
    public float sensitivity = 2f;
    public float lookXLimit = 90f;

    [Header("Сrouch")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;



    //создаётся переменная для хранения направления движения персонажа, и изначально она равна «нет движения».
    private Vector3 moveDirection = Vector3.zero;

    //Переменная для движения камеры, по умолчанию - статично
    private float rotationX = 0;
    private CharacterController characterController;

    //буловая переменная, может ли двигаться?
    public bool canMove = true;


    lifeIndicator lifeInd;


    void Awake()
    {
        lifeInd = GetComponent<lifeIndicator>();
    }

    void Start()
    {
        //Берем у персанажа компанент контроллер
        characterController = GetComponent<CharacterController>();
        //Блочим курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        walkSpeed = DefaultWalkSpeed;
        runSpeed = DefaultRunSpeed;
    }



    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //Создаем векторную переменную которая будет отвечать за передвижение перса
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        //создаем буловую переменную, которой присваевам нажатие на шифт
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && lifeInd.canRun;


        //Создаем переменную которая определяет скорость персонажа в зависимости от его действия. Если canMove, то начинается проверка бега.
        //Если персонаж ждмет shift, значит он isRunning, и берем runSpeed, если нет то walkSpeed, учитывая то, что у нас vertical.
        //Если canMove = false - то ничего не происходит
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0; //вперед назад
        float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0; //влево вправо

        //создаем переменную, которая будет хранить данные передвижения именно по y вектору.
        float movementDirectionY = moveDirection.y;

        //Указывает стороны движения. говорит где перед, где зад.
        //Двигайся вперёд/назад на curSpeedX и вправо/влево на curSpeedY, а потом сложи эти два движения вместе, чтобы получить итоговое направление движения
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        //если жмать на пробел, при условии что можно двигаться и при условии что перс стоит на земле, то присваиваем 3-х мерному значению - jumpPower 
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower; //тут y уже в пространстве, то есть вверх, а не влево-вправо. Задает направление
        }
        else
        {
            moveDirection.y = movementDirectionY; //если задача не выполняется, тогда нихуя не происходит, значение равно своему же значению.
        }
        //Если персонаж не на земле, создаем гравитацию, чтобы его жмала к земле
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime; // то гравитация прижимает перса к земле (гравитация)
        }


        //если жмешь на контрал, и при этом можно двигаться, тогда
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight; //высота персонажа изменятся на высоту присяда
            walkSpeed = crouchSpeed; //скорость меняется на скорость присяда
            runSpeed = crouchSpeed; //скорость бега изменяется на скорость бега во время присяда
        }

        else //иначе
        {
            characterController.height = defaultHeight;//высота становится дефолтная
            walkSpeed = DefaultWalkSpeed; //скорость ходьбю
            runSpeed = DefaultRunSpeed; //скорость бега
        }

        //Команда движения. Код сдвигает персонажа в сторону moveDirection с учётом скорости и времени кадра.
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove) //если может двигаться
        {
            rotationX += -Input.GetAxis("Mouse Y") * sensitivity; // присваиваем переменной движения камеры вверх-вниз значение мышки по y
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); //ограничивает поворт камеры, на определенное кол. градусов.
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); //крутим камеру, которую заранее указали, по X
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0); //поворот самого персонажа во время поворота мыши по горизонтуы

        }

    }
    


}
