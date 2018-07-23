using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Circles Parameters")]
    [SerializeField] Sprite circle;
    [SerializeField] Color[] colorsCircles;
    [SerializeField] GameObject circlePrefab;
    [SerializeField] int sizeCircle;
    [SerializeField] float duration;
    CircleConrtoller[,] circles;

    [Header("Board")]
    [SerializeField] Transform board;

    [Header("Size")]
    public int width, height;

    public void Start()
    {
        circles = new CircleConrtoller[height, width];

        GameManager.CreateCircles += SpawnCircle;
        FingerController.SpawnCircles += SpawnCircle;
    }

    void SpawnCircle()
    {
        Vector2 sizeBoard = circlePrefab.GetComponent<SpriteRenderer>().bounds.size * (sizeCircle * 1.5f); 

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (circles[i, j] == null)
                {
                    CircleConrtoller circle = Instantiate(circlePrefab, new Vector2(board.position.x + (sizeBoard.x * j), 0), board.transform.rotation, board).GetComponent<CircleConrtoller>();
                    circle.Move(board.position.y - (sizeBoard.y * i), duration);

                    circle.transform.DOScale(sizeCircle, duration);

                    int randomColor = Random.Range(0, colorsCircles.Length);
                    circle.transform.DOScale(sizeCircle, duration);

                    circle.GetComponent<SpriteRenderer>().color = colorsCircles[randomColor];

                    circles[i, j] = circle;
                }
            }
        }
    }
}
