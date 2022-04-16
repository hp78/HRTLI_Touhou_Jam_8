using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanLaser : MonoBehaviour
{

    public Laser[] lasers;
    public Transform player;
   
    public float delayTime;
    public float rotateSpeed;
    public float decel;
 


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        while (rotateSpeed > 1.0f)
        {
            float angle = Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle -90f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            rotateSpeed -= decel*Time.deltaTime*rotateSpeed;

            yield return 0;
        }
        yield return new WaitForSeconds(delayTime);

        foreach (Laser l in lasers) l.ManualFire();
        Destroy(this.gameObject, 1f);

    }
}
