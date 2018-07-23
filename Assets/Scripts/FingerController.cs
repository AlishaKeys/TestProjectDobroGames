using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class FingerController : MonoBehaviour
{
    private static FingerController instance;

    public static FingerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FingerController>();
            }
            return instance;
        }
    }
    [Header("Line")]
    [SerializeField] int countLine;
    LineRenderer line;

    [Header("Circle")]
    [SerializeField] float durationDestroy;
    SpriteRenderer currentColor;

    bool isMousePressed;
    Vector3 mousePos;

    List<SpriteRenderer> clicksOnCircles;

    public delegate void OnSpawnCircles();
    public static event OnSpawnCircles SpawnCircles;

    private void Start()
    {
        CircleConrtoller.EventClickCircle += ClickCircle;

        line = gameObject.GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("UI/Default"));
        line.widthCurve = new AnimationCurve(new Keyframe(0.1f, 0.1f));
        line.SetColors(Color.green, Color.red);
        line.useWorldSpace = true;
        isMousePressed = false;

        var startMousePos = Vector2.zero;

        clicksOnCircles = new List<SpriteRenderer>();

        var click = Observable.EveryUpdate()
            .Subscribe(xs =>
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var intersection = Physics2D.OverlapPoint(startMousePos, LayerMask.NameToLayer("Circle"));

                    if (intersection && intersection.CompareTag("Circle"))
                    {
                        print(intersection.name);
                        currentColor = intersection.GetComponent<SpriteRenderer>();

                        clicksOnCircles.Add(currentColor);
                        line.positionCount = countLine;

                        SetLinePosition(line.positionCount - 1, clicksOnCircles[0].transform.position);

                        isMousePressed = true;

                        line.SetColors(currentColor.color, currentColor.color);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isMousePressed = false;

                    var clicks = clicksOnCircles.Distinct();
                    GameManager.Instance.AddScore(clicks.Count());

                    if (clicks.Count() > 1)
                    {
                        foreach (var clickCircle in clicks)
                        {
                            clickCircle.transform.DOScale(Vector3.zero, durationDestroy)
                                .OnComplete(() =>
                                {
                                    Destroy(clickCircle.gameObject);
                                    Observable.Timer(System.TimeSpan.FromSeconds(.1f))
                                        .Subscribe(_ => SpawnCircles()).AddTo(this);
                                });
                        }
                    }

                    clicksOnCircles.Clear();
                    line.positionCount = 0;
                }

                if (isMousePressed)
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;
                    SetLinePosition(line.positionCount, mousePos);
                }
            })
            .AddTo(this);
    }

    void SetLinePosition(int index, Vector3 pos)
    {
            line.SetPosition(index - 1, pos);
    }

    public void ClickCircle(SpriteRenderer circleColor)
    {
        if (isMousePressed && circleColor && currentColor.color == circleColor.color)
        {
            float distance = (clicksOnCircles[clicksOnCircles.Count - 1].bounds.center - circleColor.bounds.center).sqrMagnitude;
            if (distance < 1 && clicksOnCircles.Count > 0)
            {
                if (clicksOnCircles.Count > 1 && clicksOnCircles[clicksOnCircles.Count - 2].bounds.center == circleColor.bounds.center)
                {
                    print(currentColor);
                    clicksOnCircles.RemoveAt(clicksOnCircles.Count - 1);
                }
                else
                {
                    clicksOnCircles.Add(circleColor);
                }
                line.positionCount = clicksOnCircles.Count;

                for (int i = 0; i < clicksOnCircles.Count; i++)
                {
                    line.SetPosition(i, clicksOnCircles[i].bounds.center);
                }
            }
        }

    }

}