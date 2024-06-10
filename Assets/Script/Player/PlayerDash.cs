using UnityEngine;

public class PlayerDash : PlayerState
{
    private int playerLayer;
    private int enemyLayer;

    public PlayerDash(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public override void Enter()
    {
        base.Enter();

        if (!player.UseStamina(player.dashStaminaCost))
        {
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
        player.SetZeroVelocity();

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
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