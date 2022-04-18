using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<BossBase>())
                collision.GetComponent<BossBase>().TakeDamage(damage);

            if (collision.GetComponent<BossMikoSakiScript>())
                collision.GetComponent<BossMikoSakiScript>().TakeDamage(damage);

            if (collision.GetComponent<BossMikoScript>())
                collision.GetComponent<BossMikoScript>().TakeDamage(damage);
        }
    }
}
