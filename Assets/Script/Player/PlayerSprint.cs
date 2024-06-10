using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : PlayerGrounded
{
    public PlayerSprint(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (player.playerStats.currStamina < player.sprintStaminaCostPerSecond)
        {
            stateMachine.ChangeState(player.moveState);
        }
        else
        {
            player.SetVelocity(xInput * player.moveSpeed * 2f, yInput * player.moveSpeed * 2f);
            player.UseStamina(player.sprintStaminaCostPerSecond * Time.deltaTime);

            if (xInput == 0 && yInput == 0)
            {
                stateMachine.ChangeState(player.idleState);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(player.moveState);
            }
        }

        if (!player.UseStamina(player.sprintStaminaCostPerSecond))
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
