using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSprite : UpObject
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();

        alpha = spriteRenderer.color;
    }

    public override void Update()
    {
        base.Update();
        spriteRenderer.color = alpha;
    }
}
