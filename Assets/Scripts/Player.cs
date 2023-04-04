using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillKind
{
    Heal,
    Boom,
    Kaisa
}

[System.Serializable]
public class SkillData
{
    public int skillLevel;

    public int useCount;

    public float coolTime;

    public float maxCoolTime;
}

public class Player : Unit
{
    public static Player instance;

    [SerializeField]
    private float maxX;

    [SerializeField]
    private float maxZ;

    [SerializeField]
    private float minZ;

    public int level;

    [SerializeField]
    private float exp;

    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
        }
    }

    [SerializeField]
    private float maxExp;

    public float MaxExp
    {
        get
        {
            return maxExp;
        }
        set
        {
            maxExp = value;
        }
    }

    [SerializeField]
    private float gas;

    public float Gas
    {
        get
        {
            return gas;
        }
        set
        {
            gas = value;
            if (value <= 0)
            {
                StartCoroutine(Dead());
            }
        }
    }

    [SerializeField]
    private float maxGas;

    public float MaxGas
    {
        get
        {
            return maxGas;
        }
        set
        {
            maxGas = value;
        }
    }

    [SerializeField]
    private int power;

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }

    [SerializeField]
    private GameObject bullet;

    private GameObject curBullet;

    private GameObject targetObj;

    [SerializeField]
    private GameObject skillTestBullet;

    public List<GameObject> skillRangeInEnemys = new List<GameObject>();

    [SerializeField]
    private SkillData[] sd;

    private StageManager sm;

    WaitForSeconds shootDelay = new WaitForSeconds(0.15f);

    [SerializeField]
    private GameObject shieldObj;

    private bool isShielding;

    IEnumerator shieldCoroutine;

    IEnumerator shieldEndCoroutine;

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
        StartSetting();
    }

    private void StartSetting()
    {
        sm = StageManager.instance;
        StartCoroutine(Move());
        StartCoroutine(Shoot());
        StartCoroutine(Skill());
        StartCoroutine(GasDown());
    }

    public void SkillReSetting()
    {
        for (int i = 0; i < sd.Length; i++)
        {
            sd[i].coolTime = 0;
            sd[i].useCount = 4;
        }
    }

    private IEnumerator Skill()
    {
        while (true)
        {
            if (sm.gameState == GameState.Playing)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (sd[(int)SkillKind.Heal].coolTime == 0 && sd[(int)SkillKind.Heal].useCount > 0)
                    {
                        HealHp(sd[(int)SkillKind.Heal].skillLevel * 10 + 20);
                        sd[(int)SkillKind.Heal].useCount--;
                        sm.skillUseCountTexts[(int)SkillKind.Heal].text = $"{sd[(int)SkillKind.Heal].useCount}";

                        StartCoroutine(HealSkillCoolTime());
                    }
                    else
                    {
                        sm.SkillUnUseAnimStart();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    if (sd[(int)SkillKind.Boom].coolTime == 0 && sd[(int)SkillKind.Boom].useCount > 0)
                    {
                        sd[(int)SkillKind.Boom].useCount--;
                        sm.skillUseCountTexts[(int)SkillKind.Boom].text = $"{sd[(int)SkillKind.Boom].useCount}";

                        StartCoroutine(BoomSkillCoolTime());
                    }
                    else
                    {
                        sm.SkillUnUseAnimStart();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    if (skillRangeInEnemys.Count > 0) //sd[(int)SkillKind.Kaisa].coolTime == 0 && sd[(int)SkillKind.Kaisa].useCount > 0 &&
                    {
                        WaitForSeconds delay = new WaitForSeconds(0.05f);
                        int targetIndex = 0;
                        GameObject[] targetObjs = new GameObject[skillRangeInEnemys.Count];

                        for (int i = 0; i < skillRangeInEnemys.Count; i++)
                        {
                            targetObjs[i] = skillRangeInEnemys[i];
                        }

                        for (int i = 0; i < 20; i++)
                        {
                            curBullet = Instantiate(skillTestBullet, transform.position, Quaternion.identity);

                            curBullet.GetComponent<BezierBullet>().StartCoroutine(curBullet.GetComponent<BezierBullet>().BezierMove(targetObjs[targetIndex].transform.position));

                            targetIndex = (targetIndex + 1) >= targetObjs.Length ? 0 : targetIndex + 1;

                            yield return delay;
                        }

                        sd[(int)SkillKind.Kaisa].useCount--;
                        sm.skillUseCountTexts[(int)SkillKind.Kaisa].text = $"{sd[(int)SkillKind.Kaisa].useCount}";

                        StartCoroutine(KaisaSkillCoolTime());
                    }
                    else
                    {
                        sm.SkillUnUseAnimStart();
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator HealSkillCoolTime()
    {
        sd[(int)SkillKind.Heal].coolTime = sd[(int)SkillKind.Heal].maxCoolTime;

        while (sd[0].coolTime > 0)
        {
            sd[0].coolTime -= Time.deltaTime;
            sm.skillCoolTimeTexts[0].text = $"{sd[0].coolTime:N0}";
            sm.skillCoolTimeImgs[0].fillAmount = sd[0].coolTime / sd[0].maxCoolTime;
            yield return null;
        }

        sd[0].coolTime = 0f;
        sm.skillCoolTimeTexts[0].text = "";
        sm.skillCoolTimeImgs[0].fillAmount = 0;
    }

    private IEnumerator BoomSkillCoolTime()
    {
        sd[(int)SkillKind.Boom].coolTime = sd[(int)SkillKind.Boom].maxCoolTime;

        while (sd[1].coolTime > 0)
        {
            sd[1].coolTime -= Time.deltaTime;
            sm.skillCoolTimeTexts[1].text = $"{sd[1].coolTime:N0}";
            sm.skillCoolTimeImgs[1].fillAmount = sd[1].coolTime / sd[1].maxCoolTime;
            yield return null;
        }

        sd[1].coolTime = 0f;
        sm.skillCoolTimeTexts[1].text = "";
        sm.skillCoolTimeImgs[1].fillAmount = 0;
    }

    private IEnumerator KaisaSkillCoolTime()
    {
        sd[(int)SkillKind.Kaisa].coolTime = sd[(int)SkillKind.Kaisa].maxCoolTime;

        while (sd[2].coolTime > 0)
        {
            sd[2].coolTime -= Time.deltaTime;
            sm.skillCoolTimeTexts[2].text = $"{sd[2].coolTime:N0}";
            sm.skillCoolTimeImgs[2].fillAmount = sd[2].coolTime / sd[2].maxCoolTime;
            yield return null;
        }

        sd[2].coolTime = 0f;
        sm.skillCoolTimeTexts[2].text = "";
        sm.skillCoolTimeImgs[2].fillAmount = 0;
    }

    public void StartShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);    
        }

        if (shieldEndCoroutine != null)
        {
            StopCoroutine(shieldEndCoroutine);
        }

        shieldCoroutine = Shield();
        StartCoroutine(shieldCoroutine);
    }

    private IEnumerator Shield()
    {
        float shieldTime = 4f;
        bool isStartEndEffect = false;

        isShielding = true;

        shieldObj.SetActive(true);

        while (shieldTime > 0f)
        {
            shieldTime -= Time.deltaTime;

            if (shieldTime <= 1.5f && isStartEndEffect == false)
            {
                isStartEndEffect = true;
                shieldEndCoroutine = ShieldEndAnim();
           
                StartCoroutine(shieldEndCoroutine);
            }

            yield return null;    
        }

        isShielding = false;

        shieldObj.SetActive(false);
    }

    private IEnumerator ShieldEndAnim()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        
        while (isShielding)
        {
            shieldObj.SetActive(false);
            yield return delay;
            shieldObj.SetActive(true);
        }

        shieldObj.SetActive(false);
    }

    private IEnumerator GasDown()
    {
        while (true)
        {
            if (sm.gameState == GameState.Playing && Gas > 0f)
            {
                Gas -= Time.deltaTime;
                sm.gasText.text = $"{Gas:N0}/{maxGas}";
                sm.gasBarImg.fillAmount = Gas / maxGas;
            }
            yield return null;
        }
    }

    public void HealGas(int value)
    {
        Gas += value;

        if (Gas > maxGas)
        {
            Gas = maxGas;
        }
    }

    public void HealHp(int value)
    {
        Hp += value;

        if (Hp > maxHp)
        {
            Hp = maxHp;
        }

        sm.hpBarImg.fillAmount = Hp / maxHp;
        sm.hpText.text = $"{Hp}/{maxHp}";
    }

    public override IEnumerator Hit(int dmg)
    {
        if (sm.gameState == GameState.Playing && isShielding == false)
        {
            Hp -= dmg;

            sm.hpBarImg.fillAmount = hp / maxHp;

            MainCam.instance.CamShakeStart(15, 1f);

            if (hp <= 0f)
            {
                sm.hpText.text = $"{0}/{maxHp}";
                StartCoroutine(Dead());
            }
            else
            {
                sm.hpText.text = $"{hp}/{maxHp}";
                sm.PlayerHitAnimStart();
            }
        }

        yield return null;
    }

    protected override IEnumerator Move()
    {
        Vector3 moveRange;

        while (true)
        {
            if (sm.gameState == GameState.Playing)
            {
                moveVector.x = Input.GetAxisRaw("Horizontal");
                moveVector.z = Input.GetAxisRaw("Vertical");

                transform.Translate(moveVector * Time.deltaTime * speed);

                moveRange = new Vector3(Mathf.Clamp(transform.position.x, -maxX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));

                transform.position = moveRange;
            }
            
            yield return null;
        }
    }

    protected override IEnumerator Shoot()
    {
        Vector3 plusVector = Vector3.zero;

        plusVector.z = 0.25f;

        while (true)
        {
            if (Input.GetKey(KeyCode.Z) && sm.gameState == GameState.Playing)
            {
                plusVector = Vector3.zero;
                plusVector.z = 0.25f;

                switch (power)
                {
                    case 0:
                        Instantiate(bullet, transform.position + plusVector, bullet.transform.rotation);
                        break;
                    case 1:
                        plusVector.x = -0.35f;

                        for (int i = 0; i < 2; i++)
                        {
                            Instantiate(bullet, transform.position + plusVector, bullet.transform.rotation);
                            plusVector.x += 0.7f;
                        }

                        break;
                    case 2:
                        for (int i = -10; i <= 10; i += 5)
                        {
                            Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));
                        }
                        break;
                }

                yield return shootDelay;
            }

            yield return null;
        }
    }

    protected override IEnumerator Dead()
    {
        sm.gameState = GameState.Ready;

        throw new System.NotImplementedException();
    }

    public void PowerUp()
    {
        if (power < 2)
        {
            power++;
        }
        else
        {
            //점수 추가
        }
    }
}
