using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public delegate void DeleUpdateHealth(float health);
    public static event DeleUpdateHealth evntUpdateHealth;

    [Space(5)]
    public BoolVal isGamePaused;
    public BoolVal isPlayerAlive;
    public BoolVal isPlayerInControl;

    [Space(5)]
    public Animator anim;

    [Space(5)]
    public float moveForce;
    public float jumpForce;

    [Space(5)]
    bool isPlayerFacingLeft = false;
    bool isPlayerInvul = false;
    float invulFrameMax = 0.5f;
    float currInvulframe = 0.0f;

    [Space(5)]
    public float jumpThreshold;

    public bool inAir;
    bool isAttacking = false;
    float currHealth = 50;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;
    private int layermask;

    [Space(5)]
    public GameObject spriteBody;
    public SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currHealth = 50;

        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerAlive.val || isGamePaused.val || !isPlayerInControl) return;

        Movement();
        Jump();
    }

    void Movement()
    {
        movement = rigidbody2d.velocity;
        movement.x = Input.GetAxis("Horizontal") * moveForce;

        if (movement.x < 0)
        {
            isPlayerFacingLeft = true;
            spriteBody.transform.localScale = new Vector3(1, spriteBody.transform.localScale.y, spriteBody.transform.localScale.z);
        }
        else if (movement.x > 0)
        {
            isPlayerFacingLeft = false;
            spriteBody.transform.localScale = new Vector3(-1, spriteBody.transform.localScale.y, spriteBody.transform.localScale.z);
        }

        //anim.SetFloat("XVelocity", movement.x);
        //anim.SetFloat("YVelocity", movement.y);

        rigidbody2d.velocity = movement;

        if (transform.position.y < -5.0f)
        {
            currHealth = 0;
            DamagePlayer(9999);
        }

        Debug.Log(movement);
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        //Gizmos.color = Color.cyan;

        if (hit || hit2)
        {
            inAir = false;
            //anim.SetTrigger("TriggerLand");
        }
        else
        {
            inAir = true;
        }

        if (!inAir && Input.GetKeyDown(KeyCode.Z))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            //anim.SetTrigger("TriggerJump");
        }


    }

    public void DamagePlayer(float val)
    {
        if (isPlayerInvul) return;

        currHealth -= val;

        if (currHealth <= 0)
        {
            isPlayerAlive.val = false;
        }
        else
        {
            StartCoroutine(ReceiveDamage());
        }
        evntUpdateHealth?.Invoke(currHealth);
    }

    IEnumerator ReceiveDamage()
    {
        currInvulframe = 0.0f;
        isPlayerInvul = true;

        while (currInvulframe < invulFrameMax)
        {
            spriteRend.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            currInvulframe += 0.1f;

            spriteRend.color = Color.black;

            yield return new WaitForSeconds(0.05f);

            currInvulframe += 0.05f;
        }

        spriteRend.color = Color.white;
        isPlayerInvul = false;
    }

    public void HealPlayer(float val)
    {
        currHealth += val;
        evntUpdateHealth?.Invoke(currHealth);
    }

}