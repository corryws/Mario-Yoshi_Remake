using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviosa : MonoBehaviour
{
    [SerializeField]bool reversego=false;
    [SerializeField]bool UD=false,LR=false;//UP DOWN - LEFT RIGHT
    [SerializeField]float speed=0;

    void OnEnable(){StartCoroutine(Reverse());}

    void FixedUpdate()
    {   
        if(UD && !LR)
         if(reversego)transform.position +=  new Vector3(0,speed*Time.deltaTime,0);
          else transform.position -= new Vector3(0,speed*Time.deltaTime,0);
        else if(LR && !UD)
          if(reversego)transform.position += new Vector3(speed*Time.deltaTime,0,0);
            else transform.position -= new Vector3(speed*Time.deltaTime,0,0);
    }

    IEnumerator Reverse(){
        while(gameObject.activeSelf){
            yield return new WaitForSeconds(0.5f);
            reversego = !reversego;
        }
    }

}
