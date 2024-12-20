using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HatData", menuName = "ScriptableObjects/HatData", order = 2)]
public class HatData : ScriptableObject 
{
    public Sprite Icon;
    public HatType type;
    public int cost;
}
