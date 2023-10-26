using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;

    public Damageable damageable;
    public Animator animator;
    public NavMeshAgent Agent { get => agent; }
    public Path path;
    public GameObject Player { get => player; }

    [Header("Move Speeds")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fov = 65f;
    public float eyeHeight;

    [Header("Attack Values")]
    [Range(0, 10)]
    public float attackRate;
    [Range(0, 5)]
    public float playerDist;
    public GameObject sword;

    [Header("Bools")]
    public bool seesPlayer;
    public bool canAttack;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        damageable = GetComponentInChildren<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        seesPlayer = CanSeePlayer();
        animator.SetFloat(EnemyAnimations.speed, agent.velocity.magnitude);

        agent.isStopped = animator.GetBool(AnimationStrings.isHit);
        if (!damageable.IsAlive)
        {
            agent.isStopped = true;
        }
    }

    public bool CanSeePlayer()
    {
        if(player != null)
        {
            if(Vector3.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                Vector3 targetDir = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                if(angleToPlayer >= -fov && angleToPlayer <= fov)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDir);                    
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
