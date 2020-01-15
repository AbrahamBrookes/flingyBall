using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PoolableGameObject pooledObject;
    public List<GameObject> pool; // objects are pulled from the pool
    public List<GameObject> game; // and placed in the game

    void Start()
    {
        pool = new List<GameObject>();
        game = new List<GameObject>();
    }

    /**
     *  Spawns a pooledObject - if there is one available in the pool, use that one. If none are
     *  available in the pool, instatiate a new one. New objects are instantiated at the same pos/loc
     *  as they are in the game (because you drag the gameobject from the scene onto this script)
     */
    public GameObject Spawn(Vector3? position = null, Vector3? rotation = null)
    {
        // handle default position and rotation
        if (position == null) position = pooledObject.transform.position;
        if (rotation == null) rotation = pooledObject.transform.eulerAngles;

        GameObject larvae;

        if ( pool.Count < 1) // no objects in the pool
        {
            // instantiate a new object and add it to the game
            larvae = GameObject.Instantiate(pooledObject.gameObject);
            larvae.transform.SetPositionAndRotation((Vector3)position, Quaternion.Euler((Vector3)rotation));
            larvae.SetActive(true);
            game.Add(larvae);

        } else // pool has an object to offer, use it instead
        {
            larvae = pool[0];
            larvae.SetActive(true);
            larvae.transform.SetPositionAndRotation((Vector3)position, Quaternion.Euler((Vector3)rotation));
            game.Add(larvae);
            pool.Remove(larvae);
        }

        larvae.GetComponent<PoolableGameObject>().onSpawn();
        return larvae;
    }

    /**
     *  Stashes the given gameobject back into the pool from the world
     */
    public void Stash(GameObject sacrifice)
    {
        sacrifice.SetActive(false);
        game.Remove(sacrifice);
        pool.Add(sacrifice);
        sacrifice.GetComponent<PoolableGameObject>().onStash();
    }


    /**
     *  Stashes all objects from the game to the pool. Generally used when cleaning up
     */
     public void StashAll()
     {
        foreach( GameObject spawnling in game)
        {
            spawnling.SetActive(false);
            spawnling.GetComponent<PoolableGameObject>().onStash();
            pool.Add(spawnling);
        }
        game.Clear();
     }
}
