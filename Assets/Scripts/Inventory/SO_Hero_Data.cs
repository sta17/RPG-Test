using UnityEngine;

[CreateAssetMenu(menuName = "Unit/HeroStats")]
public class SO_Hero_Data : SO_Unit_Data
{
    public int heroExperience = 0;
    public int heroLevel = 1;
    public string heroTitle;

    public SO_Hero_Data()
    {
        IsHeroUnit = true;
    }
}
