using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanAtk4 : AtkBase
{

    [SerializeField] Transform[] rollPoints;
    [SerializeField] float maxspd;
    [SerializeField] int point;

    public GameObject missle;

    public float spawnCD;
    public float upgradeCD;
    public int upgradeAmt;
    public int spawnAmt;

    float internalSpawnCD;
    int internalAmt;


    public GameObject ranWoke;
    public GameObject ranNormal;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();
    }

    public override void StartSkill()
    {
        StartCoroutine(Fire());
        StartCoroutine(Move());

    }
    public override void UpgradeSkill()
    {
        spawnCD -= upgradeCD;
        spawnAmt += upgradeAmt;
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
            Instantiate(missle, transform.position, Quaternion.identity);
            internalSpawnCD = spawnCD;

            internalAmt--;
            yield return 0;

        }


        yield return 0;
    }

    IEnumerator Move()
    {
        animator.Play("Spin");
        ranNormal.SetActive(false);
        while ((transform.position - rollPoints[point].position).magnitude < 0.1f)
            point = Random.Range(0, rollPoints.Length);

        while ((transform.position - rollPoints[point].position).magnitude > 0.1f)
        {
            float step = maxspd * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, rollPoints[point].position, step);
            yield return 0;
        }


        animator.Play("Default");
        ranNormal.SetActive(true);
        yield return 0;
        bossBase.AtkIsDone();

    }

}
