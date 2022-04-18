using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkWave : AtkBase
{
    public Transform[] points;
    public Transform target;
    public Transform wave;
    public float moveSpd;

    public Transform ground;
    public float increment;
    float spawnpoint;

    public float spawnCD;
    public float upgradeCD;
    public int upgradeAmt;
    public int spawnAmt;

    public int spawnWaveAmt;
    public float spawnWaveCD;
    int internalspawnWaveAmt;
    float internalspawnWaveCD;

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
        StartCoroutine(Move());

    }

    IEnumerator Move()
    {
        int rand = Random.Range(0, 2);
        while ((transform.position - points[rand].position).magnitude > 0.1f)
        {
            float step = moveSpd * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, points[rand].position, step);
            yield return 0;
        }
        StartCoroutine(Fire());
        yield return 0;

    }

    IEnumerator Fire()
    {
        internalspawnWaveAmt = spawnWaveAmt;

        while (internalspawnWaveAmt >0)
        {
            internalspawnWaveCD -= Time.deltaTime;
            if (internalspawnWaveCD < 0f)
            {
                spawnpoint = 0f;
                internalAmt = spawnAmt;


                while (internalAmt > 0f)
                {
                    internalSpawnCD -= Time.deltaTime;
                    if (internalSpawnCD < 0.0f)
                    {
                        Vector3 spawnpt = new Vector3(transform.position.x + spawnpoint, ground.position.y, 0.0f);
                        Instantiate(wave, spawnpt, Quaternion.identity);

                        if (transform.position.x - target.position.x > 0f)
                            spawnpoint -= increment;
                        else
                            spawnpoint += increment;

                        internalSpawnCD = spawnCD;
                        --internalAmt;

                    }
                    yield return 0;
                }
                internalspawnWaveCD = spawnWaveCD;
                --internalspawnWaveAmt;

            }

            yield return 0;

        }
        bossBase.AtkIsDone();


        yield return 0;
    }


}
