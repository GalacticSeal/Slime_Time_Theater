using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandAiming : MonoBehaviour
{
    public bool allowAttack = true;
    private bool isFiring = false;

    public List<GameObject> projList = new List<GameObject>();
    public GameObject ProjPrefab;

    //sbyte is used instead of int because the ranges of values is so small for these specific numbers
    private sbyte delayCount = 0; //would use framecount or time for efficiency, but WebGL works differently from the desktop build, making this method a safer choice
    private static sbyte attackDelay = 29; //these two variables will be used to keep track of the delay between each attack, with the static sbyte being the minimum delay between attacks

    private static float displacement = 0f;

    private void Update() {
        isFiring = Input.GetMouseButton(0); //isFiring is true when left click is held or pressed down

        //rotation code from: https://forum.unity.com/threads/2d-character-rotation-towards-mouse.457126/
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void FixedUpdate() {
        if(delayCount <= 0) {
            if(isFiring && allowAttack) {
                delayCount = attackDelay;
                GameObject newProj = Instantiate(ProjPrefab, transform.position+transform.up*displacement, transform.rotation); //will send projectile in direction of wand
                projList.Add(newProj);
            }
        } else {
            delayCount--;
        }
    }

    public void ClearProj() { //Removes all projectiles in projList
        for (int i = projList.Count-1; i >= 0; i--) {
            Destroy(projList[i]);
            projList.RemoveAt(i);
        }
    }

    public void DeleteProj(GameObject proj) {
        projList.Remove(proj); //removes from list before deleting GameObject to avoid null error
        Destroy(proj);
    }
}
