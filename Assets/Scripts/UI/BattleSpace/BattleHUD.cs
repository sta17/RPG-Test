using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.SO_StatsBlock.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.SO_StatsBlock.baseHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
    }

}