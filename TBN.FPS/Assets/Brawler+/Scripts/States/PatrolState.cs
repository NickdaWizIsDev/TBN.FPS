using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;

    public float waitTimer;

    public override void Enter()
    {
    }
    public override void Perform()
    {
        if(!enemy.damageable.IsHit && enemy.damageable.IsAlive)
            PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
       
    }
    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 2f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > .5)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }
                enemy.Agent.speed = enemy.walkSpeed;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0f;
            }            
        }
    }
}
