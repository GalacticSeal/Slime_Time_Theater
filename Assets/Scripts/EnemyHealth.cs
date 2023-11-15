using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hitpoints = 1;
    public bool defaultBehavior = true;

    public void ReceiveDamage() {
        hitpoints--;
        CheckThreshold();
    }

    public void ReceiveDamage(int damageAmount) {
        hitpoints -= damageAmount;
        CheckThreshold();
    }

    public void CheckThreshold() {
        if(defaultBehavior && hitpoints <= 0) {
            Death();
        }
    }

    public void Death() {
        GameObject Spawner = GameObject.FindWithTag("MobSpawner");
        Spawner.GetComponent<EnemySpawner>().DeleteEnemy(gameObject);
    }
}
