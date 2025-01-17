using System;
using UnityEngine;

public class OnPickup : OnEnter
{
    [Header("Model Parts")]
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;
    [SerializeField] private GameObject part4;

    [Header("Main Game Object")]
    [SerializeField] private GameObject part5;

    [Header("Item Properties")]
    [SerializeField] private INGAME_Item_Data item;

    [Header("Item Handler")]
    [SerializeField] private PlayerManager itemHandler;

    public override void StartInteraction()
    {
        bool result = itemHandler.InteractWithItem(this);

        if (result)
        {
            //Debug.Log("Pickup Works.");
            part5.SetActive(false);
        }
    }

    void OnValidate()
    {
        if (!ReferenceEquals(item, null))
        {
            if (item != null)
            {
                    string temp = item.getItemName();

                    if (string.IsNullOrEmpty(temp))
                    {
                        this.gameObject.name = "";
                    }
                    else
                    {
                        this.gameObject.name = temp;
                    }
            }
        }
    }

    public ItemTypes getType()
    {
        return item.getType();
    }

    public INGAME_Item_Data getItem()
    {
        return item;
    }

    public bool HandlePickupType(UnitController unit)
    {
        //Debug.Log("Item Picked up");
        item.Use();
        return true;
    }
}
