using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject BatPrefab;
    public GameObject PortalPrefab;

    public bool SpawnEnemy(Vector3 spawnPos, string enemyType) {
        if(enemyType.Equals("bat", StringComparison.OrdinalIgnoreCase)) { //https://www.delftstack.com/howto/csharp/compare-two-strings-ignoring-case-in-csharp/
            GameObject newPortal = Instantiate(PortalPrefab, spawnPos, Quaternion.identity);
            newPortal.GetComponent<SummonPoint>().InitiateSummon(BatPrefab);
            enemyList.Add(newPortal);
            //Debug.Log(pullText.Substring(indexPull,indexNum-indexPull));
        } else {
            return false; //operation failed
        }
        return true; //operation successful
    }

    public bool SpawnEnemy(Vector3 spawnPos, string enemyType, float spawnDelay) {
        if(enemyType.Equals("bat", StringComparison.OrdinalIgnoreCase)) { //https://www.delftstack.com/howto/csharp/compare-two-strings-ignoring-case-in-csharp/
            GameObject newPortal = Instantiate(PortalPrefab, spawnPos, Quaternion.identity);
            newPortal.GetComponent<SummonPoint>().InitiateSummon(BatPrefab, spawnDelay);
            enemyList.Add(newPortal);
            //Debug.Log(pullText.Substring(indexPull,indexNum-indexPull));
        } else {
            return false; //operation failed
        }
        return true; //operation successful
    }

    public void ClearEnemies() { //Removes all enemies and portals in enemyList
        for (int i = enemyList.Count-1; i >= 0; i--) {
            Destroy(enemyList[i]);
            enemyList.RemoveAt(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
