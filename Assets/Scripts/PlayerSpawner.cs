using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    [SerializeField]
    Player player;

    public virtual void addPlayer() {
        Player result = Instantiate(player, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        result.initialize(gameController);
        gameController.addPlayer(result);
    }
}
