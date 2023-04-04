using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            player.skillRangeInEnemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            player.skillRangeInEnemys.Remove(other.gameObject);
        }
    }
}
