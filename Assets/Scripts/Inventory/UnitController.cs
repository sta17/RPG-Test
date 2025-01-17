using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour, IDropHandler
{
    #region variables, Setup and Constructors

    [Header("Unit Properties")]
    [SerializeField] private UI_PersonalInventorySystem personalInventoryEquiptment;
    [SerializeField] private int EquiptmentInventorySize = 7;

    [Header("Misc")]
    [SerializeField] private PlayerManager player;

    public void Start()
    {
        personalInventoryEquiptment = new UI_PersonalInventorySystem(EquiptmentInventorySize);
    }

    #endregion

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var itemSlot = eventData.pointerDrag.GetComponent<UI_DragableIcon>().slot;
            var itemStateDropped = (INGAME_Item_Data)itemSlot.GetObjInSlot();
            var listener = (I_UI_IconItemListener)itemSlot.GetListener();
            var displayedUnit = listener.GetDisplayedObject();

            if (this != displayedUnit)
            {
                var result = player.GetPersonalInventorySystem().Add(itemStateDropped);
                if (result)
                {
                    // remove item from slot now
                    itemSlot.NotifyListenerDragDrop();
                }
            }
        }
    }

    public UI_PersonalInventorySystem GetpersonalInventorySystem()
    {
        return player.GetPersonalInventorySystem();
    }

    public UI_PersonalInventorySystem GetpersonalInventoryEquiptmentSystem()
    {
        return personalInventoryEquiptment;
    }

}
