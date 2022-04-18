using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMikoSakiScript : MonoBehaviour
{
    public float currentHealth = 150;
    Animator anim;
    public SpriteRenderer spriteRend;
    Coroutine flicker;

    public AudioClip[] hitSFXList;
    public AudioSource AudioS;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        currentHealth = 100;
        anim.CrossFade("MikoSakiEntry", 0.01f);
        Invoke("RightSlash", 1.9f);
    }

    void RightSlash()
    {
        anim.CrossFade("MikoSakiRightToLeftSlash", 0.01f);
        Invoke("LeftHope", 4.4f);
    }

    void LeftHope()
    {
        anim.CrossFade("MikoSakiLeftHopebeams", 0.01f);
        Invoke("LeftCharge", 2.9f);
    }

    void LeftCharge()
    {
        anim.CrossFade("MikoSakiLeftToRightCharge", 0.01f);
        Invoke("RightHope", 1.9f);
    }

    void RightHope()
    {
        anim.CrossFade("MikoSakiRightHopebeams", 0.01f);
        Invoke("RightCharge", 2.9f);
    }

    void RightCharge()
    {
        anim.CrossFade("MikoSakiRightToLeftCharge", 0.01f);
        Invoke("LeftSlash", 1.9f);
    }

    void LeftSlash()
    {
        anim.CrossFade("MikoSakiLeftToRightSlash", 0.01f);
        Invoke("RightSlash", 4.4f);
    }


    //
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        int rand = UnityEngine.Random.Range(0, hitSFXList.Length);
        AudioS.clip = hitSFXList[rand];
        AudioS.Play();
        if (currentHealth <= 0f)
        {
            LevelController.instance.GoToNextLevel("MikoSakiCutscene");
        }

        if (flicker != null)
            StopCoroutine(flicker);
        flicker = StartCoroutine(ReceiveDamage());

        LevelController.instance.UpdateBossHealth((currentHealth+100) / 200);
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
