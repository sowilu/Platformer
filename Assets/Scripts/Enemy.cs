using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badbro : MonoBehaviour
{
    public Health health;

    [Header("Movement")]
    public float speed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float attackCooldownTime = 0f;

    [Header("Patrolling")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackDelay = 0.5f;

    [Header("Sound")]
    public AudioClip attackSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Transform badbro;  // Changed from 'player' to 'badbro'
    private bool isAttacking = false;
    private bool badbroInRange = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        badbro = GameObject.FindGameObjectWithTag("Badbro")?.transform;  // Safe null-check for badbro

        if (badbro == null)
        {
            Debug.LogError("Badbro not found in the scene! Ensure it has the 'Badbro' tag.");
        }
    }

    void Update()
    {
        if (badbro == null || health == null) return;

        float distanceToBadbro = Vector2.Distance(transform.position, badbro.position);

        // Check if Badbro is in range
        badbroInRange = distanceToBadbro <= detectionRange;

        if (badbroInRange && distanceToBadbro > attackRange)
        {
            // Move towards Badbro
            isPatrolling = false;
            FollowBadbro();
        }
        else if (badbroInRange && distanceToBadbro <= attackRange)
        {
            // Attack Badbro
            isPatrolling = false;
            AttackBadbro();
        }
        else
        {
            // Patrol between points
            isPatrolling = true;
            Patrol();
        }
    }

    void FixedUpdate()
    {
        if (isPatrolling && patrolPoints.Length > 0)
        {
            PatrolMovement();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Move towards the current patrol point
        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPatrolPoint.position - transform.position).normalized;

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // If we are close to the patrol point, move to the next one
        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void FollowBadbro()
    {
        Vector2 direction = (badbro.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    void AttackBadbro()
    {
        if (attackCooldownTime <= 0f && !isAttacking)
        {
            isAttacking = true;
            audioSource.PlayOneShot(attackSound);

            // Null check for Badbro and Health
            Badbro badbroScript = badbro.GetComponent<Badbro>();
            if (badbroScript != null && badbroScript.health != null)
            {
                badbroScript.health.TakeDamage(attackDamage);  // Safe damage application
            }
            else
            {
                Debug.LogError("Badbro or Badbro's Health component is missing!");
            }

            attackCooldownTime = attackCooldown;

            // Reset attacking flag after a short delay to allow attack animation to finish
            StartCoroutine(ResetAttackFlag());
        }
    }
    void PatrolMovement()
    {
        if (patrolPoints.Length == 0) return;

        
        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];

        
        Vector2 direction = (targetPatrolPoint.position - transform.position).normalized;

        // Set the velocity for patrolling
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // If we are close enough to the patrol point, switch to the next patrol point
        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }


    // Attack cooldown timer
    void UpdateCooldown()
    {
        if (attackCooldownTime > 0f)
        {
            attackCooldownTime -= Time.deltaTime;
        }
    }

    IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    // Debugging gizmos for detection range and patrol points
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}
