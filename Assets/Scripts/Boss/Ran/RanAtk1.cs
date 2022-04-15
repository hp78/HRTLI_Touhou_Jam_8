using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanAtk1 : AtkBase
{
    [SerializeField] Transform[] rollPoints;
    [SerializeField] int point;
    [SerializeField] float maxspd;
    [SerializeField] float accel;
    [SerializeField] float skillCD;
    [SerializeField] float waitCD;

    float internalCD;
    float currentSpd;

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
        StartCoroutine(SpinAttack());

    }

    IEnumerator SpinAttack()
    {
        point = 0;
        while (point < rollPoints.Length)
        {
            while ((transform.position - rollPoints[point].position).magnitude > 0.1f)
            {
                float step = maxspd * Time.deltaTime;

                // move sprite towards the target location
                transform.position = Vector2.MoveTowards(transform.position, rollPoints[point].position, step);
                yield return 0;
            }
            ++point;
            yield return new WaitForSeconds(waitCD);

        }

        yield return new WaitForSeconds(skillCD);

        bossBase.AtkIsDone();

        yield return 0;
    }
}
