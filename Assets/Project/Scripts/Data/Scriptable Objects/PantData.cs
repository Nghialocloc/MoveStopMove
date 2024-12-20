using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PantData", menuName = "ScriptableObjects/PantData", order = 3)]
public class PantData : ScriptableObject
{
    public Sprite Icon;
    public PantType type;
    public int cost;
}
