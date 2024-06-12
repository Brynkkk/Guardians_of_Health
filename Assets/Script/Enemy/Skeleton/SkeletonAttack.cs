using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : EnemyState
{
    private EnemySkeleton enemy;

    public SkeletonAttack(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        // AudioManager.instance.PlaySFX(8, null);
        enemy.isHeavyAttack = DecideAttackType(); // Decide the attack type

        // enemy.anim.SetBool("isHeavyAttack", enemy.isHeavyAttack); 
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    private bool DecideAttackType()
    {
        return Random.value > 100f; // 0% chance to perform a heavy attack
    }
}
