using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Unit/UnitStats")]
public class SO_Unit_Data : ScriptableObject
{
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float unitMaxHealth;
    public bool IsHeroUnit = false;

    public Sprite icon;
    public string unitName;
    public string unitTooltip;

    public string ColouredName
    {
        get
        {
            string hexColour = ColorUtility.ToHtmlStringRGB(Color.black);
            return $"<color=#{hexColour}>{unitName}</color>";
        }
    }

    public string TooltipInfoText
    {
        get
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Description").AppendLine();
            builder.Append(unitTooltip).AppendLine();
            return builder.ToString();
        }
    }
}