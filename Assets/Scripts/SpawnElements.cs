using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElements : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] SpawnElement; //Array che contiene object vuoti che servono solo per avere le posizioni
    public GameObject ElementPrefabs; //Prefab da spawnare in una delle posizioni di uno SpawnElement
    public GameObject currElement1, currElement2;

    public RuntimeAnimatorController[] ElementAnimators;
    public Sprite[] ElementSprites;
    public RuntimeAnimatorController randomsprite_1,randomsprite_2;

    public int   check_random = 0, randomIndex_1,randomIndex_2;
    public float spawnInterval,currY = 0f;
    

    void Awake(){SetRandomIndex(); SetRandomSprite();}

    void FixedUpdate(){if(gameManager.endGame == false) StartCoroutine(SpawnElementRandomly());}

    IEnumerator SpawnElementRandomly()
    {
        if(currElement1 == null || currElement2 == null     && 
           currElement1 != null || currElement2 != null     && 
           !currElement1.GetComponent<Elements>().isfalling &&
           !currElement2.GetComponent<Elements>().isfalling)

          Spawn(randomIndex_1,randomIndex_2);
          if(currElement1.transform.position.y == currY-1f)
          {EnableSpawnElement(randomIndex_1,true); EnableSpawnElement(randomIndex_2,true);}

          yield return new WaitForSeconds(spawnInterval);
    }

    void Spawn(int rndmndex_1,int rndmndex_2)
    {
        EnableSpawnElement(randomIndex_1,false); EnableSpawnElement(randomIndex_2,false);
        
        currElement1 = Instantiate(ElementPrefabs, SpawnElement[rndmndex_1].transform.position, Quaternion.identity);
        currElement2 = Instantiate(ElementPrefabs, SpawnElement[rndmndex_2].transform.position, Quaternion.identity);
        
        currElement1.GetComponent<Animator>().runtimeAnimatorController = randomsprite_1;
        currElement2.GetComponent<Animator>().runtimeAnimatorController = randomsprite_2;
        currY = currElement1.transform.position.y;

        SetRandomIndex (); SetRandomSprite();
        
        SpawnElement[randomIndex_1].GetComponent<Animator>().runtimeAnimatorController = randomsprite_1;
        SpawnElement[randomIndex_2].GetComponent<Animator>().runtimeAnimatorController = randomsprite_2;
    }

    void   EnableSpawnElement(int id, bool enable){SpawnElement[id].GetComponent<Animator>().enabled = enable;}
    int    GetRandom         (){return Random.Range(0, SpawnElement.Length);}
    RuntimeAnimatorController GetRandomSprite   (){return ElementAnimators[Random.Range(0, ElementAnimators.Length)]; }

    void SetRandomIndex()
    {
        randomIndex_1 = GetRandom(); randomIndex_2 = GetRandom();
        while (randomIndex_2 == randomIndex_1) randomIndex_2 = GetRandom();
    }
    void SetRandomSprite()
    {
        randomsprite_1 = GetRandomSprite();  randomsprite_2 = GetRandomSprite();
    }
}
