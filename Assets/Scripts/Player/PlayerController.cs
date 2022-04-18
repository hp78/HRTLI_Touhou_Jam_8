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
    public float currHealth;  // made it public to test

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

    [Space(5)]
    public GameObject shootSpawnPos;

    public GameObject pfGunAttack;
    public GameObject pfFistAttack;
    public GameObject pfSwordAttack;
    public GameObject pfSpearAttack;
    public GameObject pfSpearAttack2;

    [Header("LeftHandObjects")]
    public GameObject lFist;
    public GameObject lHold;
    public GameObject lGun;
    public GameObject lSword;
    public GameObject lSpear;
    public GameObject lShield;

    [Header("RightHandObjects")]
    public GameObject rFist;
    public GameObject rHold;
    public GameObject rGun;
    public GameObject rSword;
    public GameObject rSpear;
    public GameObject rShield;

    [Header("Fumo")]
    public GameObject fumo;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        isPlayerAlive.val = true;

        rigidbody2d = GetComponent<Rigidbody2D>();
        currHealth = 100;

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
        SwitchWeapon();
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
            anim.CrossFade("Sword1", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 1)
        {
            anim.CrossFade("Shield1", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 2)
        {
            anim.CrossFade("Shield2", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 3)
        {
            anim.CrossFade("Sword2", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 4)
        {
            anim.CrossFade("Shield3", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
        else if (currChain == 5)
        {
            anim.CrossFade("Sword3", 0.05f);
            currInputLock = 0.5f;
            ++currChain;
        }
    }

    void SpearAttack()
    {
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

    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currRWeapon = PlayerWeapon.FIST;
            ResetHands();
            lFist.SetActive(true);
            rFist.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currRWeapon = PlayerWeapon.GUN;
            ResetHands();
            lHold.SetActive(true);
            lGun.SetActive(true);
            rHold.SetActive(true);
            rGun.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currRWeapon = PlayerWeapon.SWORD;
            ResetHands();
            lHold.SetActive(true);
            lSword.SetActive(true);
            rFist.SetActive(true);
            rShield.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currRWeapon = PlayerWeapon.SPEAR;
            ResetHands();
            lFist.SetActive(true);
            rFist.SetActive(true);
            lSpear.SetActive(true);
        }
    }

    void ResetHands()
    {
        lFist.SetActive(false);
        lHold.SetActive(false);
        lGun.SetActive(false);
        lSword.SetActive(false);
        lSpear.SetActive(false);
        lShield.SetActive(false);

        rFist.SetActive(false);
        rHold.SetActive(false);
        rGun.SetActive(false);
        rSword.SetActive(false);
        rSpear.SetActive(false);
        rShield.SetActive(false);
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

    public void SpawnFistHitbox()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfFistAttack, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfFistAttack, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void SpawnGunHitbox()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfGunAttack, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfGunAttack, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SpawnSwordHitbox()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfSwordAttack, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfSwordAttack, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SpawnSpearHitbox1()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfSpearAttack, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfSpearAttack, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SpawnSpearHitbox2()
    {
        if (topRoot.transform.localScale.x > 0)
        {
            Instantiate(pfSpearAttack2, shootSpawnPos.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject go = Instantiate(pfSpearAttack2, shootSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
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
            fumo.SetActive(true);
            topRoot.SetActive(false);
            LevelController.instance.PlayerDiedSeq();
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

        yield return new WaitForSeconds(0.75f);

        isPlayerInvul = false;
    }

    IEnumerator ReceiveDamage()
    {
        currInvulframe = 0.0f;
        isPlayerInvul = true;
        spriteRend.gameObject.SetActive(true);

        while (currInvulframe < invulFrameMax)
        {
            spriteRend.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            currInvulframe += 0.1f;

            spriteRend.color = Color.black;

            yield return new WaitForSeconds(0.05f);

            currInvulframe += 0.05f;
        }

        spriteRend.gameObject.SetActive(false);
        spriteRend.color = Color.white;
        isPlayerInvul = false;
    }

    public void HealPlayer(float val)
    {
        currHealth += val;
        evntUpdateHealth?.Invoke(currHealth);
    }

}