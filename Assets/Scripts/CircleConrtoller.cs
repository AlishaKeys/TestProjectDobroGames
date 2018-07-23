using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CircleConrtoller : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    SpriteRenderer currentColor;

    public delegate void OnClickCircle(SpriteRenderer spriteRenderer);
    public static event OnClickCircle EventClickCircle;

    void Awake()
    {
        currentColor = GetComponent<SpriteRenderer>();
    }

    public void Move(float targetMoveY, float duration)
    {
        transform.DOMoveY(targetMoveY, duration)
        .SetEase(Ease.OutBounce);
    }

    void CurrentSprite(SpriteRenderer spriteRenderer)
    {
        spriteRenderer = currentColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventClickCircle != null)
        {
            EventClickCircle(currentColor);
        }

        currentColor.DOFade(.3f, .5f)
            .OnComplete(() =>
            {
                currentColor.DOFade(1f, .5f);
            });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
