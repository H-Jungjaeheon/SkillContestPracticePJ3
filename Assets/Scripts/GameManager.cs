using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RankingData
{
    public string name;

    public int score;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<RankingData> ranking = new List<RankingData>();

    private StageManager sm;

    [SerializeField]
    private int score; //게임 시작 버튼 누르면 초기화

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    private string curName;

    public string CurName
    {
        get
        {
            return curName;
        }
        set
        {
            curName = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sm = StageManager.instance;
    }

    public void PlusScore(int value)
    {
        Score += value;
        sm.scoreText.text = $"{Score} 점";
    }

    public void RankingSort(RankingData value)
    {
        ranking.Add(value);
        ranking.Sort(SortStart);

        ranking.RemoveAt(5);

        RankingManager.instance.RankingTextUpdate();
    }

    private int SortStart(RankingData a, RankingData b)
    {
        if (a.score == b.score)
        {
            return 0;
        }
        else
        {
            return a.score > b.score ? -1 : 1;
        }
    }
}
