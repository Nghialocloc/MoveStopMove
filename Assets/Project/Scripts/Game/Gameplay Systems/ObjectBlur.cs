using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBlur : MonoBehaviour
{
    [SerializeField] private MeshRenderer render;
    [SerializeField] private float fadeAmount;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float disBlur = 10;
    public bool DoFade = false;

    private float originValue;
    private Material objMaterial;

    private void Start()
    {
        objMaterial = render.material;
        originValue = objMaterial.color.a;
    }

    private void Update()
    {
        if(LevelManager.Ins.currentPlayer != null && GameManager.Ins.IsState(GameState.Gameplay))
        {
            if (Vector3.Distance(transform.position, LevelManager.Ins.currentPlayer.TF.position) <= disBlur)
            {
                FadeNow();
            }
            else
            {
                ResetFade();
            }
        }
    }

    public void FadeNow()
    {
        Color currentColor = objMaterial.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, 
            Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        objMaterial.color = smoothColor;
    }

    public void ResetFade()
    {
        Color currentColor = objMaterial.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, 
            Mathf.Lerp(currentColor.a, originValue, fadeSpeed * Time.deltaTime));
        objMaterial.color = smoothColor;
    }
}
