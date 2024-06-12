using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    public float detectRange;
    public bool isHeavyAttack;

    #region States
    public SkeletonIdle idleState { get; private set; }
    public SkeletonBattle battleState { get; private set; }
    public SkeletonAttack attackState { get; private set; }
    public SkeletonStunned stunnedState { get; private set; }
    public SkeletonDead deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdle(this, stateMachine, "Idle", this);
        battleState = new SkeletonBattle(this, stateMachine, "Move", this);
        attackState = new SkeletonAttack(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunned(this, stateMachine, "Stunned", this);

        deadState = new SkeletonDead(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        SetZeroVelocity();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
        {
            stateMachine.ChangeState(stunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
