using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        player.Damage();
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Die method in PlayerStats called.");
        player.Die();

    }
}
