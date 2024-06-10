using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattle : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;

    public SkeletonBattle(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (Vector2.Distance(player.position, enemy.transform.position) > enemy.detectionRadius)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (Vector2.Distance(player.position, enemy.transform.position) < enemy.attackDistance && Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - enemy.transform.position).normalized;
        enemy.SetVelocity(direction.x * enemy.moveSpeed, direction.y * enemy.moveSpeed);
    }
}
