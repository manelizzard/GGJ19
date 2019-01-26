using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundTilingStar : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .PrependInterval(Random.RandomRange(0, 2))
            .Append(canvasGroup.DOFade(0.2f, Random.Range(2f, 6f)))
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }
}
