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

    public GameObject ranWoke;
    public GameObject ranNormal;
    public Animator animator;
    public GameObject particles;
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

        animator.Play("PoP");
        ranNormal.SetActive(false);
        ranWoke.SetActive(true);
        particles.SetActive(true);
        internalAmt = spawnAmt;
        internalSpawnCD = spawnCD;

        while(internalAmt >0)
        {
            while (internalSpawnCD > 0.0f)
            {
                internalSpawnCD -= Time.deltaTime;
                yield return 0;
            }
            Vector3 pos = new Vector3(Random.Range(-20f,20f), groundLevel.position.y, 0.0f);
            Instantiate(beam, pos, Quaternion.identity);
            internalSpawnCD = spawnCD;
               
            internalAmt--;
            yield return 0;

        }
        animator.Play("Default");
        ranNormal.SetActive(true);
        ranWoke.SetActive(false);
        particles.SetActive(false);

        bossBase.AtkIsDone();
        yield return 0;

    }
}
