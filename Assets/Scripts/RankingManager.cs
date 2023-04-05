using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private GameManager gm;

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text curScoreText;

    [SerializeField]
    private Text[] rankingTexts;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        gm = GameManager.instance;
        curScoreText.text = $"Score:{gm.Score}";
    }

    public void InputName()
    {
        if (inputField.text.Length == 3)
        {
            RankingData curRankingData = new RankingData();

            curRankingData.name = inputField.text;
            curRankingData.score = gm.Score;

            gm.RankingSort(curRankingData);

            inputField.text = "";
        }
    }

    public void RankingTextUpdate()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            rankingTexts[i].text = $"{gm.ranking[i].name}:{gm.ranking[i].score}"; 
        }
    }
}
