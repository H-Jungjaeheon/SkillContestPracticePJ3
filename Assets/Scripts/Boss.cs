using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Unit
{
    [SerializeField]
    protected SpriteRenderer sr;

    [SerializeField]
    protected int giveScore;

    [SerializeField]
    protected float minZ;

    [SerializeField]
    protected GameObject bullet;

    [SerializeField]
    protected GameObject deadParticle;

    [SerializeField]
    protected GameObject shoutEffectObj;

    protected IEnumerator pattonCoroutine;

    protected WaitForSeconds pattonDelay = new WaitForSeconds(1.25f);

    protected StageManager sm;

    protected GameObject curBullet;

    protected int randIndex;

    protected Player player;

    protected virtual void Start()
    {
        sm = StageManager.instance;
        player = Player.instance;
        
        MainCam.instance.StartCoroutine(MainCam.instance.GoToBoss(new Vector3(0f,0f,9f)));

        StartCoroutine(Move());
    }

    public override IEnumerator Hit(int dmg)
    {
        if (sm.gameState == GameState.Playing && hp > 0f)
        {
            hp -= dmg;

            sm.bossHpBarImg.fillAmount = hp / maxHp;

            if (hp <= 0f)
            {
                GameManager.instance.PlusScore(giveScore);

                StartCoroutine(Dead());
            }
            else
            {
                sr.color = Color.red;

                yield return hitDelay;

                sr.color = Color.white;
            }
        }
    }

    protected override IEnumerator Move()
    {
        moveVector.z = 1f;

        while (transform.position.z > minZ)
        {
            transform.Translate(moveVector * speed * Time.deltaTime);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, minZ);

        StartCoroutine(Shout());
    }

    protected virtual IEnumerator Shout()
    {
        SpriteRenderer sr = shoutEffectObj.GetComponent<SpriteRenderer>();
        Color color;
        Vector3 scaleVector;

        sm.StartCoroutine(sm.BossHpBarStartAnim());
        MainCam.instance.CamShakeStart(100, 1f);
        shoutEffectObj.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            color = Color.white;
            scaleVector = Vector3.zero;

            while (color.a > 0)
            {
                shoutEffectObj.transform.localScale = scaleVector;

                scaleVector.x += Time.deltaTime * 70f;
                scaleVector.y += Time.deltaTime * 70f;

                sr.color = color;
                color.a -= Time.deltaTime * 3f;

                yield return null;
            }
        }

        shoutEffectObj.SetActive(false);
        
        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);

        sm.gameState = GameState.Playing;
    }

    protected override IEnumerator Shoot()
    {
        yield return pattonDelay;

        randIndex = Random.Range(0, 3);

        switch (randIndex)
        {
            case 0:
                pattonCoroutine = FirstPatton();
                break;
            case 1:
                pattonCoroutine = SecondPatton();
                break;
            case 2:
                pattonCoroutine = ThirdPatton();
                break;
        }

        StartCoroutine(pattonCoroutine);
    }

    protected virtual IEnumerator FirstPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 360; j += 30)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(0f, j + i * 7, 0f));
                yield return null;
            }

            yield return delay;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    protected virtual IEnumerator SecondPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.085f);
        float setSpeed = -12f;
        float targetAngle;
        int plusAngle;

        targetAngle = Mathf.Atan2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z) * Mathf.Rad2Deg;

        for (int i = 0; i < 5; i++)
        {
            plusAngle = -40;

            for (int j = 0; j < 5; j++)
            {
                curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, targetAngle + plusAngle + 180f, 0f));
                curBullet.GetComponent<Bullet>().SetSpeed(setSpeed);
                plusAngle += 20;
            }

            yield return delay;
            
            setSpeed += 2f;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    protected virtual IEnumerator ThirdPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        int plusAngle;

        for (int i = 0; i < 2; i++)
        {
            plusAngle = -60;
            
            for (int j = 0; j < 13; j++)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(0f, plusAngle, 0f));
                plusAngle += 10;

                yield return delay;
            }

            for (int j = 0; j < 13; j++)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(0f, plusAngle - 5, 0f));
                plusAngle -= 10;

                yield return delay;
            }
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    protected override IEnumerator Dead()
    {
        if (pattonCoroutine != null)
        {
            StopCoroutine(pattonCoroutine);
        }

        sm.gameState = GameState.Ready;

        sm.BossHpBarHide();

        MainCam.instance.CamShakeStart(240, 1f);
        MainCam.instance.StartCoroutine(MainCam.instance.GoToBoss(new Vector3(0f, 0f, 9f)));

        deadParticle.SetActive(true);

        EnemySpawner.instance.EnemyDeadCountPlus();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(2);

        Destroy(gameObject);
    }
}
