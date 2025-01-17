using UnityEngine;
using System;

[Serializable]
public class PlayerManager : MonoBehaviour
{
    #region variables, Setup and Constructors

    [Header("Selection")]
    [SerializeField] private UnitController selectedUnit;

    [Header("Inventory")]
    [SerializeField] private UI_PersonalInventorySystem personalInventory;
    [SerializeField] private int inventorySize = 5;

    [Header("UI")]
    [SerializeField] private UIManager UIManager;
    [SerializeField] private GameObject general_UI;

    [Header("BattleSystem")]
    [SerializeField] private BattleSystem BattleSystem;
    [SerializeField] private GameObject BattleSystem_GO;

    private void Start()
    {
        personalInventory = new UI_PersonalInventorySystem(inventorySize);
        UIManager.UpdateUI(selectedUnit);
    }

    #endregion

    #region Mouse and Keyboard Actions

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            IKeyButtonDown();

        }
    }

    private void IKeyButtonDown()
    {
        UIManager.DisplayHideInventoryWindow();
    }

    #endregion

    #region Misc

    public void RaiseItemChangeNotification(UnitController unit)
    {
        if (unit == UIManager.GetdisplayedObject())
        {
            UIManager.UpdateUI(unit);
        }
    }

    public UI_PersonalInventorySystem GetPersonalInventorySystem()
    {
        return personalInventory;
    }

    #endregion

    #region Add Items

    public bool InteractWithItem(OnPickup itemHandler)
    {
        var result = false;
        if (itemHandler.getType() == ItemTypes.Pickup)
        {
            result = itemHandler.HandlePickupType(selectedUnit);
        }
        else
        {
            result = AddItem(itemHandler.getItem());
        }
        return result;
    }

    public bool AddItem(INGAME_Item_Data item)
    {
        var result = personalInventory.Add(item);

        if (result)
        {
            RaiseItemChangeNotification(selectedUnit);
        }

        return result;
    }

    #endregion

    #region Battlesystem
        
   public void StartBattle()
    {
        BattleSystem_GO.SetActive(true);
        general_UI.SetActive(false);
        //set camera
        BattleSystem.StartBattle();
    }

    public void EndBattle()
    {
        //set camera to original position.
        general_UI.SetActive(true);
        BattleSystem_GO.SetActive(false);
    }

    #endregion

}
