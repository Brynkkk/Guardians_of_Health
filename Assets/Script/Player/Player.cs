using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    public bool isHeavyAttack;
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

    private Vector3 respawnPoint;
    public int potionCount;
    public const int maxPotions = 3;
    private bool isAtCheckpoint;
    private PlayerStats ps;

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

        respawnPoint = transform.position; // Save starting position as respawn point
        potionCount = maxPotions; // Start with full potions
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

        // Check for checkpoint activation
        if (isAtCheckpoint && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed. Setting respawn point.");
            AudioManager.instance.PlaySFX(7, null);

            SetRespawnPoint();
        }

        // Check for potion use
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySFX(10, null);
            UsePotion();
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
        Debug.Log("Die method in PlayerStats called.");
        stateMachine.ChangeState(deadState);

        // Start respawn process
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before respawning
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPoint;
        playerStats.currHp = playerStats.GetMaxHpValue();
        playerStats.currStamina = playerStats.GetMaxStamina();

        stateMachine.ChangeState(idleState); // Reset to idle state
        Debug.Log("Player respawned. State set to idle.");

        if (playerStats.onHpChange != null)
        {
            playerStats.onHpChange();
        }

        if (playerStats.onStaminaChange != null)
        {
            playerStats.onStaminaChange();
        }
    }

    private void SetRespawnPoint()
    {
        respawnPoint = transform.position;
        potionCount = maxPotions; // Refill potions at checkpoint
        HealPlayer(playerStats.GetMaxHpValue() - playerStats.currHp); // Heal to full health
        Debug.Log("Respawn point set to: " + respawnPoint + " and potions refilled.");
    }

    private void UsePotion()
    {
        if (potionCount > 0)
        {
            potionCount--;
            HealPlayer(0.3f * playerStats.GetMaxHpValue());
            Debug.Log("Potion used. Remaining potions: " + potionCount);
        }
        else
        {
            Debug.Log("No potions left.");
        }
    }

    private void HealPlayer(float healAmount)
    {
        playerStats.currHp += (int)healAmount;
        if (playerStats.currHp > playerStats.GetMaxHpValue())
        {
            playerStats.currHp = playerStats.GetMaxHpValue();
        }

        if (playerStats.onHpChange != null)
        {
            playerStats.onHpChange();
        }
        Debug.Log("Player healed by " + healAmount + ". Current HP: " + playerStats.currHp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            isAtCheckpoint = true;
            Debug.Log("Player at checkpoint. Press 'F' to set respawn point.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            isAtCheckpoint = false;
            Debug.Log("Player exited checkpoint.");
        }
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
        while (playerStats.currStamina < playerStats.GetMaxStamina())
        {
            playerStats.RecoverStamina(staminaRegenValue);
            yield return new WaitForSeconds(staminaRegenSpeed);
        }
        staminaRegenCoroutine = null;
    }
}