using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform sprite;
    public SpriteRenderer spriteR;
    public BoxCollider2D boxCol;

    public float maxSize;
    public float growthSpd;
    public float fadeSpd;
    public AudioSource audioS;

    // Start is called before the first frame update
    public float activationDelay;
    public bool activated = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activationDelay -= Time.deltaTime;
        if (!activated)
        {
            if (activationDelay < 0.0f)
            {
                StartCoroutine(FIRELASER());
                activated = true;
            }
        }
    }

    public void ManualFire()
    {
        StartCoroutine(FIRELASER());

    }

    IEnumerator FIRELASER()
    {

        while(sprite.localScale.x < maxSize)
        {
            sprite.localScale = new Vector3(sprite.localScale.x + (growthSpd * Time.deltaTime), sprite.localScale.y, sprite.localScale.z);
            yield return 0;
        }
        boxCol.enabled = true;
        audioS.Play();
        yield return new WaitForSeconds(0.1f);

        boxCol.enabled = false;
        while(spriteR.color.a >0f)
        {
            spriteR.color = new Color(1, 1, 1, spriteR.color.a - (fadeSpd * Time.deltaTime));
            yield return 0;

        }

        Destroy(this.gameObject, 0.5f);
        yield return 0;
    }
}
