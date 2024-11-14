using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    [SerializeField] private Vector3 Offset;
    [SerializeField] private Vector3 Scale;
    private GameObject shadow;
    [SerializeField] private Color shadowColor;

    private void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;
        shadow.transform.localPosition = Offset;
        shadow.transform.localRotation = Quaternion.identity;
        shadow.transform.localScale = Scale;

        //lemásolja az eredeti spriteot
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = renderer.sprite;

        shadowRenderer.material = renderer.material;
        shadowRenderer.color = shadowColor;

        //a sprite mögé rakja az árnyékot
        shadowRenderer.sortingLayerName = renderer.sortingLayerName;
        shadowRenderer.sortingOrder = renderer.sortingOrder-1;
    }
    private void LateUpdate()
    {
        shadow.transform.localPosition = Offset;
    }

}
