using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int health = 100;

    public Transform Player;
    public float detectionRange = 10f;
    public float attackDistance = 10f;
    public float attackInterval = 1f;
    public float attackDamage = 50f; 

    NavMeshAgent Agent;
    Animator anim;
    bool isDead = false;
    bool isAttacking = false;
    private lifeIndicator playerLife;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
          if (Player != null)
        {
            playerLife = Player.GetComponent<lifeIndicator>();
        }
    }

    private void Update()
    {
        if (isDead) return;

        float Distance = Vector3.Distance(transform.position, Player.position);

        if (Distance <= detectionRange)
        {
            Agent.SetDestination(Player.position);
            anim.SetBool("isWalking", true);

            if (Distance <= attackDistance && !isAttacking)
            {
                StartCoroutine(PlayAttackAnimation());
            }
        }
        else
        {
            Agent.ResetPath();
            anim.SetBool("isWalking", false);
        }
    }

     IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        Agent.isStopped = true;
        anim.SetTrigger("Attack");

        // ждём половину интервала, имитация задержки удара
        yield return new WaitForSeconds(attackInterval / 2f);

        // проверяем — игрок всё ещё в зоне атаки?
        if (Vector3.Distance(transform.position, Player.position) <= attackDistance)
        {
            if (playerLife != null)
            {
                playerLife.TakeDamage(attackDamage);
                Debug.Log("Зомби нанёс урон: " + attackDamage);
            }
        }

        // ждём остаток перед следующей атакой
        yield return new WaitForSeconds(attackInterval / 2f);

        Agent.isStopped = false;
        isAttacking = false;
    }
}