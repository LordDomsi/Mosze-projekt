using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicator : MonoBehaviour
{
    [SerializeField] private Color hitColor;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private float animLength = 0.15f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    //hit animáció
    public void Hit()
    {
        StartCoroutine(HitAnim());
    }

    private IEnumerator HitAnim()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(animLength);
        spriteRenderer.color = originalColor;
    }
}
