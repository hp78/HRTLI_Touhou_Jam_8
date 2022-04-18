using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public Transform player;
    public SpriteRenderer[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - player.position.x < 0.0f)
        {
            foreach (SpriteRenderer r in sprites) r.flipX = true;
        }
        else 
            foreach (SpriteRenderer r in sprites) r.flipX = false;
    }
}
