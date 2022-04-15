using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanAtk3 : AtkBase
{
    public GameObject beam;
    public Transform groundLevel;
    public Transform player;

    public float spawnCD;
    public int spawnAmt;

    float internalSpawnCD;
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
        internalAmt = spawnAmt;
        internalSpawnCD = spawnCD;

        while(internalAmt >0)
        {
            while (internalSpawnCD > 0.0f)
            {
                internalSpawnCD -= Time.deltaTime;
                yield return 0;
            }
            Vector3 pos = new Vector3(Random.Range(-25f,25f), groundLevel.position.y, 0.0f);
            Instantiate(beam, pos, Quaternion.identity);
            internalSpawnCD = spawnCD;
               
            internalAmt--;
            yield return 0;

        }

        // bossBase.AtkIsDone();
        yield return new WaitForSeconds(2f);
        StartCoroutine(Fire());

        yield return 0;

    }
}
