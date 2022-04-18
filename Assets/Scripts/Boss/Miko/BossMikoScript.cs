using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMikoScript : MonoBehaviour
{
    public float currentHealth = 150;
    Animator anim;
    public SpriteRenderer spriteRend;
    Coroutine flicker;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        currentHealth = 225;
        anim.CrossFade("MikoRightDives", 0.01f);
        Invoke("LeftSlash", 4.5f);
    }

    void RightDives()
    {
        anim.CrossFade("MikoRightDives", 0.01f);
        Invoke("LeftSlash", 4.6f);
    }

    void LeftSlash()
    {
        anim.CrossFade("MikoLeftSlash", 0.01f);
        Invoke("RightCharge", 3.9f);
    }

    void RightCharge()
    {
        anim.CrossFade("MikoRightCharge", 0.01f);
        Invoke("RightSlash", 2.5f);
    }

    void RightSlash()
    {
        anim.CrossFade("MikoRightSlash", 0.01f);
        Invoke("LeftCharge", 3.9f);
    }

    void LeftCharge()
    {
        anim.CrossFade("MikoLeftCharge", 0.01f);
        Invoke("LeftDives", 2.5f);
    }

    void LeftDives()
    {
        anim.CrossFade("MikoLeftDives", 0.01f);
        Invoke("RightDives", 4.6f);
    }

    //
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0f)
        {
            LevelController.instance.GoToNextLevel("MikoCutscene");
        }

        if (flicker != null)
            StopCoroutine(flicker);
        flicker = StartCoroutine(ReceiveDamage());
    }

    IEnumerator ReceiveDamage()
    {
        spriteRend.color = Color.black;
        yield return new WaitForSeconds(0.05f);

        spriteRend.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        spriteRend.color = Color.black;
        yield return new WaitForSeconds(0.05f);

        spriteRend.color = Color.white;
    }
}