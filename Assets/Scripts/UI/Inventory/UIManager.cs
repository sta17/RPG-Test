using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerManager player;

    [Header("UI")]
    [SerializeField] private UnitController displayedObject;
    [SerializeField] private UI_DisplayItemsInGridHandler DisplayInventoryItems;
    [SerializeField] private UI_DisplayItemsInGridHandler DisplayInventoryEquiptmentItems;
    //[SerializeField] private UI_Inventory_Hero characterInventoryUI;

    [Header("Icon")]
    [SerializeField] protected GameObject moveable_icon_gameObject;
    
    [Header("Tooltip")]
    [SerializeField] private UI_TooltipPopup tooltipPopup;

    [Header("UI")]
    [SerializeField] protected GameObject Slots_parent_gameObject;

    // Start is called before the first frame update
    void Start()
    {
        //characterInventoryUI.SetTooltipPopup(tooltipPopup);

        DisplayInventoryItems.SetPlayerListener(player);
        DisplayInventoryItems.SetUIManager(this);
        DisplayInventoryItems.Setmoveable_icon_gameObject(moveable_icon_gameObject);
        DisplayInventoryItems.SetTooltipPopup(tooltipPopup);

        DisplayInventoryEquiptmentItems.SetPlayerListener(player);
        DisplayInventoryEquiptmentItems.SetUIManager(this);
        DisplayInventoryEquiptmentItems.Setmoveable_icon_gameObject(moveable_icon_gameObject);
        DisplayInventoryEquiptmentItems.SetTooltipPopup(tooltipPopup);
    }

    #region CharacterInventoryUI

    public bool IsCharacterInventoryWindowActive()
    {
        return Slots_parent_gameObject.activeSelf;
    }

    public void CharacterInventoryWindowSetActive(bool onOroff)
    {
        Slots_parent_gameObject.SetActive(onOroff);
    }

    public void CharacterInventoryWindowUpdateDisplay(UnitController unit)
    {
        DisplayInventoryItems.UpdateDisplay(unit);
        DisplayInventoryEquiptmentItems.UpdateDisplay(unit);
    }

    #endregion

    #region DisplayItemsInGridHandler

    public void ClearSlots()
    {
        DisplayInventoryItems.ClearSlots();
        DisplayInventoryEquiptmentItems.ClearSlots();
    }

    public void DisplayItemsUpdateDisplay(UnitController unit)
    {
        DisplayInventoryItems.UpdateDisplay(unit);
        DisplayInventoryEquiptmentItems.UpdateDisplay(unit);
    }

    #endregion

    #region General

    public UnitController GetdisplayedObject()
    {
        return displayedObject;
    }

    public void UpdateUI(UnitController obj)
    {
        displayedObject = obj;

        DisplayItemsUpdateDisplay(obj);
    }

    public void DisplayHideInventoryWindow()
    {
        if (IsCharacterInventoryWindowActive())
        {
            CharacterInventoryWindowSetActive(false);
        }
        else
        {
            CharacterInventoryWindowUpdateDisplay(displayedObject);
            CharacterInventoryWindowSetActive(true);

        }
    }

    #endregion

}
