using System.Collections;
using UnityEngine;

public class PlayerHeavyAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2f;

    private PlayerAnimationTriggers pat;
    private Player pp;

    public PlayerHeavyAttack(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        this.pp = _player; // Ensure the Player instance is assigned
        this.pat = _player.GetComponentInChildren<PlayerAnimationTriggers>(); // Assign the PlayerAnimationTriggers instance
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Heavy Attack State");

        AudioManager.instance.PlaySFX(1, null);

        if (!player.UseStamina(player.heavyAttackStaminaCost))
        {
            AudioManager.instance.StopSFX(1);
            stateMachine.ChangeState(player.idleState);
            return;
        }

        xInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;

        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        pp.isHeavyAttack = DecideAttackType();
        Debug.Log($"Heavy Attack Type: {(pp.isHeavyAttack ? "Heavy" : "Normal")}");

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private bool DecideAttackType()
    {
        return Random.value > 0f; // 50% chance to perform a heavy attack
    }
}
