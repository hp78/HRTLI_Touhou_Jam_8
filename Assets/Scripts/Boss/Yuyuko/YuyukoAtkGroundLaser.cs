using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkGroundLaser : AtkBase
{
    public float moveSpeed;
    public Transform groundlaser;
    public Transform groundlaserGroup;
    public Transform movePt;

    public float spawnCD;
    public int spawnAmt;

    bool spawnExtra;


    public float trackSpeed;
    public float delay;
    float internalSpawnCD;
    int internalAmt;

    public GameObject particles;

    public GameObject yykWoke;
    public GameObject yykNormal;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            //Instantiate(fanlaser, transform.position, Quaternion.identity);
            internalSpawnCD = spawnCD;

            internalAmt--;
            yield return 0;

        }

        // yield return new WaitForSeconds(2f);
        particles.SetActive(false);
        animator.Play("PoP");
        yykNormal.SetActive(true);
        yykWoke.SetActive(false);
        StartCoroutine(Fire());
        bossBase.AtkIsDone();

        yield return 0;

    }
}
