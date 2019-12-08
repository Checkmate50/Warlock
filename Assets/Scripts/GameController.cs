using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [SerializeField]
    PlayerSpawner playerSpawner;
    [SerializeField]
    EnemySpawner enemySpawner;
    [SerializeField]
    Camera mainCamera;

    private List<GameObject> players;
    private List<GameObject> enemies;

    public List<GameObject> Players {
        get {
            return players;
        }
    }

    public Camera MainCamera {
        get {
            return mainCamera;
        }
    }

    private void Start() {
        players = new List<GameObject>();
        enemies = new List<GameObject>();

        createInitialSpawners();
    }

    public void addEnemy(Enemy e) {
        enemies.Add(e.gameObject);
    }

    public void enemyDeath(Enemy e) {
        enemies.Remove(e.gameObject);
    }

    public void addPlayer(Player p) {
        players.Add(p.gameObject);
    }

    public void playerDeath(Player p) {
        players.Remove(p.gameObject);
        if (players.Count == 0) {
            Application.Quit();
        }
    }

    private void createInitialSpawners() {
        PlayerSpawner psIns = Instantiate(playerSpawner);
        psIns.initialize(this);
        psIns.addPlayer();
        EnemySpawner enIns = Instantiate(enemySpawner);
        enIns.initialize(this);
    }
}
