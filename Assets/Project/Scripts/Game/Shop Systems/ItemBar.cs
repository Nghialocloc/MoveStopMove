using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemBar : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private SkinShopCatergory type;

    // Dong goi doi týõng
    public SkinShopCatergory Type => type;

    private UISkinShop shop;

    public void SetShop(UISkinShop shop)
    {
        this.shop = shop;
    }

    // Lua chon loai trang bi
    public void Select()
    {
        shop.SelectBar(this);
    }

    // Kich hoat background khi duoc chon
    public void ActiveBar(bool active)
    {
        bg.SetActive(!active);
    }
}
