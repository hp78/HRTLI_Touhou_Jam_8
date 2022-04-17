using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkHomingFans : AtkBase
{
    [SerializeField] Transform[] rollPoints;
    [SerializeField] float maxspd;
    [SerializeField] int point;

    public GameObject fans;

    public float spawnCD;
    public float upgradeCD;
    public int upgradeAmt;
    public int spawnAmt;

    float internalSpawnCD;
    int internalAmt;


    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();
        StartSkill();
    }

    public override void StartSkill()
    {
        StartCoroutine(Fire());
        StartCoroutine(Move());

    }

    IEnumerator Fire()
    {
        internalAmt = spawnAmt;
        internalSpawnCD = spawnCD;

        while (internalAmt > 0)
        {
            while (internalSpawnCD > 0.0f)
            {
                internalSpawnCD -= Time.deltaTime;
                yield return 0;
            }
            var fan = Instantiate(fans, transform.position, Quaternion.identity).GetComponent<HomingFan>();
            fan.firePoint = new Vector3(transform.position.x + Random.Range(-20f, 20f), transform.position.y + Random.Range(-5f, 5f), 0.0f);
            internalSpawnCD = spawnCD;

            internalAmt--;
            yield return 0;

        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fire());

        yield return 0;
    }

    IEnumerator Move()
    {
        while ((transform.position - rollPoints[point].position).magnitude < 0.1f)
            point = Random.Range(0, rollPoints.Length);

        while ((transform.position - rollPoints[point].position).magnitude > 0.1f)
        {
            float step = maxspd * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, rollPoints[point].position, step);
            yield return 0;
        }


        yield return 0;
        // bossBase.AtkIsDone();

    }
}
