using System.Collections;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    protected float maxHp;

    [SerializeField]
    protected float hp;

    [SerializeField]
    protected float speed;

    protected Vector3 moveVector;

    protected WaitForSeconds hitDelay = new WaitForSeconds(0.05f);

    public abstract IEnumerator Hit(int dmg);

    protected abstract IEnumerator Move();

    protected abstract IEnumerator Shoot();

    protected abstract IEnumerator Dead();
}
