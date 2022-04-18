using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanAtk1 : AtkBase
{
    [SerializeField] Transform[] rollPoints;
    [SerializeField] int point;
    [SerializeField] float maxspd;
    [SerializeField] float accel;
    [SerializeField] float waitCD;


    public GameObject ranSprite;
    public Animator animator;
    public CircleCollider2D spinCol;

    float internalCD;
    float currentSpd;
    bool reverse;

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

    public override void UpgradeSkill()
    {
        maxspd += accel;
        waitCD /= 2f;
    }

    IEnumerator SpinAttack()
    {
        animator.Play("Spin");
        ranSprite.SetActive(false);
        yield return new WaitForSeconds(.6f);

        spinCol.enabled = true;
        if (!reverse)
        {
            point = 0;
            var temp = Random.Range(0, 2);
            if (temp == 1) point = rollPoints.Length - 1;
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
        }
        else
        {
            point = rollPoints.Length - 1;
            var temp = Random.Range(0, 2);
            if (temp == 1) point = 0;

            while (point >= 0)
            {
                while ((transform.position - rollPoints[point].position).magnitude > 0.1f)
                {
                    float step = maxspd * Time.deltaTime;

                    // move sprite towards the target location
                    transform.position = Vector2.MoveTowards(transform.position, rollPoints[point].position, step);
                    yield return 0;
                }
                --point;
                yield return new WaitForSeconds(waitCD);

            }
        }

        reverse = !reverse;
        animator.Play("Default");
        ranSprite.SetActive(true);
        spinCol.enabled = false;

        bossBase.AtkIsDone();

        yield return 0;
    }

   
}
