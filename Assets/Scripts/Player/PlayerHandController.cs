using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public bool isLeftHand = false;
    List<SpriteRenderer> weaponSprites = new List<SpriteRenderer>();
    public int backhandSortOrder = 7;
    public int fronthandSortOrder = 13;

    // Gun shit
    public Transform topRoot;
    public GameObject pfGunAttack;
    public GameObject shootSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        weaponSprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    public void FlipLeft()
    {
        //transform.localScale = new Vector3(1, -1, 1);
        //transform.localRotation = Quaternion.Euler(0, 0, -75);
        //
        //foreach (SpriteRenderer sr in weaponSprites)
        //{
        //    if (isLeftHand)
        //        sr.sortingOrder = fronthandSortOrder;
        //    else
        //        sr.sortingOrder = backhandSortOrder;
        //}
    }

    public void FlipRight()
    {
        //transform.localScale = new Vector3(1, -1, 1);
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        //
        //foreach (SpriteRenderer sr in weaponSprites)
        //{
        //    if (isLeftHand)
        //        sr.sortingOrder = backhandSortOrder;
        //    else
        //        sr.sortingOrder = fronthandSortOrder;
        //}
    }

    public void SwitchWeapon(int weaponIndex)
    {

    }

    public void Shoot()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfGunAttack, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfGunAttack, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
