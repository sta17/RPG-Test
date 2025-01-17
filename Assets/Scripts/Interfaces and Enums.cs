using UnityEngine;

#region Online Resources

// https://coffeebraingames.wordpress.com/2017/10/15/multi-scene-development-in-unity/

// https://stackoverflow.com/questions/1759352/how-to-mark-a-method-as-obsolete-or-deprecated

// [System.Obsolete("Possible Removal")]
// if placed above a method will produce a warning for all uses.

#endregion

#region Interfaces

public interface I_UI_Slot
{
    void Add(I_UI_SlotInterfacer obj);
    void ClearSlot();
    void Use();
}

public interface I_UI_SlotInterfacer
{
    Sprite getIcon();
    void iconInteract();
    int getID();
    string GetColouredName();
    string GetTooltipInfoText();
}

public interface I_UI_Item_SlotInterfacer : I_UI_SlotInterfacer
{
    ItemSlotTypes getItemSlotTypes();
}

public interface I_UI_IconListener
{
    void iconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj);
    void SwapSlots(UI_Slot_General from, UI_Slot_General to);
}

public interface I_UI_IconItemListener : I_UI_IconListener
{
    void RemoveFromInventory(int slot);
    UnitController GetDisplayedObject();
    void SetPlayerListener(PlayerManager player);
    void RaiseNeedToUpdateUI();
}

public interface I_Modifier
{
    void AddValue(ref int baseValue);
}


#endregion

#region Enums

public enum ItemTypes
{
    Generic,
    Pickup,
    Stackable
}

public enum ItemSlotTypes
{
    Generic,
    Head,
    Shoulder,
    Chest,
    Wrist,
    Hands,
    Waist,
    Legs,
    Feet,
    Neck,
    Back,
    Finger,
    Trinket,
    OneHand,
    TwoHand,
    MainHand,
    OffHand
}

public enum Attributes
{
    Agility,
    Intellect,
    Stamina,
    Strength
}

#endregion