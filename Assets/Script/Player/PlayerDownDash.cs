using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownDash : PlayerState
{
    private int playerLayer;
    private int enemyLayer;

    public PlayerDownDash(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(11, null);

        if (!player.UseStamina(player.dashStaminaCost))
        {
            AudioManager.instance.StopSFX(11);
            stateMachine.ChangeState(player.idleState);
            return;
        }

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        player.SetVelocity(player.dashDirection.x * player.dashSpeed, player.dashDirection.y * player.dashSpeed);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}

