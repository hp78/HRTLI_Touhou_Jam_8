using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkMeleeSlap : AtkBase
{
    public Transform fans;
    public Transform player;
    public float moveSpd;

    public float spawnCD;
    public float upgradeCD;
    public int upgradeAmt;
    public int spawnAmt;

    bool invert;
    bool flip;
    float internalSpawnCD;
    float internalDuration;
    int internalAmt;
    // Start is called before the first frame update
    void Start()
    {
        bossBase = GetComponent<BossBase>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Move());

    }

    public override void StartSkill()
    {
        StartCoroutine(Move());

    }

    IEnumerator Fire()
    {

        internalAmt = spawnAmt;
        while (internalAmt > 0f)
        {

            internalSpawnCD -= Time.deltaTime;

            if (internalSpawnCD < 0.0f)
            {
                FanSlap temp;
                if (invert)
                {
                    if(flip)
                        temp = Instantiate(fans, new Vector2(transform.position.x+1f, transform.position.y + 1.25f), transform.rotation * Quaternion.Euler(0f, 180f, 0f)).GetComponent<FanSlap>();
                    else
                        temp = Instantiate(fans, new Vector2(transform.position.x-1f, transform.position.y + 1.25f), transform.rotation * Quaternion.Euler(0f, 0f, 0f)).GetComponent<FanSlap>();

                    temp.reverse = true;
                    invert = !invert;
                }
                else
                {
                    if(flip)
                        temp = Instantiate(fans, new Vector2(transform.position.x+1f, transform.position.y + 1.25f), transform.rotation * Quaternion.Euler(0f, 180f, 180f)).GetComponent<FanSlap>();
                    else
                        temp = Instantiate(fans, new Vector2(transform.position.x-1f, transform.position.y + 1.25f), transform.rotation * Quaternion.Euler(0f, 0f, 180f)).GetComponent<FanSlap>();

                    temp.reverse = false;
                    invert = !invert;

                }
                --internalAmt;

                while ((transform.position - player.position).magnitude > 2.5f)
                {
                    float step = moveSpd * Time.deltaTime;
                    if (transform.position.x - player.position.x < 0.0f) flip = true;
                    else flip = false;
                    // move sprite towards the target location
                    transform.position = Vector2.MoveTowards(transform.position, player.position, step);
                    yield return 0;
                }
                internalSpawnCD = spawnCD;

            }
            yield return 0;

        }
        // bossBase.AtkIsDone();
        StartCoroutine(Move());


        yield return 0;
    }

    IEnumerator Move()
    {
        while ((transform.position - player.position).magnitude > 2.5f)
        {
            float step = moveSpd * Time.deltaTime;
            if (transform.position.x - player.position.x < 0.0f) flip = true;
           else flip = false;
            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, player.position, step);
            yield return 0;
        }
        StartCoroutine(Fire());
        yield return 0;

    }
}
