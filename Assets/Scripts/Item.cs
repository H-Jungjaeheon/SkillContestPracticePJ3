using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int randIndex;

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Sprite[] sprites;

    private Vector3 moveVector;

    private void Awake()
    {
        randIndex = Random.Range(0, 5);

        if (randIndex >= 3)
        {
            randIndex = 3;
        }

        sr.sprite = sprites[randIndex];
    }

    private void Start()
    {
        moveVector.z = -1f;
        Destroy(gameObject, 6f);
    }

    private void FixedUpdate()
    {
        transform.Translate(moveVector * Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = Player.instance;

            switch (randIndex)
            {
                case 0:
                    player.PowerUp();
                    break;
                case 1:
                    player.StartShield();
                    break;
                case 2:
                    player.HealHp(50);
                    break;
                case 3:
                    player.HealGas(100);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
