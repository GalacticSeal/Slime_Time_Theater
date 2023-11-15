using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private LayerMask terrainMask, enemyMask;
    private CircleCollider2D hitbox;

    private float projSpeed = 0.1f;

    private void Start() {
        hitbox = GetComponent<CircleCollider2D>();
        terrainMask = LayerMask.GetMask("Terrain");
        enemyMask = LayerMask.GetMask("Enemy");
    }

    private void FixedUpdate() {
        transform.position += transform.up*projSpeed;

        RaycastHit2D hit = Physics2D.CircleCast(hitbox.bounds.center, hitbox.radius, transform.up, hitbox.radius, enemyMask, -0.1f, 0.1f);
        if(hit.collider != null) {
            hit.collider.GetComponent<EnemyHealth>().ReceiveDamage();
            GameObject Wand = GameObject.FindWithTag("PlayerWand");
            Wand.GetComponent<WandAiming>().DeleteProj(gameObject);
        }
        hit = Physics2D.CircleCast(hitbox.bounds.center, hitbox.radius/2, transform.up, hitbox.radius/2, terrainMask, -0.1f, 0.1f);
        if(hit.collider != null) {
            GameObject Wand = GameObject.FindWithTag("PlayerWand");
            Wand.GetComponent<WandAiming>().DeleteProj(gameObject);
        }
    }
}
