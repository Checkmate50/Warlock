using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    protected Enemy kobold;
    [SerializeField]
    protected int spawnTimer;

    int spawnCD;

    protected new virtual void Start() {
        base.Start();
        spawnCD = 0;
    }

    protected new virtual void Update() {
        base.Update();
        spawnCD -= 1;
        if (spawnCD <= 0) {
            spawnEnemy(kobold, Random.insideUnitCircle.normalized * Random.Range(1.0f, 5.0f));
            spawnCD = spawnTimer;
        }
    }

    protected void spawnEnemy(Enemy enemy, Vector2 position) {
        Enemy result = Instantiate(enemy, position, Quaternion.identity);
        result.initialize(gameController);
        gameController.addEnemy(result);
    }
}
