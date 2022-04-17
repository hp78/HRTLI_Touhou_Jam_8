using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuyukoAtkDmg : MonoBehaviour
{
    public float dmg;
    public bool knockback;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (knockback)
            {
                Vector2 temp = collision.transform.position - this.transform.position;

                collision.GetComponent<PlayerController>().DamagePlayerWithKnockback(dmg, temp * 10f);
            }
            else
                collision.GetComponent<PlayerController>().DamagePlayer(dmg);

        }
    }
}
