using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour 
{

    public float duration, increasedSize, normalSize;

    private void Start()
    {
        var mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(increasedSize, duration)) //присоединяем твин
        .Append(transform.DOScale(normalSize, duration)) //присоединяем ещё один твин - обратный путь 
        .SetLoops(-1, LoopType.Restart); //повторяем всё заново 
    }
}
