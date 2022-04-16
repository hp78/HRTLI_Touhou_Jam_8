using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScripts : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerHandController lHandController;
    public PlayerHandController rHandController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootAttack()
    {
        lHandController.Shoot();
        //playerController.IncreaseChain();
    }

    public void ResetChain()
    {
        playerController.ResetChain();
    }
}
