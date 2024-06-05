using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInst : MonoBehaviour
{
    public GameObject gridPrefab;
    public float col;
    public float row;

    void Start()
    {
        Transform parentTransform = this.transform;

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GameObject grid = Instantiate(gridPrefab) as GameObject;
                grid.transform.position = new Vector2(this.transform.position.x + i, this.transform.position.x + j);
                grid.transform.SetParent(parentTransform);
            }
        }
    }
}