using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkGroundLaser : AtkBase
{
    public float moveSpeed;
    public Transform groundlaser;
    public Transform groundlaserGroup;
    public Transform movePt;

    public Transform player;
    public Transform ground;

    public float spawnCD;
    public int spawnAmt;

    public float upgradeCD;
    public float upgradeMove;
    public int upgradeAmt;

    public bool spawnExtra;

    public float trackSpeed;
    public float trackTime;

    float internalSpawnCD;
    float internaltrackTime;

    int internalAmt;

    public GameObject particles;
    public GameObject yykWoke;
    public GameObject yykNormal;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
       
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
        trackSpeed += upgradeMove;
        trackTime = 2f;
        spawnExtra = true;
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
        internaltrackTime = trackTime;

        while (internalAmt > 0)
        {
            while (internalSpawnCD > 0.0f)
            {
                internalSpawnCD -= Time.deltaTime;
                yield return 0;
            }
            Transform temp;

             if (!spawnExtra)
             temp = Instantiate(groundlaser, transform.position, Quaternion.identity).transform;
             else
             temp = Instantiate(groundlaserGroup, transform.position, Quaternion.identity).transform;

            Vector3 point = new Vector3(player.position.x, ground.position.y + 1f, 0.0f);
            while (internaltrackTime > 0.0f)
            {
 
                point = new Vector3(player.position.x, ground.position.y + 1f, 0.0f);
                float step = trackSpeed * Time.deltaTime;

                temp.transform.position = Vector2.MoveTowards(temp.transform.position, point, step);

                internaltrackTime -= Time.deltaTime;
                yield return 0;

            }

            internalSpawnCD = spawnCD;
            internaltrackTime = trackTime;
            internalAmt--;
            yield return 0;

        }

        // yield return new WaitForSeconds(2f);
        particles.SetActive(false);
        animator.Play("Default");
        yykNormal.SetActive(true);
        yykWoke.SetActive(false);

        bossBase.AtkIsDone();

        yield return 0;

    }
}
