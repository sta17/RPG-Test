using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Slot_Item : UI_Slot_General, IDropHandler
{
    public delegate void SlotUpdated(UI_Slot_Item _slot);
    [SerializeField] public SlotUpdated OnAfterUpdate;
    [SerializeField] public SlotUpdated OnBeforeUpdate;

    [Header("Item")]
    public ItemSlotTypes slotType;
    public INGAME_Item_Data item = new();

    [Header("UI")]
    public Button removeButton;
    public TextMeshProUGUI stackText;
    public UI_DisplayItemsInGridHandler UI_DisplayItemsInGridHandler;

    [Header("Icon")]
    [SerializeField] protected GameObject moveable_icon_gameObject;
    [SerializeField] private UI_DragableIcon MoveableIcon;

    public void Start()
    {
        MoveableIcon.slot = this;
        //MoveableIcon.moveable_icon_rectTransform = moveable_icon_gameObject.GetComponent<RectTransform>();
    }

    public new bool Add(I_UI_SlotInterfacer newObj)
    {
        objInSlot = newObj;

        icon.sprite = objInSlot.getIcon();
        icon.enabled = true;
        button.interactable = true;

        removeButton.interactable = true;
        return true;
    }

    public void SetIcon(Sprite newIcon)
    {
        icon.sprite = newIcon;
        icon.enabled = true;
        icon.color = Color.gray;
    }

    public new void ClearSlot()
    {
        objInSlot = null;

        icon.sprite = null;
        icon.enabled = false;
        button.interactable = false;

        removeButton.interactable = false;
    }

    public void UpdateSlot(INGAME_Item_Data _item, int _amount)
    {
        OnBeforeUpdate?.Invoke(this);
        item = _item;
        _item.SetItemCurrentAmount(_amount);
        OnAfterUpdate?.Invoke(this);
    }
    public void RemoveItem()
    {
        UpdateSlot(new INGAME_Item_Data(), 0);
    }

    public new void Use()
    {
        //Debug.Log("slot used");
        if (objInSlot != null)
        {
            Listener.iconInteractedWith(SlotNumber, objInSlot);
        }
    }

    public void OnRemoveButton()
    {
        //inventory.Remove(item);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //get icon
            var icon = eventData.pointerDrag.GetComponent<UI_DragableIcon>();
            var itemSlot = icon.slot;
            var item = (INGAME_Item_Data)itemSlot.GetObjInSlot();
            var fromItemType = item.getItemSlotType();
            var toItemType = slotType;

            //check if empty
            if (objInSlot == null && icon != null)
            {

                if ((toItemType == ItemSlotTypes.Generic) | (fromItemType == toItemType))
                {
                    if ((fromItemType != ItemSlotTypes.Generic) | (toItemType != ItemSlotTypes.Generic))
                    {
                        var itemStateDropped = (INGAME_Item_Data)itemSlot.GetObjInSlot();
                        var listener = (I_UI_IconItemListener)itemSlot.GetListener();
                        var displayedUnit = listener.GetDisplayedObject();

                        //Debug.Log("this: " + this.name);
                        //Debug.Log("displayedUnit: " + displayedUnit.name);
                        if (this != displayedUnit)
                        {
                            bool result = false;

                            if (toItemType == ItemSlotTypes.Generic)
                            {
                                var inv = displayedUnit.GetpersonalInventorySystem();

                                result = inv.Add(itemStateDropped, SlotNumber);
                            }
                            else if (fromItemType != ItemSlotTypes.Generic)
                            {
                                var inv = displayedUnit.GetpersonalInventoryEquiptmentSystem();

                                result = inv.Add(itemStateDropped, SlotNumber);

                            }

                            if (result)
                            {
                                // remove item from slot now
                                itemSlot.NotifyListenerDragDrop();
                            }
                        }
                    }
                    else
                    {
                        // if empty do move item script
                        Listener.SwapSlots(from: icon.slot, to: this);
                    }
                }
            }
        }

    }

    public void NotifyListenerDragDrop()
    {
        var IconListener = (I_UI_IconItemListener)Listener;
        IconListener.RemoveFromInventory(SlotNumber);
    }

    public void Setmoveable_icon_gameObject(GameObject moveable_icon_gameObject)
    {
        this.moveable_icon_gameObject = moveable_icon_gameObject;
        MoveableIcon.moveable_icon_rectTransform = moveable_icon_gameObject.GetComponent<RectTransform>();
    }

    public void Set_UI_DisplayItemsInGridHandler(UI_DisplayItemsInGridHandler UI_DisplayItemsInGridHandler)
    {
        this.UI_DisplayItemsInGridHandler = UI_DisplayItemsInGridHandler;
    }
}
