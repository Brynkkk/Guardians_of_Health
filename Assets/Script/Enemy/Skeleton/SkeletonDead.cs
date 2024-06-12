using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDead : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonDead(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Destroy(enemy.gameObject);
    }

    public override void Enter()
    {
        AudioManager.instance.PlaySFX(5, null);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
    }
}
