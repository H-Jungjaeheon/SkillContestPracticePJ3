using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Ready,
    Playing
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public GameState gameState;

    [SerializeField]
    private Image stageOpImg;

    [SerializeField]
    private EnemySpawner es;

    [SerializeField]
    private GameObject warningObj;

    [SerializeField]
    private GameObject bossHpUIObj;

    public Image bossHpBarImg;

    [SerializeField]
    private Image warningImg;

    public Text waveText;

    public Text waveStartText;

    public Text scoreText;

    public Text enemyDeadCountText;

    public Image hpBarImg;

    public Text hpText;

    public Image gasBarImg;

    public Text gasText;

    public Image expBarImg;

    [SerializeField]
    private Image hitEffectImg;

    [SerializeField]
    private Image skillUnUseImg;

    public Text[] skillCoolTimeTexts;

    public Image[] skillCoolTimeImgs;

    public Text[] skillUseCountTexts;

    private IEnumerator skillUnUseAnimCoroutine;

    private IEnumerator playerHitAnimCoroutine;

    // 받아와야 하는 정보 : 경험치, 체력, 가스, 스코어

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        gameState = GameState.Ready;
    }

    private void Start()
    {
        Player player = Player.instance;

        StartCoroutine(StageOp());

        hpBarImg.fillAmount = player.Hp / player.MaxHp;
        hpText.text = $"{player.Hp}/{player.MaxHp}";

        gasBarImg.fillAmount = player.Gas / player.MaxGas;
        gasText.text = $"{player.Gas}/{player.MaxGas}";

        expBarImg.fillAmount = player.Exp / player.MaxExp;

        scoreText.text = $"Score:{GameManager.instance.Score}";
    }

    private IEnumerator StageOp()
    {
        Color color = Color.white;

        yield return new WaitForSeconds(3.5f);

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            stageOpImg.color = color;

            yield return null;
        }

        stageOpImg.enabled = false;

        gameState = GameState.Playing;

        es.EnemySpawnStart();
    }

    public void PlayerHitAnimStart()
    {
        if (playerHitAnimCoroutine != null)
        {
            StopCoroutine(playerHitAnimCoroutine);
        }

        playerHitAnimCoroutine = PlayerHitAnim();
        StartCoroutine(playerHitAnimCoroutine);
    }

    private IEnumerator PlayerHitAnim()
    {
        Color color;

        hitEffectImg.enabled = true;

        color = Color.white;

        while (color.a > 0f)
        {
            hitEffectImg.color = color;

            color.a -= Time.deltaTime * 1.5f;

            yield return null;
        }

        hitEffectImg.enabled = false;
    }

    public void SkillUnUseAnimStart()
    {
        if (skillUnUseAnimCoroutine != null)
        {
            StopCoroutine(skillUnUseAnimCoroutine);
        }

        skillUnUseAnimCoroutine = SkillUnUseAnim();
        StartCoroutine(skillUnUseAnimCoroutine);
    }

    private IEnumerator SkillUnUseAnim()
    {
        Color color = Color.white;

        skillUnUseImg.enabled = true;

        while (color.a > 0)
        {
            skillUnUseImg.color = color;

            color.a -= Time.deltaTime;

            yield return null;
        }

        skillUnUseImg.enabled = false;
    }

    public IEnumerator WaveStartTextAnim(int curWave)
    {
        float maxCount = 2.5f;
        float curCount = 0f;

        RectTransform rt = waveStartText.rectTransform;

        waveStartText.text = $"Wave{curWave} Start!";

        while (curCount < maxCount)
        {
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, new Vector2(-20f, -100f), 0.05f);
            curCount += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        curCount = 0;

        while (curCount < maxCount)
        {
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, new Vector2(380f, -100f), 0.05f);
            curCount += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator WarningAnim()
    {
        Color color = Color.red;

        color.a = 0;

        warningObj.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            while (color.a < 0.35f)
            {
                warningImg.color = color;
                color.a += Time.deltaTime * 0.5f;

                yield return null;
            }

            while (color.a > 0f)
            {
                warningImg.color = color;
                color.a -= Time.deltaTime * 0.5f;

                yield return null;
            }
        }

        warningObj.SetActive(false);

        gameState = GameState.Ready;

        es.BossSpawn();
    }

    public IEnumerator BossHpBarStartAnim()
    {
        float amount = 0f;

        bossHpUIObj.SetActive(true);

        while (amount < 0f)
        {
            bossHpBarImg.fillAmount = amount / 1f;

            amount += Time.deltaTime * 1.5f;
            
            yield return null;
        }
    }

    public void BossHpBarHide() => bossHpUIObj.SetActive(false);
}
