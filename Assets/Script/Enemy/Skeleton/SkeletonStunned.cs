using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunned : EnemyState
{
    private EnemySkeleton enemy;
    private const int ArmorReduction = 20;

    public SkeletonStunned(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunnedDuration;

        rb.velocity = new Vector2(-enemy.facingDirection * enemy.stunnedDirection.x, enemy.stunnedDirection.y);

        enemy.stats.armor.AddModifier(-ArmorReduction);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedBlink", 0);

        enemy.stats.armor.RemoveModifier(-ArmorReduction);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
