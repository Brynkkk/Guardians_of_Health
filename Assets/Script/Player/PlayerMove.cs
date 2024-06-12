using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerGrounded
{
    public PlayerMove(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(6, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(6);
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, yInput * player.moveSpeed);

        if (xInput == 0 && yInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.sprintState);
        }
    }
}