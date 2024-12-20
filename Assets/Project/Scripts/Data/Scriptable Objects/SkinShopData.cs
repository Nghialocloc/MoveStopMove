using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class SkinShopData : ScriptableObject
{
    public List<HatData> hats;
    public List<PantData> pants;
    public List<AccessoryData> accessories;
    public List<SkinData> skins;
}

