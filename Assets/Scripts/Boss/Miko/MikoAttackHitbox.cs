using UnityEngine;

public class MikoAttackHitbox : MonoBehaviour
{
    public float dmg;
    float currlife = 0.0f;
    public bool isPerma = false;

    // Update is called once per frame
    void Update()
    {
        currlife += Time.deltaTime;
        if(!isPerma && currlife > 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if(pc != null)
        {
            Vector2 temp = collision.transform.position - this.transform.position;
            collision.GetComponent<PlayerController>().DamagePlayerWithKnockback(dmg, temp * 10f);
        }
    }
}
