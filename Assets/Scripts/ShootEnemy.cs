using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : Enemy
{
    protected override void Start()
    {
        StartCoroutine(Move());
    }

    protected override IEnumerator Move()
    {
        yield return base.Move();

        StartCoroutine(Shoot());
    }

    protected override IEnumerator Shoot()
    {
        WaitForSeconds shootDelay = new WaitForSeconds(2f);
        Player player = Player.instance;
        GameObject curBullet;

        float setSpeed;
        float targetAngle;
        int plusAngle;

        while (true)
        {
            yield return shootDelay;

            targetAngle = Mathf.Atan2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z) * Mathf.Rad2Deg;
            
            switch (power)
            {
                case 0:
                    plusAngle = -15;

                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(bullet, transform.position, Quaternion.Euler(0f, targetAngle + plusAngle + 180f, 0f));
                        plusAngle += 15;
                    }
                    break;
                case 1:
                    plusAngle = -30;

                    for (int i = 0; i < 5; i++)
                    {
                        curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, targetAngle + plusAngle + 180f, 0f));
                        
                        if (i == 1 || i == 3)
                        {
                            curBullet.GetComponent<Bullet>().SetSpeed(-12f);
                        }

                        plusAngle += 15;
                    }
                    break;
                case 2:
                    plusAngle = -60;

                    for (int i = 0; i < 11; i++)
                    {
                        setSpeed = i % 3 == 0 ? -13f : -8f;

                        curBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0f, targetAngle + plusAngle + 180f, 0f));

                        curBullet.GetComponent<Bullet>().SetSpeed(setSpeed);

                        plusAngle += 15;
                    }
                    break;
            }
            
        }
    }
}
