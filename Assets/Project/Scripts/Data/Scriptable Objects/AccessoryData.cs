using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryData", menuName = "ScriptableObjects/AccessoryData", order = 4)]
public class AccessoryData : ScriptableObject
{
    public Sprite Icon;
    public AccessoryType type;
    public int cost;
}
