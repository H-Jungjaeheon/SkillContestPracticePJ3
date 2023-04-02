using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected string targetTag;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected float destroyTime;

    protected virtual void Start()
    {
        StartCoroutine(Move());
        Destroy(gameObject, destroyTime);
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    protected virtual IEnumerator Move()
    {
        Vector3 moveVector = Vector3.zero;

        moveVector.z = 1f;

        while (true)
        {
            transform.Translate(moveVector * speed * Time.deltaTime);

            yield return null;
        }
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
