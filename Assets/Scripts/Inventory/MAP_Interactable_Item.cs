using UnityEngine;

public class MAP_Interactable_Item : MonoBehaviour
{
    #region Variables, Constructors and Setup

    [Header("Item Properties")]
    [SerializeField] private INGAME_Item_Data item;

    [Header("Selection and Interaction")]
    [SerializeField] private float InteractRange;

    void OnValidate() {
        if (!ReferenceEquals(item, null)) {
            if (item != null) {
                if (item.getItemName() != null) {
                    this.gameObject.name = item.getItemName();
                }
            }
        }   
    }

    #endregion

    #region Getters and Setters

    public float getInteractRange()
    {
        return InteractRange;
    }
    public Sprite getIcon()
    {
        return item.getIcon();
    }

    public ItemTypes getType()
    {
        return item.getType();
    }

    public INGAME_Item_Data getItem()
    {
        return item;
    }

    #endregion

    #region Misc

    public void ModelCleanUp()
    {
        Destroy(this.gameObject);
    }

    public bool HandlePickupType(UnitController unit)
    {
        //Debug.Log("Item Picked up");
        item.Use();
        return true;
    }

    #endregion

}