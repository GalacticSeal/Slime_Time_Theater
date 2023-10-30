using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private LayerMask playerMask;
    private RaycastHit2D playerHit;
    private BoxCollider2D hitbox;
    
    private void Start() {
        hitbox = GetComponent<BoxCollider2D>();
        playerMask = LayerMask.GetMask("Player");
    }

    void Update()
    {
        playerHit = Physics2D.BoxCast(hitbox.bounds.center, hitbox.bounds.size, 0f, new Vector2(0f,0f), hitbox.bounds.size.x/2f, playerMask, -0.1f, 0.1f);
        if(playerHit.collider != null) {
            playerHit.collider.GetComponent<PlayerMovement>().Respawn();
        }
    }
}
