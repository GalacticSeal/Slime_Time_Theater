using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    private float batSpeed = 0.05f;
    private GameObject Player;
    private Vector2 direction;

    private Vector2 batSize;
    private LayerMask terrainMask;
    private RaycastHit2D[] hitArray = new RaycastHit2D[2];
    private BoxCollider2D hitbox;
    

    private void Start() {
        Player = GameObject.FindWithTag("Player");
        hitbox = GetComponent<BoxCollider2D>();
        batSize = hitbox.bounds.size;
        terrainMask = LayerMask.GetMask("Terrain");
    }

    private void FixedUpdate() {
        direction = new Vector2(Player.transform.position.x-transform.position.x, Player.transform.position.y-transform.position.y);
        direction.Normalize();
        transform.position += (Vector3) direction*batSpeed;

        hitArray = Physics2D.BoxCastAll(hitbox.bounds.center, batSize, 0f, new Vector2(0f,0f), batSize.x/2f, terrainMask, -0.1f, 0.1f);
        for(int i = 0; i < hitArray.Length; i++) {
            //Check vertical collision data
            Vector2 contactPoint = hitArray[i].point;
            switch((int) Mathf.Ceil(hitArray[i].normal.y)) { //Positive y on normal means the collision was below due to the normal being outward from the collision point
                case -1:
                    if(direction.y > 0) { //bat needs to be moving up for this collision
                        transform.position = new Vector2(transform.position.x, contactPoint.y-batSize.y/2f); //snap to ceiling upon collision
                    }
                    break;
                case 1:
                    if(direction.y < 0) { //bat needs to be moving down for this collision
                        transform.position = new Vector2(transform.position.x, contactPoint.y+batSize.y/2f); //snap to floor opon collision
                    }
                    break;
                default:
                    break;
            }
            switch((int) Mathf.Ceil(hitArray[i].normal.x)) { //Positive x on normal means the collision was from the left . . . this becomes more confusing than verticals
                case -1:
                    if(direction.x > 0) { //bat needs to be moving up for this collision
                        transform.position = new Vector2(contactPoint.x-batSize.x/2f, transform.position.y); //snap to left wall upon collision
                    }
                    break;
                case 1:
                    if(direction.x < 0) { //bat needs to be moving down for this collision
                        transform.position = new Vector2(contactPoint.x+batSize.x/2f, transform.position.y); //snap to right wall upon collision
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
