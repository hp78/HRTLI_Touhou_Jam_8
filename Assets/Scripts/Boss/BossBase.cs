using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossBase : MonoBehaviour
{

    public enum BOSS
    {
        RAN,
        YUYUKO
    }
    public BossStats Stats;
    public BOSS thisboss;

    public AudioClip[] hitSFXList;
    public AudioSource AudioS;

    int rand=-1;
    bool repeat = false;

    public float currentHealth;

    public int phaseNumber;
    public int atkNumber;
    public float cooldown;
    public bool phaseEnd;

    public SpriteRenderer[] sprites;

    public float flickerCD;
    float internalFlickerCD;
    [Serializable]
    public struct ListOfPhase
    {
        public AtkBase[] listOfAtk;
        // Start is called before the first frame update

    }

    [SerializeField]
    public ListOfPhase[] listOfPhases;
    

    void Start()
    {
        currentHealth = Stats.Health[phaseNumber];
        StartCoroutine(PrepareAtk());

    }

    private void Update()
    {
        FlickerDmg();
    }
    public void AtkIsDone()
    {
        if (phaseEnd)
        {
            if (phaseNumber >= listOfPhases.Length)
            {
                End();
                return;
            }
            else
            {

                StartCoroutine(RefillHealth());

            }
        }
        else
            StartCoroutine(PrepareAtk());
    }

    IEnumerator RefillHealth()
    {
        while(currentHealth < Stats.Health[phaseNumber])
        {
            currentHealth += 100f * Time.deltaTime;
            LevelController.instance.UpdateBossHealth(currentHealth / Stats.Health[phaseNumber]);

            yield return 0;
        }

        foreach (AtkBase atk in listOfPhases[phaseNumber - 1].listOfAtk)
            atk.UpgradeSkill();
        phaseEnd = false;

        StartCoroutine(PrepareAtk());

        yield return 0;
    }

    IEnumerator PrepareAtk()
    {
        yield return new WaitForSeconds(cooldown);

        int newrand = UnityEngine.Random.Range(0, listOfPhases[phaseNumber].listOfAtk.Length);
        if (!repeat)
        {
            while(newrand == rand)
            newrand = UnityEngine.Random.Range(0, listOfPhases[phaseNumber].listOfAtk.Length);
        }
        rand = newrand;
        repeat = listOfPhases[phaseNumber].listOfAtk[rand].repeat;
        listOfPhases[phaseNumber].listOfAtk[rand].StartSkill();

        yield return 0;
    }

    public void TakeDamage(float dmg)
    {
        if (!phaseEnd)
        {
            currentHealth -= dmg;
            int rand = UnityEngine.Random.Range(0, hitSFXList.Length);
            AudioS.clip = hitSFXList[rand];
            AudioS.Play();
            if (currentHealth <= 0f)
            {
                phaseEnd = true;
                ++phaseNumber;
                if (phaseNumber >= listOfPhases.Length)
                {
                    if (thisboss == BOSS.RAN)
                    {
                        LevelController.instance.GoToNextLevel("RanCutscene");
                           }
                    if (thisboss == BOSS.YUYUKO)
                    {
                        LevelController.instance.GoToNextLevel("StageEnding");
                    }
                    

                }
            }
        }
        internalFlickerCD = flickerCD;
        if (!(phaseNumber >= listOfPhases.Length))
            LevelController.instance.UpdateBossHealth(currentHealth / Stats.Health[phaseNumber]);
    }

    public void End()
    {
        Debug.Log("END");
    }

    public void FlickerDmg()
    {
        if(internalFlickerCD>0.0f)
        {
            foreach(SpriteRenderer r in sprites)
            {
                r.color = Color.red;
            }
        }
        else
        {
            foreach (SpriteRenderer r in sprites)
            {
                r.color = Color.white;
            }
        }
        internalFlickerCD -= Time.deltaTime;
    }
}
