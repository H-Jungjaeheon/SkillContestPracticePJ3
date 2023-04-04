using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [SerializeField]
    protected int power;

    [SerializeField]
    protected Image hpBarImg;

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
    protected GameObject item;

    protected virtual void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(Shoot());
    }

    public override IEnumerator Hit(int dmg)
    {
        hp -= dmg;

        hpBarImg.fillAmount = hp / maxHp;
        
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

    protected override IEnumerator Move()
    {
        moveVector.z = 1f;

        while (transform.position.z > minZ)
        {
            transform.Translate(moveVector * speed * Time.deltaTime);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, minZ);
    }

    protected override IEnumerator Shoot()
    {
        WaitForSeconds shootDelay = new WaitForSeconds(2.5f);
        WaitForSeconds shootDelay2 = new WaitForSeconds(0.5f);

        GameObject curBullet;

        while (true)
        {
            yield return shootDelay;

            switch (power)
            {
                case 0:
                    for (int i = 0; i <= 360; i += 30)
                    {
                        Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));
                    }
                    break;
                case 1:
                    for (int i = 0; i <= 360; i += 15)
                    {
                        curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));

                        if (i == 0 || i % 2 == 0)
                        {
                            curBullet.GetComponent<Bullet>().SetSpeed(-13f);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i <= 360; i += 15)
                    {
                        Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));
                    }

                    yield return shootDelay2;

                    for (int i = 0; i < 360; i += 90)
                    {
                        curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));
                        curBullet.GetComponent<Bullet>().SetSpeed(-13f);
                    }

                    yield return shootDelay2;

                    for (int i = 45; i < 360; i += 45)
                    {
                        curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, i, 0f));
                        curBullet.GetComponent<Bullet>().SetSpeed(-13f);
                        i += 45;
                    }

                    break;
            }
        }
    }

    protected override IEnumerator Dead()
    {
        int randIndex = Random.Range(0, 2);

        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);

        EnemySpawner.instance.EnemyDeadCountPlus();
       
        MainCam.instance.CamShakeStart(20, 2f);

        if (randIndex == 1)
        {
            Instantiate(item, transform.position, item.transform.rotation);
        }

        transform.position += new Vector3(0f, 20f, 0f);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
