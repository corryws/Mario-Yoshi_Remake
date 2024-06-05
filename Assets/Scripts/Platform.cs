using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Aggiungi questo using

public class Platform : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> childObjects = new List<GameObject>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("element"))
        {
            //Debug.Log("Collisione con la piattaforma!");
            if(collision.gameObject.GetComponent<Animator>().runtimeAnimatorController.name == "egg_up") Destroy(collision.gameObject);
            
            Elements elementsComponent  = collision.gameObject.GetComponent<Elements>();
            elementsComponent.fallspeed = 0f; elementsComponent.isfalling = false;

            collision.gameObject.transform.position = new Vector2(this.transform.position.x,this.transform.position.y+1f);

            childObjects.Add(collision.gameObject);

            collision.transform.SetParent(transform);
            collision.gameObject.tag = "PlatformElement";
            collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            collision.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(collision.gameObject.GetComponent<BoxCollider2D>().offset.x,0.32f);
            collision.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collision.gameObject.GetComponent<BoxCollider2D>().size.x,1.3f);
        
            if (elementsComponent != null) elementsComponent.fatherParent = this.transform;
            RemoveNullObjects();
            ListEggCheck();
            ListAdiacentCheck();
            CheckGameOver();  
        }
    }

     public void RemoveNullObjects()
    {
        // Rimuovi tutti gli oggetti null dalla lista
        childObjects = childObjects.Where(item => item != null).ToList();
    }

    public void DestroyAllChildObjects()
    {
        foreach (GameObject child in childObjects) if (child != null)  Destroy(child);
        childObjects.Clear();
    }


   public void CheckGameOver()
    {
        if (childObjects.Count >= 8 && childObjects.All(item => item != null)) {DestroyAllChildObjects(); gameManager.GameOver();}
    }

    // Controlla se l'oggetto che ha collidato con la piattaforma è quello che ti interessa

    public void ListAdiacentCheck()
    {
        if (childObjects.Count >= 2)
        {
            // Iterare dalla fine della lista all'inizio per evitare problemi di indice
            for (int i = childObjects.Count - 2; i >= 0; i--)
            {
                if (childObjects[i] != null && childObjects[i + 1] != null)
                {
                    Animator spriteRenderer1 = childObjects[i].GetComponent<Animator>();
                    Animator spriteRenderer2 = childObjects[i + 1].GetComponent<Animator>();

                    if (spriteRenderer1 != null && spriteRenderer2 != null && spriteRenderer1.runtimeAnimatorController == spriteRenderer2.runtimeAnimatorController)
                    {
                        Destroy(childObjects[i]);
                        Destroy(childObjects[i + 1]);
                        childObjects.RemoveAt(i + 1); // Rimuove prima l'elemento con indice maggiore
                        childObjects.RemoveAt(i); // Poi rimuove l'elemento con indice minore

                        gameManager.IncrementScore(5);
                    }
                }
            }
        }
    }

    
public void ListEggCheck()
{
    if (childObjects.Count >= 2)
    {
        // Iterare dalla fine della lista all'inizio per evitare problemi di indice
        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i] != null)
            {
                Animator spriteRenderer = childObjects[i].GetComponent<Animator>();
                
                if (spriteRenderer != null && spriteRenderer.runtimeAnimatorController.name == "egg_down")
                {
                    for (int j = i + 1; j < childObjects.Count; j++)
                    {
                        if (childObjects[j] != null)
                        {
                            Animator spriteRendererEnd = childObjects[j].GetComponent<Animator>();

                            if (spriteRendererEnd != null && spriteRendererEnd.runtimeAnimatorController.name == "egg_up")
                            {
                                // Distruggere e rimuovere tutti gli elementi tra "soprauovo" e "sottouovo" inclusi
                                for (int k = i; k <= j; k++) if (childObjects[k] != null) Destroy(childObjects[k]);

                                childObjects.RemoveRange(i, j - i + 1); gameManager.IncrementScore((j - i + 1) * 5); i--; 
                                gameManager.IncrementEgg(1); break;
                            }
                        }
                    }
                }
            }
        }
    }
}
}
