using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // increase damage + 1
    public Stat agility; // increase crit + 1
    public Stat intelligence; // increase magic? + 1
    public Stat vitality; // increase hp + 5

    [Header("Defensive Stats")]
    public Stat maxHp;
    public Stat armor;

    public Stat damage;
    public Stat maxStamina;

    public int currHp;
    public int currStamina;
    public int totalDamage;

    public System.Action onHpChange;
    public System.Action onStaminaChange;

    protected virtual void Start()
    {
        currHp = GetMaxHpValue();
        currStamina = GetMaxStamina();

        if (onHpChange != null)
        {
            onHpChange();
        }
        if (onStaminaChange != null)
        {
            onStaminaChange();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        DecreaseHp(damage);

        if (currHp <= 0)
        {
            Debug.Log("Character died.");
            Die();
        }
    }

    protected virtual void DecreaseHp(int damage)
    {
        currHp -= damage;

        if (onHpChange != null)
        {
            onHpChange();
        }
    }

    public virtual void UseStamina(int amount)
    {
        currStamina -= amount;

        if (currStamina < 0)
        {
            currStamina = 0;
        }

        if (onStaminaChange != null)
        {
            onStaminaChange();
        }
    }

    public virtual void RecoverStamina(int amount)
    {
        currStamina += amount;
        if (currStamina > maxStamina.GetValue())
        {
            currStamina = maxStamina.GetValue();
        }

        if (onStaminaChange != null)
        {
            onStaminaChange();
        }
    }

    public virtual void DoDamage(CharacterStats targetStats, bool isHeavyAttack)
    {
        if (isHeavyAttack)
        {
            totalDamage = damage.GetValue() + strength.GetValue() * 2;
            Debug.Log("Is Heavy Attack");
        }
        else
        {
            totalDamage = damage.GetValue() + strength.GetValue();
        }

        totalDamage -= targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        targetStats.TakeDamage(totalDamage);
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
    }

    public int GetMaxHpValue()
    {
        return maxHp.GetValue() + vitality.GetValue() * 5;
    }

    public int GetMaxStamina()
    {
        return maxStamina.GetValue();
    }
}
