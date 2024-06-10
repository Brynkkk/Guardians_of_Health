using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    public PlayerStats playerStats { get; private set; }

    [Header("Move Info")]
    public float moveSpeed;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashStaminaCost = 20f;
    public Vector2 dashDirection { get; private set; }

    [Header("Sprint Info")]
    public float sprintStaminaCostPerSecond = 1f;

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float primaryAttackStaminaCost = 10f;
    public float heavyAttackStaminaCost = 20f;
    public float counterAttackStaminaCost = 20f;
    public float counterAttackDuration = 0.2f;
    private float counterAttackCooldown = 2.0f;
    private float cooldownTimer = 0f;

    [Header("Stamina Regen Info")]
    public float staminaRegenSpeed = 0.1f;
    public int staminaRegenValue = 1;
    [SerializeField] private float staminaRegenDelay = 3f;

    private Coroutine staminaRegenCoroutine;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdle idleState { get; private set; }
    public PlayerMove moveState { get; private set; }
    public PlayerDash dashState { get; private set; }
    public PlayerUpDash upDashState { get; private set; }
    public PlayerDownDash downDashState { get; private set; }
    public PlayerSprint sprintState { get; private set; }

    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerHeavyAttack heavyAttack { get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }

    public PlayerDead deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        playerStats = GetComponent<PlayerStats>();

        idleState = new PlayerIdle(this, stateMachine, "Idle");
        moveState = new PlayerMove(this, stateMachine, "Move");
        sprintState = new PlayerSprint(this, stateMachine, "Move");

        dashState = new PlayerDash(this, stateMachine, "Dash");
        upDashState = new PlayerUpDash(this, stateMachine, "UpDash");
        downDashState = new PlayerDownDash(this, stateMachine, "DownDash");

        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        heavyAttack = new PlayerHeavyAttack(this, stateMachine, "Heavy");
        counterAttack = new PlayerCounterAttack(this, stateMachine, "CounterAttack");

        deadState = new PlayerDead(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currState.Update();
        CheckDashInput();

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (!isBusy && !IsPerformingStaminaConsumingAction() && staminaRegenCoroutine == null)
        {
            staminaRegenCoroutine = StartCoroutine(DelayStaminaRegen());
        }
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.W) && playerStats.currStamina >= dashStaminaCost)
        {
            UseStamina(dashStaminaCost);
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            dashDirection = new Vector2(xInput, yInput).normalized;

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(facingDirection, 0);
            }

            stateMachine.ChangeState(upDashState);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) && playerStats.currStamina >= dashStaminaCost)
        {
            UseStamina(dashStaminaCost);
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            dashDirection = new Vector2(xInput, yInput).normalized;

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(facingDirection, 0);
            }

            stateMachine.ChangeState(downDashState);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && playerStats.currStamina >= dashStaminaCost)
        {
            UseStamina(dashStaminaCost);
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            dashDirection = new Vector2(xInput, yInput).normalized;

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(facingDirection, 0);
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void StartCounterAttackCooldown()
    {
        cooldownTimer = counterAttackCooldown;
    }

    public bool IsCounterAttackOnCooldown()
    {
        return cooldownTimer > 0;
    }

    public bool IsPerformingStaminaConsumingAction()
    {
        return stateMachine.currState is PlayerDash || stateMachine.currState is PlayerSprint ||
               stateMachine.currState is PlayerPrimaryAttack || stateMachine.currState is PlayerHeavyAttack ||
               stateMachine.currState is PlayerCounterAttack;
    }

    public bool UseStamina(float amount)
    {
        if (playerStats.currStamina >= amount)
        {
            playerStats.UseStamina((int)amount);
            if (staminaRegenCoroutine != null)
            {
                StopCoroutine(staminaRegenCoroutine);
                staminaRegenCoroutine = null;
            }
            return true;
        }
        return false;
    }

    private IEnumerator DelayStaminaRegen()
    {
        yield return new WaitForSeconds(staminaRegenDelay);
        staminaRegenCoroutine = StartCoroutine(RegenerateStamina());
    }

    private IEnumerator RegenerateStamina()
    {
        while (playerStats.currStamina < playerStats.maxStamina.GetValue())
        {
            playerStats.RecoverStamina(staminaRegenValue);
            yield return new WaitForSeconds(staminaRegenSpeed);
        }
        staminaRegenCoroutine = null;
    }
}
