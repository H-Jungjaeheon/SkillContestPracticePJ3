using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierBullet : MonoBehaviour
{
    Vector3[] lerpVectors = new Vector3[5];

    public IEnumerator BezierMove(Vector3 targetPos)
    {
        float curMoveIndex = 0f;

        Vector3 startPos = transform.position;
        Vector3 secondPos = transform.position - new Vector3(Random.Range(-6, 6), Random.Range(1, 7), Random.Range(-6, -5));
        Vector3 thirdPos = targetPos - new Vector3(Random.Range(-6, 6), Random.Range(1, 5), Random.Range(-4, -3));

        while (curMoveIndex < 1f)
        {
            lerpVectors[0] = Vector3.Lerp(startPos, secondPos, curMoveIndex / 1f);
            lerpVectors[1] = Vector3.Lerp(secondPos, thirdPos, curMoveIndex / 1f);
            lerpVectors[2] = Vector3.Lerp(thirdPos, targetPos, curMoveIndex / 1f);

            lerpVectors[3] = Vector3.Lerp(lerpVectors[0], lerpVectors[1], curMoveIndex / 1f);
            lerpVectors[4] = Vector3.Lerp(lerpVectors[1], lerpVectors[2], curMoveIndex / 1f);

            transform.position = Vector3.Lerp(lerpVectors[3], lerpVectors[4], curMoveIndex / 1f);

            curMoveIndex += Time.deltaTime * 3.5f;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Unit>().StartCoroutine(other.GetComponent<Unit>().Hit(2));
        }
    }
}
