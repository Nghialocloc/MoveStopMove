using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "ScriptableObjects/MaterialData", order = 7)]
public class MaterialData : ScriptableObject
{
    //theo tha material theo dung thu tu ColorType
    public Material[] materials;

    //lay material theo mau tuong ung
    public Material GetMat(PantType pantType)
    {
        return materials[(int)pantType];
    }
}
