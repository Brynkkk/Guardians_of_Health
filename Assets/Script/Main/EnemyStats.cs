using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private EnemySkeleton es;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
        es = GetComponent<EnemySkeleton>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        enemy.Damage();
    }

    protected override void Die()
    {
        base.Die();

        es.stateMachine.ChangeState(es.deadState);
    }
}
