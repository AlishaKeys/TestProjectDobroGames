using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    static UIController instance;
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
            }
            return instance;
        }
    }

    public Text[] scoreCounters;

    public void UpdateScores()
    {
        string scoreString = GameManager.Instance.TotalScore.ToString();
        foreach(var text in scoreCounters)
        {
            text.text = scoreString;
        }
    }
}
