using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkFan : AtkBase
{

    public float moveSpeed;
    public Transform fanlaser;
    public Transform movePt;

    public float spawnCD;
    public int spawnAmt;


    public GameObject particles;
    float internalSpawnCD;
    int internalAmt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartSkill()
    {
        StartCoroutine(Move());

    }

    IEnumerator Move()
    {
        while ((transform.position - movePt.position).magnitude > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, movePt.position, step);
            yield return 0;
        }
        StartCoroutine(Fire());

    }

    IEnumerator Fire()
    {

        internalAmt = spawnAmt;
        internalSpawnCD = spawnCD;
        particles.SetActive(true);
        while (internalAmt > 0)
        {
            while (internalSpawnCD > 0.0f)
            {
                internalSpawnCD -= Time.deltaTime;
                yield return 0;
            }
            
            Instantiate(fanlaser, transform.position, Quaternion.identity);
            internalSpawnCD = spawnCD;

            internalAmt--;
            yield return 0;

        }

        // bossBase.AtkIsDone();
        // yield return new WaitForSeconds(2f);
        particles.SetActive(false);

        StartCoroutine(Fire());

        yield return 0;

    }
}
