using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public GameObject  player;
    public Transform[] positions;
    public GameObject[] platforms;
    public Sprite[] PlayerSprites;
    public int[] PosPlatform;
    public int index1 = 0, index2 = 1;
    private int currentIndex = 0;
    public string CURRENT_STATE = "FRONT"; 

    void Awake()
    {        
        player.transform.position = positions[currentIndex].position;
        ColorPositionPlatform();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
        else if (Input.GetKeyDown(KeyCode.D)) MoveRight();
        else if (Input.GetKeyDown(KeyCode.Return)) ChangeCurrentState();
    }

    void MoveLeft()
    {
        if (currentIndex == 0) currentIndex = 2;
        else if (currentIndex == 1) currentIndex = 0;

        player.transform.position = positions[currentIndex].position;
        ColorPositionPlatform();
    }

    void MoveRight()
    {
        if (currentIndex == 0) currentIndex = 1;
        else if (currentIndex == 2) currentIndex = 0;

        player.transform.position = positions[currentIndex].position;
        ColorPositionPlatform();
    }

    void ColorPositionPlatform()
    {
      Color defaultColor = new Color(1f, 1f, 1f); // Normalizzati tra 0 e 1
      Color colorPlatform = new Color(1f, 0.608f, 0f);

    for (int i = 0; i < 4; i++)
    {
        var childTransform = platforms[i].transform.GetChild(0);
        var spriteRenderer = childTransform.GetComponent<SpriteRenderer>();

        spriteRenderer.color = defaultColor;
        childTransform.position = new Vector2(platforms[i].transform.position.x, childTransform.position.y);
    }

    void SetPlatformColorAndPosition(int index1, int index2, float offset1, float offset2)
    {
        var childTransform1 = platforms[index1].transform.GetChild(0);
        var childTransform2 = platforms[index2].transform.GetChild(0);

        var spriteRenderer1 = childTransform1.GetComponent<SpriteRenderer>();
        var spriteRenderer2 = childTransform2.GetComponent<SpriteRenderer>();

        spriteRenderer1.color = colorPlatform;
        spriteRenderer2.color = colorPlatform;

        childTransform1.position = new Vector2(platforms[index1].transform.position.x + offset1, childTransform1.position.y);
        childTransform2.position = new Vector2(platforms[index2].transform.position.x + offset2, childTransform2.position.y);
    }

    if (currentIndex == 0)
    {
        SetPlatformColorAndPosition(1, 2, -0.1f, 0.1f);
    }
    else if (currentIndex == 2)
    {
        SetPlatformColorAndPosition(0, 1, -0.1f, 0.1f);
    }
    else if (currentIndex == 1)
    {
        SetPlatformColorAndPosition(3, 2, 0.1f, -0.1f);
    }
}


    void ChangeCurrentState()
     {
        if (CURRENT_STATE == "FRONT") 
        {
            CURRENT_STATE = "BACK";
            player.GetComponent<Animator>().SetBool("fliptoback",true);
            player.GetComponent<Animator>().SetBool("fliptofront",false);
        }
        else if (CURRENT_STATE == "BACK")
        {
            CURRENT_STATE = "FRONT";
            player.GetComponent<Animator>().SetBool("fliptofront",true);
            player.GetComponent<Animator>().SetBool("fliptoback",false);
        } 
        //Debug.Log("CURRENT_STATE changed in " + CURRENT_STATE);

        if (currentIndex == 0)
        {
            index1 = 1; index2 = 2;

        }else if(currentIndex == 2)
        {
            index1 = 0 ; index2 = 1;

        }else if(currentIndex == 1)
        {
            index1 = 3; index2 = 2;
        } 

        SwapPlatform(index1,index2);
     }

     void SwapPlatform(int index1, int index2)
    {
        if (index1 < 0 || index1 >= platforms.Length || index2 < 0 || index2 >= platforms.Length)
        {
            //Debug.Log("Indici delle piattaforme non validi");
            return;
        }  

        int tempPlatform = PosPlatform[index1];
        PosPlatform[index1]       = PosPlatform[index2];
        PosPlatform[index2]       = tempPlatform;

        Vector3 tempPosition = platforms[index1].transform.position;
        platforms[index1].transform.position = platforms[index2].transform.position;
        platforms[index2].transform.position = tempPosition;

        GameObject tempPlatformobj  = platforms[index1];
        platforms[index1] = platforms[index2];
        platforms[index2] = tempPlatformobj;

        //Debug.Log("SWAP : " + index1 + " WITH " + index2);
    }
}
