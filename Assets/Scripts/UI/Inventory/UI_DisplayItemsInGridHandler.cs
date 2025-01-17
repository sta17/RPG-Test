using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_DisplayItemsInGridHandler : MonoBehaviour, I_UI_IconItemListener
{
    [SerializeField] protected List<UI_Slot_Item> itemSlots;
    [SerializeField] protected UnitController displayedUnit;
    [SerializeField] protected UI_PersonalInventorySystem personalInventorySystem;
    [SerializeField] protected UI_TooltipPopup tooltipPopup;

    public PlayerManager player;

    [SerializeField] private UIManager UIManager;

    [SerializeField] protected Sprite disabledIcon;

    [SerializeField] protected bool isEquiptment;

    [Header("Icon")]
    [SerializeField] protected GameObject moveable_icon_gameObject;

    [Header("UI")]
    [SerializeField] protected GameObject Slots_parent_gameObject;

    void Start()
    {
        itemSlots = new List<UI_Slot_Item>();
        itemSlots = Slots_parent_gameObject.GetComponentsInChildren<UI_Slot_Item>().ToList();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].SetSlotNumber(i);
            itemSlots[i].SetListener(this);
            itemSlots[i].Setmoveable_icon_gameObject(moveable_icon_gameObject);
            itemSlots[i].Set_UI_DisplayItemsInGridHandler(this);
        }
    }

    public bool Add(INGAME_Item_Data item, int slotNumber)
    {
        return itemSlots[slotNumber].Add((I_UI_SlotInterfacer)item);
    }

    public void ClearSlots()
    {
        foreach (var slot in itemSlots)
        {
            slot.ClearSlot();
        }
    }

    public void Remove(int slot)
    {
        itemSlots[slot].ClearSlot();
        var actionSlot = itemSlots[slot];
        itemSlots.RemoveAt(slot);
        itemSlots.Add(actionSlot);
        itemSlots[^1].SetSlotNumber(itemSlots.Count - 1);
    }

    public void UpdateDisplay(UnitController unit)
    {
        ClearSlots();

        if (unit != null)
        {
            displayedUnit = unit;
            if (isEquiptment)
            {
                personalInventorySystem = displayedUnit.GetpersonalInventoryEquiptmentSystem();

                for (int i = 0; i < displayedUnit.GetpersonalInventoryEquiptmentSystem().GetSize(); i++)
                {
                    if (displayedUnit.GetpersonalInventoryEquiptmentSystem().IsSlotFull(i))
                    {
                        Add(displayedUnit.GetpersonalInventoryEquiptmentSystem().GetItem(i), i);
                    }
                }

                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (i > (displayedUnit.GetpersonalInventoryEquiptmentSystem().GetSize() - 1))
                    {
                        itemSlots[i].SetIcon(disabledIcon);
                    }
                }
            }
            else
            {
                if (player != null)
                {
personalInventorySystem = player.GetPersonalInventorySystem();

                for (int i = 0; i < personalInventorySystem.GetSize(); i++)
                {
                    if (personalInventorySystem.IsSlotFull(i))
                    {
                        Add(personalInventorySystem.GetItem(i), i);
                    }
                }

                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (i > (personalInventorySystem.GetSize() - 1))
                    {
                        itemSlots[i].SetIcon(disabledIcon);
                    }
                }
                }
                
            }
        }
    }

    public void iconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj)
    {
        personalInventorySystem.Use(slotNumber);
        RaiseNeedToUpdateUI();
    }

    public void SetPlayerListener(PlayerManager player)
    {
        this.player = player;
    }

    public void RaiseNeedToUpdateUI()
    {
        UIManager.UpdateUI(displayedUnit);
    }

    public void SetTooltipPopup(UI_TooltipPopup tooltipPopup)
    {
        this.tooltipPopup = tooltipPopup;
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].SetTooltipPopup(tooltipPopup);
        }
    }

    public void SwapSlots(UI_Slot_General from, UI_Slot_General to)
    {
        var fromSlotNumber = from.GetSlotNumber();
        var toSlotNumber = to.GetSlotNumber();

        if (toSlotNumber < personalInventorySystem.GetSize())
        {
            var fromItem = personalInventorySystem.GetItem(fromSlotNumber);
            personalInventorySystem.RemoveAt(fromSlotNumber);

            if (!to.IsEmpty())
            {
                var toItem = personalInventorySystem.GetItem(toSlotNumber);

                personalInventorySystem.RemoveAt(toSlotNumber);

                personalInventorySystem.Add(toItem, fromSlotNumber);
            }

            personalInventorySystem.Add(fromItem, toSlotNumber);
            RaiseNeedToUpdateUI();
        }
    }

    public void RemoveFromInventory(int slot)
    {
        personalInventorySystem.RemoveAt(slot);
        RaiseNeedToUpdateUI();
    }

    public UnitController GetDisplayedObject()
    {
        return displayedUnit;
    }

    public void SetUIManager(UIManager UIManager)
    {
        this.UIManager = UIManager;
    }

    public void Setmoveable_icon_gameObject(GameObject moveable_icon_gameObject)
    {
        this.moveable_icon_gameObject = moveable_icon_gameObject;
    }

    public PlayerManager Getplayer()
    {
        return player;
    }

}
