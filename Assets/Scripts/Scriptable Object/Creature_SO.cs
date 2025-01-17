using UnityEngine;

[CreateAssetMenu(menuName = "Creatures and People/Creature Stat Block")]
public class Creature_SO : ScriptableObject
{
    public bool isCatchable;

    public float baseDamage;
    public float baseDefense;
    public float baseSpecialDefense;
    public float baseSpecialAttack;
    public float baseHP;
    public float baseSpeed;

    public ElementType typing;

    public Sprite icon;
    public string unitName;
    public string unitDescription;
}
