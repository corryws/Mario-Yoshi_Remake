using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour
{
    public GameManager gameManager;
    public Transform fatherParent;
    public LayerMask groundLayer;

    public float fallspeed = -1f;
    public bool  isfalling = false;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(StartToFall());
    }

    IEnumerator StartToFall()
    {
        yield return new WaitForSeconds(gameManager.WaitingTime);
        if(isfalling) transform.Translate(0, fallspeed , 0);
        StartCoroutine(StartToFall());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("element"))
        {
            if(fatherParent != null && this.gameObject.tag == "PlatformElement")
            {
                //Debug.Log("Collision beetween element");
                if(this.gameObject.GetComponent<Animator>().runtimeAnimatorController.name == "egg_down" && collision.gameObject.GetComponent<Animator>().runtimeAnimatorController.name == "egg_up")
                  Destroy(collision.gameObject);
                 

                Elements elementsComponent  = collision.gameObject.GetComponent<Elements>();
                elementsComponent.fallspeed = 0f; elementsComponent.isfalling = false;

                collision.gameObject.transform.position = new Vector2(this.transform.position.x,this.transform.position.y+1f);

                fatherParent.GetComponent<Platform>().childObjects.Add(collision.gameObject);

                collision.gameObject.transform.SetParent(fatherParent);
                collision.gameObject.tag = "PlatformElement";
                collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                collision.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(collision.gameObject.GetComponent<BoxCollider2D>().offset.x,0.32f);
                collision.gameObject.GetComponent<BoxCollider2D>().size   = new Vector2(collision.gameObject.GetComponent<BoxCollider2D>().size.x,1.3f);
            
                if (elementsComponent != null)  elementsComponent.fatherParent = fatherParent;
                fatherParent.GetComponent<Platform>().RemoveNullObjects();
                fatherParent.GetComponent<Platform>().ListEggCheck();
                fatherParent.GetComponent<Platform>().ListAdiacentCheck();
                fatherParent.GetComponent<Platform>().CheckGameOver();
                
            }
        }
    }
}
