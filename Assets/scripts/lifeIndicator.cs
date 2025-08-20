using TMPro;
using System;
using UnityEngine;

public class lifeIndicator : MonoBehaviour
{

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI hungryText;
    public TextMeshProUGUI thirstyText;

    [Header("LIFE INDICATORS")]
//HEALTH
    public float overallHealthCount;
    public float maxHealth = 1000f;
//HUNGER
    public float Hunger = 1000f;
    public float MaxHungerCount = 1000f;
    public float StarvationRatePerSecond;
    public float fullStarvationTime = 3600f;
//THIRSTY
    public float thirsty = 1000f;
    public float MaxThirstyCount = 1000f;
    public float ThirstyRatePerSecond;
    public float fullThirstyTime = 3600f;
//ILLNESSES
    public float bleeding = 0f;
    public float pain = 0f;
    public float depresion = 0f;

    [Header("BODY PARTS")]
    //Head
    //Lower leg left

    [Header("TYPES OF DAMAGE")]
    public bool brokenBone = false;



    [Header("STAMINA")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float minStamina = 30f;
    public float decreaseRunDuration = 20f;
    public float increaseRunDuration = 40f;

    public float decreaseJumpStamina = 10f;

    public bool canRun = true;

    [Header("Player")]

    private playerMovement playerMove;
    private CharacterController characterController;

    private bool wasGroundedLastFrame;

    void Start()
    {
        overallHealthCount = maxHealth;
        StarvationRatePerSecond = Hunger / fullStarvationTime;
        ThirstyRatePerSecond = thirsty / fullThirstyTime;
    }


    void Awake()
    {
        playerMove = GetComponent<playerMovement>();
        characterController = GetComponent<CharacterController>();

    }



    void Update()
    {
        runStamina();
        RefreshJumpFromStamina();
        wasGroundedLastFrame = characterController.isGrounded;
        Debug.Log("Stamina: " + stamina);
        UI();
        Starving();
        Thirsting();
    }


    void runStamina()
    {
        if (stamina <= 0)
        {
            stamina = 0;
            playerMove.canJump = false;
            canRun = false;
        }
        else if (!canRun && !playerMove.canJump && stamina >= minStamina)
        {
            canRun = true;
            playerMove.canJump = true;
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

    public void jumpStamina()
    {
        stamina -= decreaseJumpStamina;
    }

    void RefreshJumpFromStamina()
{
    if (stamina <= 0f)
    {
        // блокируем прыжок, если выжгли стамину в ноль
        playerMove.canJump = false;
        return;
    }

    // если прыжок сейчас заблокирован И стамины уже ≥ 30 И нет кулдауна — включаем обратно
    if (!playerMove.canJump && stamina >= minStamina && !playerMove.IsJumpCooldownActive)
    {
        playerMove.canJump = true;
    }
}



    //bodyPartsStatus
    void bodyPartStatus_Stable()
    {

    }


    public void TakeDamage(float damage)
    {
        overallHealthCount -= damage;
        overallHealthCount = Mathf.Clamp(overallHealthCount, 0, maxHealth);
    }

    void UI()
    {
        healthText.text = "health: " + ((int)overallHealthCount).ToString();
        staminaText.text = "stamina: " + ((int)stamina).ToString();
        hungryText.text = "hungry: " + ((int)Hunger).ToString();
        thirstyText.text = "thirsty: " + ((int)thirsty).ToString();
    }

    void Starving() //процесс голодания
    {
        if (Hunger > 0)
        {
            Hunger -= StarvationRatePerSecond * Time.deltaTime;
            if (Hunger < 0) Hunger = 0; // защита от ухода в минус
        }

    }

    void Thirsting() //процесс жажды
    {
        if (thirsty > 0)
        {
            thirsty -= ThirstyRatePerSecond * Time.deltaTime;
            if (thirsty < 0) thirsty = 0; // защита от ухода в минус
        }

    }


    





}
