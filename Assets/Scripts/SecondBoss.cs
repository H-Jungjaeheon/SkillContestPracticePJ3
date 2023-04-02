using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Boss
{
    protected override IEnumerator Shoot()
    {
        yield return pattonDelay;

        randIndex = Random.Range(0, 4);

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
            case 3:
                pattonCoroutine = FourthPatton();
                break;
        }

        StartCoroutine(pattonCoroutine);
    }

    protected override IEnumerator FirstPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.07f);

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 360; j += 30)
            {
                curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, j + i * 9, 0f));

                if (i / 2 == 0)
                {
                    curBullet.GetComponent<Bullet>().SetSpeed(-14f);
                }

                yield return null;
            }

            yield return delay;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    protected override IEnumerator SecondPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.075f);
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

            setSpeed -= 2f;
        }

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

    protected override IEnumerator ThirdPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.025f);

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

            for (int j = 0; j < 360; j += 15)
            {
                curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, j + i * 9, 0f));
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

    private IEnumerator FourthPatton()
    {
        SpriteRenderer sr = shoutEffectObj.GetComponent<SpriteRenderer>();
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        Color color;
        Vector3 scaleVector;

        MainCam.instance.CamShakeStart(120, 1f);
        shoutEffectObj.SetActive(true);

        for (int i = 0; i < 5; i++)
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

        for (int i = 0; i < 40; i++)
        {
            curBullet = Instantiate(bullet, transform.position,
            Quaternion.Euler(0f, Mathf.Atan2(player.transform.position.x - transform.position.x,
            player.transform.position.z - transform.position.z) * Mathf.Rad2Deg + 180f, 0f));

            curBullet.GetComponent<Bullet>().SetSpeed(-20f);

            yield return delay;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }
}
