using UnityEngine;

public class Unit : MonoBehaviour
{
    public Creature_SO SO_StatsBlock;

    public int unitLevel;

    public float currentHP;

    public bool isAlive;

    public Color unitColor;

    public bool TakeDamage(float dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
        }
        else
            isAlive = true;

        return isAlive;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > SO_StatsBlock.baseHP)
            currentHP = SO_StatsBlock.baseHP;
    }

}
