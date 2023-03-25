using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public GameObject enemy;
    public GameMap map;
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    private float spawnRate = 0.75f;
    private float spawnTimer = 0;

    void Update()
    {
        handleDeadEnemies();
        handleEnemiesThatReachedTarget();
        moveEnemies();
        handleNewEnemies();
    }

    private void handleNewEnemies()
    {
        if (spawnTimer < spawnRate)
        {
            spawnTimer += Time.deltaTime;
        }
        else
        {
            spawnEnemy();
            spawnTimer = 0;
        }
    }
    private void spawnEnemy()
    {
        var spawnTile = map.getSpawnTile();
        var spawnedEnemy = Instantiate(enemy, spawnTile.transform.position, spawnTile.transform.rotation).GetComponent<Enemy>();
        enemies.Add(spawnedEnemy);
    }

    private void moveEnemies()
    {
        var path = map.getEnemyPath();
        enemies.ForEach(enemy =>
        {
            enemy.move(path[enemy.getCurrentTargetIdx()]);
        });
    }

    private void handleDeadEnemies()
    {
        var deadEnemies = enemies.FindAll(enemy => enemy.isDead());
        enemies = enemies.Except(deadEnemies).ToList();

        deadEnemies.ForEach(enemy => enemy.destroy());
    }

    private void handleEnemiesThatReachedTarget()
    {
        var enemiesThatReachedFinalTarget = new List<Enemy>();
        var path = map.getEnemyPath();

        foreach (var enemy in enemies) {
            var targetIdx = enemy.getCurrentTargetIdx();
            if (enemy.hasReachedTarget(path[targetIdx]))
            {
                if (targetIdx == path.Count - 1)
                {
                    enemiesThatReachedFinalTarget.Add(enemy);
                } 
                else
                {
                    enemy.setCurrentTargetIdx(targetIdx + 1);
                }
            }
        }

        enemiesThatReachedFinalTarget.ForEach(enemy =>
        {
            Debug.Log("enemy reached target");
            enemy.destroy();
        });

        enemies = enemies.Except(enemiesThatReachedFinalTarget).ToList();
    }
}
