using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/SkinData", order = 5)]
public class SkinData : ScriptableObject
{
    public Sprite Icon;
    public SkinType type;
    public int cost;
}
