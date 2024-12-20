using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Color[] bgColor;
    [SerializeField] private Image bgIcon;

    [Header("Item Info")]
    public Enum itemID;
    [SerializeField] private Image icon;
    [SerializeField] private BuyState state;
    [SerializeField] public int cost;

    private UISkinShop shop;

    // Lua chon item
    public void Select()
    {
        shop.SelectItem(this);
    }

    // Kich hoat background hien thi trang thai item
    public void SetState(BuyState state)
    {
        bgIcon.color = bgColor[(int)state];
    }

    public BuyState GetItemState()
    {
        return state;
    }

    #region Set Info in Container

    public void SetHatData(HatData itemData, UISkinShop shop, BuyState value)
    {
        itemID = itemData.type;
        this.shop = shop;
        state = value;
        icon.sprite = itemData.Icon;
        cost = itemData.cost;
    }

    public void SetPantData(PantData itemData, UISkinShop shop, BuyState value)
    {
        itemID = itemData.type;
        this.shop = shop;
        state = value;
        icon.sprite = itemData.Icon;
        cost = itemData.cost;
    }

    public void SetSkinData(SkinData itemData, UISkinShop shop, BuyState value)
    {
        itemID = itemData.type;
        this.shop = shop;
        state = value;
        icon.sprite = itemData.Icon;
        cost = itemData.cost;
    }

    public void SetAccessoryData(AccessoryData itemData, UISkinShop shop, BuyState value)
    {
        itemID = itemData.type;
        this.shop = shop;
        state = value;
        icon.sprite = itemData.Icon;
        cost = itemData.cost;
    }

    #endregion
}
