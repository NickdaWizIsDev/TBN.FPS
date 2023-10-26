using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float loosePlayerTimer;
    private float attackTimer;
    private float timer;

    public float maxTime = 0.5f;

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer() && !enemy.damageable.IsHit && enemy.damageable.IsAlive) // Player located.
        {
            // Lock the timer for losing the player's position, and start timers.
            loosePlayerTimer = 0f;
            attackTimer += Time.deltaTime;
            timer -= Time.deltaTime;
            if (attackTimer >= enemy.attackRate)
                enemy.canAttack = true;
            if (timer < 0f)
            {
                float distance = (enemy.transform.position - enemy.Player.transform.position).magnitude;
                if (distance >= enemy.playerDist)
                {
                    Vector3 directionToPlayer = enemy.Player.transform.position - enemy.transform.position - (Vector3.up * enemy.eyeHeight);
                    directionToPlayer.Normalize(); // Normalize the direction vector
                    Vector3 targetPos = enemy.Player.transform.position - (directionToPlayer * .5f);
                    enemy.Agent.speed = enemy.runSpeed;
                    enemy.Agent.SetDestination(targetPos);
                }

                if (enemy.canAttack && distance <= 3)
                {
                    Attack();
                }
                timer = maxTime;
            }
        }
        else
        {
            loosePlayerTimer += Time.deltaTime;
            if(loosePlayerTimer >= 1f)
            {
                //Go back to patrol state
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public void Attack()
    {
        enemy.animator.SetTrigger(EnemyAnimations.atk);
        Debug.Log(enemy.gameObject.name + " has attacked!");
        attackTimer = 0f;
        enemy.canAttack = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
