using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanAtk2 : AtkBase
{

    public Transform[] fireSpots;
    public GameObject laser;
    public GameObject target;

    public float targetMovement;
    public float duration;

    public float spawnCD;
    public int spawnAmt;

    bool invert;
    float internalSpawnCD;
    float internalDuration;
    int internalAmt;
    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();

    }

    public override void StartSkill()
    {
        StartCoroutine(Fire());


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        target.transform.position = transform.position;
        int rand = Random.Range(0, fireSpots.Length);
        Vector3 direction = (fireSpots[rand].position - transform.position).normalized; 

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        internalDuration = duration;
        internalAmt = spawnAmt;
        while (internalDuration > 0f)
        {
            internalDuration -= Time.deltaTime;
            internalSpawnCD -= Time.deltaTime;

            target.transform.position = target.transform.position + (direction * targetMovement * Time.deltaTime);

            if (internalSpawnCD < 0.0f && internalAmt > 0)
            {
                float randomAngle = Random.Range(-45f, 45f);
                if (invert)
                {
                    Instantiate(laser, target.transform.position, target.transform.rotation * Quaternion.Euler(0f, 0f, 90f+randomAngle));
                    invert = !invert;
                }
                else 
                {
                    Instantiate(laser, target.transform.position, target.transform.rotation * Quaternion.Euler(0f, 0f, -90f+randomAngle));
                    invert = !invert;

                }
                internalSpawnCD = spawnCD + Random.Range(-spawnCD/2,spawnCD/2);
                --internalAmt;
            }
            yield return 0;

        }
        bossBase.AtkIsDone();

        yield return 0;
    }
}
