using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

[Serializable]
public class ScaleParams
{
    [Range(0f, 5f)]
    public float scaleUpDamper = .1f;

    [Range(0f, 5f)]
    public float scaleDownDamper = .1f;

    [Range(.1f, 5f)]
    public float maxScale = 1f;

    [Range(.1f, 5f)]
    public float minScale = .3f;

    [Range(.1f, 5f)]
    public float scaleCompletionTime = .5f;

    [Range(0f, 5f)]
    public float scaleStartDelay = 0f;

    public Ease scaleEaseType = Ease.Linear;

    public TweenParams GetScaleTweenParams()
    {
        return new TweenParams().SetDelay(scaleStartDelay).SetEase(scaleEaseType);
    }
}