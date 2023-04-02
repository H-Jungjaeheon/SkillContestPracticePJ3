using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField]
    private StageManager sm;

    [SerializeField]
    private int stageIndex;

    [SerializeField]
    private GameObject[] enemys;

    [SerializeField]
    private GameObject boss;

    [SerializeField]
    private Vector3[] spawnPoses;

    private int waveIndex = 1;

    private int maxEnemyDeadCount;

    private int enemyDeadCount;

    WaitForSeconds two = new WaitForSeconds(2f);

    WaitForSeconds four = new WaitForSeconds(4f);

    WaitForSeconds seven = new WaitForSeconds(7f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void EnemySpawnStart()
    {
        switch (stageIndex)
        {
            case 1:
                StartCoroutine(Stage1Spawn());
                break;
            case 2:
                StartCoroutine(Stage2Spawn());
                break;
            case 3:
                StartCoroutine(Stage3Spawn());
                break;
        }
    }

    private IEnumerator Stage1Spawn() //0 1 2 3 4 5 6
    {
        sm.StartCoroutine(sm.WaveStartTextAnim(waveIndex));

        SettingMaxEnemyDeadCount(3);

        Instantiate(enemys[0], spawnPoses[3], Quaternion.identity);
        yield return two;
        Instantiate(enemys[0], spawnPoses[2], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[5], Quaternion.identity);

        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }

        waveIndex++;

        sm.StartCoroutine(sm.WaveStartTextAnim(waveIndex));

        SettingMaxEnemyDeadCount(5);

        Instantiate(enemys[0], spawnPoses[2], Quaternion.identity);
        Instantiate(enemys[0], spawnPoses[4], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[0], Quaternion.identity);
        Instantiate(enemys[0], spawnPoses[6], Quaternion.identity);
        yield return seven;
        Instantiate(enemys[1], spawnPoses[3], Quaternion.identity);

        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }

        waveIndex++;

        sm.StartCoroutine(sm.WaveStartTextAnim(waveIndex));

        SettingMaxEnemyDeadCount(8);

        Instantiate(enemys[0], spawnPoses[1], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[1], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[2], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[2], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[4], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[4], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[5], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[5], Quaternion.identity);

        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }

        waveIndex++;

        sm.StartCoroutine(sm.WaveStartTextAnim(waveIndex));

        SettingMaxEnemyDeadCount(10);

        Instantiate(enemys[0], spawnPoses[2], Quaternion.identity);
        Instantiate(enemys[0], spawnPoses[3], Quaternion.identity);
        Instantiate(enemys[0], spawnPoses[4], Quaternion.identity);
        yield return four;
        Instantiate(enemys[1], spawnPoses[0], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[1], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[5], Quaternion.identity);
        yield return two;
        Instantiate(enemys[1], spawnPoses[6], Quaternion.identity);
        yield return seven;
        Instantiate(enemys[0], spawnPoses[3], Quaternion.identity);
        yield return four;
        Instantiate(enemys[0], spawnPoses[2], Quaternion.identity);
        Instantiate(enemys[0], spawnPoses[4], Quaternion.identity);

        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }

        waveIndex++;

        sm.StartCoroutine(sm.WaveStartTextAnim(waveIndex));

        SettingMaxEnemyDeadCount(1);

        sm.StartCoroutine(sm.WarningAnim());

        yield return null; 
    }

    private IEnumerator Stage2Spawn()
    {

        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }
    }

    private IEnumerator Stage3Spawn()
    {
        while (enemyDeadCount < maxEnemyDeadCount)
        {
            yield return null;
        }
    }

    public void BossSpawn()
    {
        Instantiate(boss, spawnPoses[3], Quaternion.identity);
    }

    private void SettingMaxEnemyDeadCount(int value)
    {
        maxEnemyDeadCount = value;
        enemyDeadCount = 0;

        sm.enemyDeadCountText.text = $"{enemyDeadCount}/{maxEnemyDeadCount}";
    }

    public void EnemyDeadCountPlus()
    {
        enemyDeadCount++;
        sm.enemyDeadCountText.text = $"{enemyDeadCount}/{maxEnemyDeadCount}";
    }
}
