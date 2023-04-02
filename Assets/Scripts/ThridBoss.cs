using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridBoss : Boss
{
    [SerializeField]
    private GameObject bullet2;

    [SerializeField]
    private GameObject bullet3;

    protected override IEnumerator Shoot()
    {
        yield return pattonDelay;

        randIndex = Random.Range(0, 5);

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
            case 4:
                pattonCoroutine = FifthPatton();
                break;
            case 5:
                pattonCoroutine = SixthPatton();
                break;
        }

        StartCoroutine(pattonCoroutine);
    }

    protected override IEnumerator FirstPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 360; j += 20)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(0f, j + i * 7, 0f));
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
        float setSpeed = -5f;
        float targetAngle;
        int plusAngle;

        for (int i = 0; i < 8; i++)
        {
            targetAngle = Mathf.Atan2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z) * Mathf.Rad2Deg;
            plusAngle = -40;

            for (int j = 0; j < 5; j++)
            {
                curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, targetAngle + plusAngle + 180f, 0f));
                curBullet.GetComponent<Bullet>().SetSpeed(setSpeed);
                plusAngle += 20;
            }

            yield return delay;

            setSpeed -= 3f;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    protected override IEnumerator ThirdPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(0.025f);

        yield return delay;

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    private IEnumerator FourthPatton()
    {
        WaitForSeconds delay = new WaitForSeconds(1.25f);

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 360; i += 30)
            {
                curBullet = Instantiate(bullet2, transform.position, Quaternion.Euler(0f, i + 180f, 0f));
                curBullet.GetComponent<Bullet>().SetSpeed(-8f);
            }
            yield return delay;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    private IEnumerator FifthPatton()
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

        for (int i = 0; i < 50; i++)
        {
            if (i % 10 == 0)
            {
                for (int j = 0; j < 360; j += 20)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(0f, j, 0f));
                }
            }

            curBullet = Instantiate(bullet, transform.position,
            Quaternion.Euler(0f, Mathf.Atan2(player.transform.position.x - transform.position.x,
            player.transform.position.z - transform.position.z) * Mathf.Rad2Deg + 180f, 0f));

            curBullet.GetComponent<Bullet>().SetSpeed(-25f);

            yield return delay;
        }

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }

    private IEnumerator SixthPatton()
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

        yield return new WaitForSeconds(1.5f);

        pattonCoroutine = Shoot();
        StartCoroutine(pattonCoroutine);
    }
}
