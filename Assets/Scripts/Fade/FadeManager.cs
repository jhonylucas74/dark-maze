using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    CanvasGroup _cg;

    protected override void Awake()
    {
        base.Awake();

        _cg = GetComponent<CanvasGroup>();
    }

    public void Fade(bool toggle, float delay = 0f, SimpleEvent callback = null)
    {
        _cg.interactable = toggle;
        _cg.blocksRaycasts = toggle;
        _cg.DOFade(toggle ? 1f : 0f, 1f).SetDelay(delay).OnComplete(() => callback?.Invoke()).Play();
    }
}
