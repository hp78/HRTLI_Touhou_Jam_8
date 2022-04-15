using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossBase : MonoBehaviour
{

    public float cooldown;
    [Serializable]
    public struct ListOfPhase
    {
        public AtkBase[] listOfAtk;
        // Start is called before the first frame update

    }

    [SerializeField]
    public ListOfPhase[] listOfPhases;

    public int phaseNumber;
    public int atkNumber;

    void Start()
    {
        StartCoroutine(PrepareAtk());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtkIsDone()
    {
        StartCoroutine(PrepareAtk());
    }

    IEnumerator PrepareAtk()
    {
        yield return new WaitForSeconds(cooldown);

        int rand = UnityEngine.Random.Range(0, listOfPhases[phaseNumber].listOfAtk.Length);
        listOfPhases[phaseNumber].listOfAtk[rand].StartSkill();

        yield return 0;
    }
}
