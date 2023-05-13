using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


public class WaveManager : Singleton<WaveManager>
{
    private int wave = 0;
    private List<Wave> currentLevelWaves;

    [SerializeField]
    private Text waveTxt;

    List<EnemyShip> activeEnemies = new List<EnemyShip>();

    [SerializeField]
    private GameObject waveBtn;

    private Wave CurrentWave { get => currentLevelWaves[wave - 1]; }

    public bool WaveActive
    {
        get
        {
            return activeEnemies.Count > 0;
        }
    }

    public void StartWave()
    {
        wave++;
        waveTxt.text = string.Format("Wave: <color=#FFD700>{0}</color>", wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        foreach(SpawnInfo spawnInfo in CurrentWave.spawnInfos)
        {
            EnemyShip enemy = GameManager.Instance.Pool.GetObject(spawnInfo.enemyType).GetComponent<EnemyShip>();
            enemy.Spawn();
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(spawnInfo.spawnCooldown);
        }
    }

    public void RemoveEnemy(EnemyShip enemy)
    {
        activeEnemies.Remove(enemy);
        if (!WaveActive && GameManager.Instance.canShowNewWaveButton())
        {
            GameManager.Instance.Currency += CurrentWave.waveReward;

            if (wave >= currentLevelWaves.Count)
            {
                GameManager.Instance.finishGame();
            } else
            {
                waveBtn.SetActive(true);
            }
        }
    }

    public void LoadLevel(string wavesFileName)
    {
        TextAsset wavesJson = Resources.Load<TextAsset>(wavesFileName);
        currentLevelWaves = JsonConvert.DeserializeObject<List<Wave>>(wavesJson.ToString());
        waveBtn.SetActive(true);
    }
}
