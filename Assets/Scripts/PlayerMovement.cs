using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float plVelocityX = 0f;
    private float plVelocityY = 0f;

    private float spawnPosX;
    private float spawnPosY;

    private bool isGrounded = true;
    private bool isJumping = false; //determines if player inputs to jump on next FixedUpdate - does not actually determine whether they are jumping or not
    private bool allowInput = true; //determines whether the player can control their character at the given time, but does not affect physics simulation
    private bool stasis = false; //for temporarily putting the player's movement in a stasis where they can't move

    private float plSpeed = 0.1f;
    private float plAirFactor = 0.2f; //decreases aerial control
    private float plJumpForce = 0.2f;
    private float gravity = 0.01f;

    private Vector2 playerSize;
    private LayerMask terrainMask;
    private RaycastHit2D[] hitArray = new RaycastHit2D[2];

    private BoxCollider2D hitbox;

    public void Respawn() {
        transform.position = new Vector2(spawnPosX,spawnPosY);
        plVelocityX = 0f;
        plVelocityY = 0f;
        isGrounded = true;
        isJumping = false;
        allowInput = true;
        stasis = false;
    }

    public void SetSpawn(float xPos, float yPos) {
        spawnPosX = xPos;
        spawnPosY = yPos;
    }

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        playerSize = hitbox.bounds.size;
        terrainMask = LayerMask.GetMask("Terrain");

        spawnPosX = transform.position.x;
        spawnPosY = transform.position.y;
    }

    void Update() {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) { //detects it player is trying to jump for next FixedUpdate
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        if(!stasis) {
            plVelocityY -= gravity; //constant gravity

            if(isGrounded && allowInput) {
                //Horizontal Movement
                plVelocityX = Input.GetAxisRaw("Horizontal")*plSpeed;

                //Jumping
                if(isJumping) {
                    plVelocityY = plJumpForce;
                    isJumping = false;
                    isGrounded = false;
                }
            } else {
                //limits horizontal air mobility, but does not low top speed from grounded state
                plVelocityX += Input.GetAxisRaw("Horizontal")*plSpeed*plAirFactor;
                plVelocityX = Mathf.Clamp(plVelocityX, -plSpeed, plSpeed);
                isJumping = false;
            }

            transform.position = new Vector2(transform.position.x+plVelocityX, transform.position.y+plVelocityY);

            //Detect all current terrain collisions
            hitArray = Physics2D.BoxCastAll(hitbox.bounds.center, playerSize, 0f, new Vector2(0f,0f), playerSize.x/2f, terrainMask, -0.1f, 0.1f);
            for(int i = 0; i < hitArray.Length; i++) {
                //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Normal -> " + hitArray[i].normal);

                isGrounded = false; //for indicating the player is not grounded if no collision below the player is detected

                //Check for vertical collision data
                Vector2 contactPoint = hitArray[i].point;
                switch((int) Mathf.Ceil(hitArray[i].normal.y)) { //Positive y on normal means the collision was below due to the normal being outward from the collision point
                    case -1:
                        if(plVelocityY > 0) { //player needs to be moving up for this collision
                            plVelocityY = 0;
                            transform.position = new Vector2(transform.position.x, contactPoint.y-playerSize.y/2f); //snap to ceiling upon collision
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                            break;
                        } else {
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Vertical Hit");
                            break;
                        }
                    case 1:
                        if(plVelocityY < 0) { //player needs to be moving down for this collision
                            plVelocityY = 0;
                            transform.position = new Vector2(transform.position.x, contactPoint.y+playerSize.y/2f); //snap to floor opon collision
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                            Landed();
                            break;
                        } else {
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Vertical Hit");
                            break;
                        }
                    default:
                        //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Vertical Hit");
                        break;
                }

                //Check for horizontal collision data
                switch((int) Mathf.Ceil(hitArray[i].normal.x)) { //Positive x on normal means the collision was from the left . . . this becomes more confusing than verticals
                    case -1:
                        if(plVelocityX > 0) { //player needs to be moving right for this collision
                            plVelocityX = 0;
                            transform.position = new Vector2(contactPoint.x-playerSize.x/2f, transform.position.y); //snap to ceiling upon collision
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                            break;
                        } else {
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Horizontal Hit");
                            break;
                        }
                    case 1:
                        if(plVelocityX < 0) { //player needs to be moving left for this collision
                            plVelocityX = 0;
                            transform.position = new Vector2(contactPoint.x+playerSize.x/2f, transform.position.y); //snap to floor opon collision
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Snapped Player To " + transform.position);
                            break;
                        } else {
                            //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Horizontal Hit");
                            break;
                        }
                    default:
                        //Debug.Log("Player BoxCastAll: Collision " + (i+1) + " Not A Horizontal Hit");
                        break;
                }
            }
        }
    }

    private void Landed() { //reset grounded state
        isGrounded = true;
        plVelocityY = 0f;
    }
}
