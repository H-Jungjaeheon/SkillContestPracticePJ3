using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet2 : Bullet
{
    protected override void Start()
    {
        StartCoroutine(StartMove());
        Destroy(gameObject, destroyTime);
    }

    private IEnumerator StartMove()
    {
        float startMoveTime = 0;
        float targetAngle;

        Vector3 moveVector = Vector3.zero;

        moveVector.z = 1f;

        while (startMoveTime < 2f)
        {
            transform.Translate(moveVector * speed * Time.deltaTime);

            startMoveTime += Time.deltaTime;
            
            yield return null;
        }

        targetAngle = Mathf.Atan2(Player.instance.transform.position.x - transform.position.x, Player.instance.transform.position.z - transform.position.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, targetAngle + 180f, 0f);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Move());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Unit unit = other.GetComponent<Unit>();

            unit.StartCoroutine(unit.Hit(damage));

            Destroy(gameObject);
        }
    }
}
