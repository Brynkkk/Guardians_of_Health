using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGrounded : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform player;

    public SkeletonGrounded(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();
    }
}
