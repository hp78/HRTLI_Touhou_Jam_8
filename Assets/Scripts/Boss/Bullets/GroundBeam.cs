using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBeam : MonoBehaviour
{
    public Transform groundsprite;
    public Transform beamsprite;

    public SpriteRenderer groundSpriteR;
    public SpriteRenderer beamSpriteR;

    public BoxCollider2D col;

    public float shrinkSpd;
    public float unFadeSpd;

    public float maxSize;
    public float growthSpd;
    public float unFadeSpdBeam;
    public float fadeSpd;
    bool fired;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FIRELASER());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FIRELASER()
    {
        while(groundsprite.localScale.x >0.05)
        {
            groundsprite.localScale = new Vector3(groundsprite.localScale.x - (shrinkSpd * Time.deltaTime), groundsprite.localScale.y - (shrinkSpd * Time.deltaTime), groundsprite.localScale.z);
            groundSpriteR.color = new Color(1, 1, 1, groundSpriteR.color.a - (unFadeSpd * Time.deltaTime));

            yield return 0;
        }
        groundsprite.gameObject.SetActive(false);
        beamsprite.gameObject.SetActive(true);
        col.enabled = true;
        while (beamsprite.localScale.x < maxSize)
        {
            beamsprite.localScale = new Vector3(beamsprite.localScale.x + (growthSpd * Time.deltaTime), beamsprite.localScale.y, beamsprite.localScale.z);
            if (beamSpriteR.color.a < 1.0f)
            {
                beamSpriteR.color = new Color(1, 1, 1, beamSpriteR.color.a + (unFadeSpdBeam * Time.deltaTime));

            }
            

            yield return 0;
        }
        col.enabled = false;

        while (beamSpriteR.color.a >0.0f)
        {
            beamSpriteR.color = new Color(1, 1, 1, beamSpriteR.color.a - (fadeSpd * Time.deltaTime));
            yield return 0;

        }

        Destroy(this.gameObject, 0.5f);
        yield return 0;


    }
}
