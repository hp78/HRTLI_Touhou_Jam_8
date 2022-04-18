using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingFan : MonoBehaviour
{

    public Transform player;
    public Transform ground;
    public SpriteRenderer sprite;
    public CircleCollider2D col;

    public float startSpeed;
    public float accel;
    public float decel;

    public Vector3 firePoint;
    public float fadeTime;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ground = GameObject.FindGameObjectWithTag("Ground").transform;
        StartCoroutine(Fire());
        anim.speed = 0.25f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Fire()
    {
        while (startSpeed > 0.0f)
        {
            if ((transform.position - firePoint).magnitude > 0.1f)
            {
                float step = startSpeed * Time.deltaTime;

                // move sprite towards the target location
                transform.position = Vector2.MoveTowards(transform.position, firePoint, step);
            }
            startSpeed -= decel *Time.deltaTime;
            
            anim.speed += 0.4f*Time.deltaTime;

            yield return 0;

        }

        Vector3 point = new Vector3(player.position.x, player.position.y,0.0f);
        while((transform.position - point).magnitude>0.01f)
        {
            float step = startSpeed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, point, step);
            startSpeed += accel * Time.deltaTime;

            yield return 0;
        }

        anim.speed= 0f;
        col.enabled = false;
        while(sprite.color.a >0.0f)
        {
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - fadeTime * Time.deltaTime);
            yield return 0;

        }
        Destroy(this.gameObject, 0.5f);
        yield return 0;

    }


}
