using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public float growSpeed;
    public float fadeSpeed;
    public float maxHeight;
    public SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());

    }

    // Update is called once per frame
    IEnumerator Fire()
    {
        while(transform.localScale.y < maxHeight)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + growSpeed * Time.deltaTime, transform.localScale.z);
            if (sprite.color.a < 1.0f)
                sprite.color = new Color(1f, 1f, 1f, sprite.color.a + fadeSpeed * Time.deltaTime);

            yield return 0;
        }

        while (transform.localScale.y > 0.1f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - growSpeed * Time.deltaTime, transform.localScale.z);
            if (sprite.color.a > 0.0f)
                sprite.color = new Color(1f, 1f, 1f, sprite.color.a - fadeSpeed * Time.deltaTime);
            yield return 0;
        }

        Destroy(this.gameObject, 0.5f);
        yield return 0;
    }
}
