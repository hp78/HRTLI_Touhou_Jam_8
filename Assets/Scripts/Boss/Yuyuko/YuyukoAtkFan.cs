using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkFan : AtkBase
{

    public float moveSpeed;
    public Transform fanlaser;
    public Transform movePt;

    public float spawnCD;
    public float upgradeCD;
    public int upgradeAmt;
    public int spawnAmt;


    public GameObject particles;
    float internalSpawnCD;
    int internalAmt;

    public GameObject yykWoke;
    public GameObject yykNormal;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartSkill()
    {
        StartCoroutine(Move());

    }
    public override void UpgradeSkill()
    {
        spawnCD -= upgradeCD;
        spawnAmt += upgradeAmt;
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
        animator.Play("PoP");
        yykNormal.SetActive(false);
        yykWoke.SetActive(true);
        particles.SetActive(true);

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


        particles.SetActive(false);
        animator.Play("Default");
        yykNormal.SetActive(true);
        yykWoke.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        bossBase.AtkIsDone();

        yield return 0;

    }
}
