using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickMoneyText : UpObject
{
    public TextMeshPro text;
    public ulong money;

    public void SetUp()
    {
        text = GetComponent<TextMeshPro>();
    }

    public override void Start()
    {
        base.Start();

        alpha = text.color;
        text.text = string.Format("{0:n0}", money);
    }

    public override void Update()
    {
        base.Update();
        text.color = alpha;
    }
}
