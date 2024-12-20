using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : GameUnit
{
    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform canvasRect;

    [Header("Score")]
    [SerializeField] private Image scoreImage;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    [Header("Direction")]
    [SerializeField] private Image dirImage;
    [SerializeField] private RectTransform dirIndicator;
    [SerializeField] private Transform target;

    [Header("Character Name")]
    [SerializeField] private TextMeshProUGUI nameTxt;

    
    Vector3 screenHalf = new Vector2(Screen.width, Screen.height) / 2;

    Vector3 viewPoint;

    Vector2 viewPointX = new Vector2(0.075f, 0.925f);
    Vector2 viewPointY = new Vector2(0.05f, 0.85f);

    Vector2 viewPointInCameraX = new Vector2(0.075f, 0.925f);
    Vector2 viewPointInCameraY = new Vector2(0.05f, 0.95f);

    Camera Camera => GameManager.Ins.screen;

    private bool IsInCamera => viewPoint.x > viewPointInCameraX.x && viewPoint.x < viewPointInCameraX.y && viewPoint.y > viewPointInCameraY.x && viewPoint.y < viewPointInCameraY.y;

    private void LateUpdate()
    {
        viewPoint = Camera.WorldToViewportPoint(target.position);
        dirIndicator.gameObject.SetActive(!IsInCamera);
        nameTxt.gameObject.SetActive(IsInCamera);

        viewPoint.x = Mathf.Clamp(viewPoint.x, viewPointX.x, viewPointX.y);
        viewPoint.y = Mathf.Clamp(viewPoint.y, viewPointY.x, viewPointY.y);

        Vector3 targetSPoint = Camera.ViewportToScreenPoint(viewPoint) - screenHalf;
        Vector3 playerSPoint = Camera.WorldToScreenPoint(LevelManager.Ins.currentPlayer.TF.position) - screenHalf;
        canvasRect.anchoredPosition = targetSPoint;

        dirIndicator.up = (targetSPoint - playerSPoint).normalized;
    }

    private void OnInit()
    {
        SetScore(0);
        SetColor(new Color(Random.value, Random.value, Random.value, 1));
        SetAlpha(GameManager.Ins.IsState(GameState.Gameplay) ? 1 : 0);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        OnInit();
    }

    public void SetScore(int score)
    {
        scoreTxt.SetText(score.ToString());
    }

    public void SetName(string name)
    {
        nameTxt.SetText(name);
    }

    private void SetColor(Color color)
    {
        scoreImage.color = color;
        nameTxt.color = color;
    }

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}
