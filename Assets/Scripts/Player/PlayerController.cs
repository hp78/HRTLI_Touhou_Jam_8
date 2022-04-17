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
    float currInputLock = 0.0f;
    float currRollTime = 0.0f;

    [Space(5)]
    public float jumpThreshold;

    public bool inAir;
    bool isAttacking = false;
    float currHealth = 50;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;
    private int layermask;

    [Space(5)]
    public GameObject topRoot;
    public GameObject lHandRoot;
    public GameObject rHandRoot;
    public SpriteRenderer spriteRend;

    [Space(5)]
    public PlayerHandController lHandController;
    public PlayerHandController rHandController;

    /// <summary>
    /// 0 = fist
    /// 1 = gun
    /// 2 = sword
    /// 3 = spear
    /// </summary>
    public enum PlayerWeapon { FIST, GUN, SWORD, SPEAR };
    [Space(5)]
    public PlayerWeapon currRWeapon = PlayerWeapon.FIST;
    public PlayerWeapon currLWeapon = PlayerWeapon.FIST;
    int currChain = 0;

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

        currInputLock -= Time.deltaTime;
        currRollTime -= Time.deltaTime;

        if (currInputLock > 0.0f) return;
        
        Roll();
        if (currRollTime > 0.1f) return;

        Movement();
        Jump();
        Attack();
        
    }

    void Movement()
    {
        movement = rigidbody2d.velocity;
        movement.x = Input.GetAxis("Horizontal") * moveForce;

        if (movement.x < 0)
        {
            isPlayerFacingLeft = true;
            topRoot.transform.localScale = new Vector3(1, topRoot.transform.localScale.y,
                topRoot.transform.localScale.z);
        }
        else if (movement.x > 0)
        {
            isPlayerFacingLeft = false;
            topRoot.transform.localScale = new Vector3(-1, topRoot.transform.localScale.y,
                topRoot.transform.localScale.z);
        }

        anim.SetFloat("XVelocity", Mathf.Abs(movement.x * 0.125f));
        //anim.SetFloat("YVelocity", movement.y);

        rigidbody2d.velocity = movement;

        if (transform.position.y < -5.0f)
        {
            currHealth = 0;
            DamagePlayer(9999);
        }
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        //Gizmos.color = Color.cyan;

        if (hit || hit2)
        {
            inAir = false;
            anim.SetTrigger("TriggerLand");
        }
        else
        {
            inAir = true;
        }

        if (!inAir && Input.GetKeyDown(KeyCode.X))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            anim.SetTrigger("TriggerJump");
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch(currRWeapon)
            {
                case PlayerWeapon.FIST:
                    FistAttack();
                        break;

                case PlayerWeapon.GUN:
                    GunAttack();
                        break;

                case PlayerWeapon.SWORD:
                    SwordAttack();
                        break;

                case PlayerWeapon.SPEAR:
                    SpearAttack();
                        break;
            }
        }            
    }

    void FistAttack()
    {
        if (currChain == 0)
        {
            anim.CrossFade("Fist1", 0.1f);
            currInputLock = 0.4f;
            ++currChain;
        }
        else if (currChain % 2 == 1)
        {
            anim.Play("Fist2");
            currInputLock = 0.15f;
            ++currChain;
        }
        else
        {
            anim.Play("Fist3");
            currInputLock = 0.15f;
            ++currChain;
        }
    }

    void GunAttack()
    {
        if (currChain == 0)
        {
            anim.CrossFade("Gun1", 0.1f);
            currInputLock = 0.4f;
            ++currChain;
        }
        else if (currChain == 1)
        {
            anim.Play("Gun2");
            currInputLock = 0.4f;
            ++currChain;
        }
        else if (currChain == 2)
        {
            anim.Play("Gun3");
            currInputLock = 0.4f;
            ++currChain;
        }
        else if (currChain == 3)
        {
            anim.Play("Gun4");
            currInputLock = 0.75f;
            ++currChain;
        }
    }

    void SwordAttack()
    {
        if (currChain == 0)
        {
            anim.CrossFade("Sword1", 0.01f);
            currInputLock = 0.55f;
            ++currChain;
        }
        else if (currChain == 1)
        {
            anim.CrossFade("Sword2", 0.01f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 2)
        {
            anim.CrossFade("Sword3", 0.01f);
            currInputLock = 0.6f;
            ++currChain;
        }
    }

    void SpearAttack()
    {
        Debug.Log("Spear attack");
        if (currChain == 0)
        {
            anim.CrossFade("Spear1", 0.01f);
            currInputLock = 0.45f;
            ++currChain;
            
        }
        else if (currChain == 1)
        {
            anim.CrossFade("Spear2", 0.01f);
            currInputLock = 0.45f;
            ++currChain;
        }
        else if (currChain == 2)
        {
            anim.CrossFade("Spear3", 0.01f);
            currInputLock = 1.0f;
            ++currChain;
        }
    }

    void Roll()
    {
        if(currRollTime > 0)
        {
            if (isPlayerFacingLeft)
            {
                rigidbody2d.velocity = new Vector2(-1f * moveForce, 0);
            }
            else
            {
                rigidbody2d.velocity = new Vector2(1f * moveForce, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            currChain = 0;
            anim.CrossFade("BodyRoll", 0.01f);
            //anim.SetTrigger("TriggerRoll");
            currRollTime = 1.0f;

            isPlayerInvul = true;
            currInvulframe = 0.0f;
            StartCoroutine(SetInvul());
        }
    }

    public void IncreaseChain()
    {
        ++currChain;
        anim.SetInteger("CurrChain", currChain);
    }

    public void ResetChain()
    {
        currChain = 0;
        anim.SetInteger("CurrChain", 0);
    }

    void GunShoot()
    {

    }

    public void DamagePlayerWithKnockback(float val, Vector2 forceVector, float stunDura = 0.15f)
    {
        if (isPlayerInvul) return;
        rigidbody2d.velocity += forceVector;

        DamagePlayer(val, stunDura);
    }

    public void DamagePlayer(float val, float stunDura = 0.15f)
    {
        if (isPlayerInvul) return;

        currInputLock = stunDura;
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

    IEnumerator SetInvul()
    {
        currInvulframe = 0.0f;
        isPlayerInvul = true;
        Debug.Log("invul");
        yield return new WaitForSeconds(0.9f);
        Debug.Log("not invul");
        isPlayerInvul = false;
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