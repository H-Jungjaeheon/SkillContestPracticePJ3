using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public static MainCam instance;

    [SerializeField]
    private GameObject playerObj;

    private Vector3 camObjStartPos;

    private Vector3 bossCamPos;

    private Vector3 shakeVector;

    WaitForSeconds shakeDelay = new WaitForSeconds(0.01f);

    private StageManager sm;

    IEnumerator camShakeCoroutine;

    private bool isBossCam;

    private int shakeCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        camObjStartPos = transform.position;
    }

    private void Start()
    {
        sm = StageManager.instance;
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            if (sm.gameState == GameState.Playing)
            {
                transform.position = Vector3.Lerp(transform.position, playerObj.transform.position + camObjStartPos, 0.01f);
            }

            yield return null;
        }
    }

    public IEnumerator GoToBoss(Vector3 bossPos)
    {
        isBossCam = true;

        bossCamPos = bossPos;

        while (sm.gameState == GameState.Ready)
        {
            transform.position = Vector3.Lerp(transform.position, bossPos + camObjStartPos, 0.015f);

            yield return null;
        }

        isBossCam = false;
    }

    public void CamShakeStart(int count, float amount)
    {
        if (camShakeCoroutine != null)
        {
            StopCoroutine(camShakeCoroutine);
        }

        camShakeCoroutine = CamShake(count, amount);
        StartCoroutine(camShakeCoroutine);
    }

    private IEnumerator CamShake(int count, float amount)
    {
        shakeCount = 0;

        while (shakeCount < count)
        {
            shakeVector.x = Random.Range(-amount, amount);
            shakeVector.y = Random.Range(-amount, amount);
            shakeVector.z = Random.Range(-amount, amount);

            transform.position = transform.position + shakeVector;

            yield return shakeDelay;

            transform.position = isBossCam == true ? bossCamPos + camObjStartPos : playerObj.transform.position + camObjStartPos;

            shakeCount++;
        }
       
        transform.position = isBossCam == true ? bossCamPos + camObjStartPos : playerObj.transform.position + camObjStartPos; ;
    }
}
