using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPoint : MonoBehaviour
{
    static private float defaultDelay = 2f; //default summon delay

    private float portalSize = 1f;
    private float startTime;
    private float summonDelay = 0;
    private bool isSummoning = false;

    private GameObject Mob;
    private GameObject Spawner;
    private SpriteRenderer sprite;


    public void InitiateSummon(GameObject Enemy, float delay, float portalScale) {
        isSummoning = true;
        Mob = Enemy;
        summonDelay = delay;
        startTime = Time.time;
        portalSize = portalScale;
    }

    public void InitiateSummon(GameObject Enemy, float delay) { //allows for use of default portal size if no size is provided
        isSummoning = true;
        Mob = Enemy;
        summonDelay = delay;
        startTime = Time.time;
    }

    public void InitiateSummon(GameObject Enemy) { //option to simply use preset defaults
        isSummoning = true;
        Mob = Enemy;
        summonDelay = defaultDelay;
        startTime = Time.time;
    }


    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        Spawner = GameObject.FindWithTag("MobSpawner");
        sprite.enabled = false;
    }

    void FixedUpdate() {
        if(isSummoning) {
            //Debug.Log(Time.time + " | " + startTime+summonDelay);
            if(Time.time < startTime+summonDelay) {
                float scaling = portalSize-portalSize*(Time.time-startTime)/summonDelay; //shrink portal size as it nears the summon time
                transform.localScale = new Vector3(scaling, scaling, 1);
            } else {
                GameObject newEnemy = Instantiate(Mob, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity); //destroy portal and summon enemy
                Spawner.GetComponent<EnemySpawner>().enemyList.Add(newEnemy);
                Spawner.GetComponent<EnemySpawner>().DeleteEnemy(gameObject);
            }
        }
    }
    
    void Update() {
        if(isSummoning) {
            sprite.enabled = !sprite.enabled; //causes flickering effect of portal
        } else {
            sprite.enabled = false;
        }
    }
}
