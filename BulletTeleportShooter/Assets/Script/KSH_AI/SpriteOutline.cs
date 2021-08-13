using UnityEngine;
using DG.Tweening;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;
    private Sequence glowSequence;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        glowSequence = DOTween.Sequence();
        glowSequence.
            Append(DOTween.ToAlpha(() => color, x => color = x, 0, 1f).SetEase(Ease.Linear)).
            Append(DOTween.ToAlpha(() => color, x => color = x, 1, 1f).SetEase(Ease.Linear)).
            SetLoops(-1);
        UpdateOutline(true);
    }

    void OnDisable()
    {
        glowSequence.Kill();
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}