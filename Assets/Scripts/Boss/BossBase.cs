using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossBase : MonoBehaviour
{
    public BossStats Stats;

    int rand;
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
        if(phaseEnd)
        {
            ++phaseNumber;
            if (phaseNumber >= listOfPhases.Length)
            {
                End();
                return;
            }
            else
            {

                currentHealth = Stats.Health[phaseNumber];
                foreach (AtkBase atk in listOfPhases[phaseNumber-1].listOfAtk)
                    atk.UpgradeSkill();
                phaseEnd = false;
            }
        }
        
            StartCoroutine(PrepareAtk());
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
            if (currentHealth <= 0f)
            {
                phaseEnd = true;
            }
        }
        internalFlickerCD = flickerCD;
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
