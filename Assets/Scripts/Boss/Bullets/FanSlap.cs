using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSlap : MonoBehaviour
{


    public float rotSpeed;
    public float FadeSpd;
    public bool right;
    public bool reverse;

    public float fadeStart;

    public SpriteRenderer sprite;
    public TrailRenderer trail;

    void Start()
    {
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        while(sprite.color.a >0.0f)
        {
            if(reverse)
            transform.Rotate(Vector3.forward * (rotSpeed * Time.deltaTime));
            else
            transform.Rotate(Vector3.forward * (-rotSpeed * Time.deltaTime));

            if (fadeStart < 0.0f)
            {
                sprite.color = new Color(1f, 1f, 1f, sprite.color.a - FadeSpd * Time.deltaTime);
                trail.startColor = new Color(1f, 1f, 1f, trail.startColor.a - FadeSpd * Time.deltaTime);
                trail.endColor = new Color(1f, 1f, 1f, trail.endColor.a - FadeSpd * Time.deltaTime);
            }
            fadeStart -= Time.deltaTime;
            yield return 0;

        }

        Destroy(this.gameObject, 0.5f);
        yield return 0;
    }
}
