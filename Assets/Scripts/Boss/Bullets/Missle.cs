using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public Transform player;
    public GameObject beam;

    public float startSpeed;
    public float accel;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        while (duration > 0f)
        {
            startSpeed += accel * Time.deltaTime;
            transform.position = transform.position + (direction * startSpeed * Time.deltaTime);
            duration -= Time.deltaTime;
            yield return 0;

        }


        yield return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            Instantiate(beam, transform.position + new Vector3 (0f, -2f, 0f), Quaternion.identity);
            StopAllCoroutines();
            Destroy(this.gameObject, .5f);
        }
    }

}
