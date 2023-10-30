using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float plVelocityX = 0f;
    private float plVelocityY = 0f;

    private float plSpeed = 1f;
    private float gravity = 2f;

    private Vector2 playerSize;
    private LayerMask terrainMask;
    private RaycastHit2D[] hitArray = new RaycastHit2D[2];

    private BoxCollider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        playerSize = hitbox.bounds.size;
        terrainMask = LayerMask.GetMask("Terrain");
    }

    void FixedUpdate()
    {
        //Detect all current terrain collisions
        hitArray = Physics2D.BoxCastAll(hitbox.bounds.center, playerSize, 0f, new Vector2(0f,0f), playerSize.x/2f, terrainMask, 0f, playerSize.x/2f);
        for(int i = 0; i < hitArray.Length; i++) {
            Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Normal -> " + hitArray[i].normal);

            //Check for vertical collision data
            Vector2 contactPoint = hitArray[i].point;
            switch((int) Mathf.Ceil(hitArray[i].normal.y)) { //Positive y on normal means the collision was below due to the normal being outward from the collision point
                case -1:
                    transform.position = new Vector2(transform.position.x, contactPoint.y-playerSize.y/2f); //snap to ceiling upon collision
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                    break;
                case 1:
                    transform.position = new Vector2(transform.position.x, contactPoint.y+playerSize.y/2f); //snap to floor opon collision
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                    break;
                default:
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Vertical Hit");
                    break;
            }

            //Check for horizontal collision data
            switch((int) Mathf.Ceil(hitArray[i].normal.x)) { //Positive x on normal means the collision was from the left . . . this becomes more confusing than verticals
                case -1:
                    transform.position = new Vector2(contactPoint.x-playerSize.x/2f, transform.position.y); //snap to ceiling upon collision
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                    break;
                case 1:
                    transform.position = new Vector2(contactPoint.x+playerSize.x/2f, transform.position.y); //snap to floor opon collision
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                    break;
                default:
                    Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Horizontal Hit");
                    break;
            }
        }
        //more code here
    }
}
