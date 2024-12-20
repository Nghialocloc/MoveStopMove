using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : GameUnit
{
    [Header("Model")]
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Renderer pant;
    public Renderer skinColor;
    public MaterialData materialData;

    [Header("Info")]
    public Animator anim;
    public bool isFullSet;

}
