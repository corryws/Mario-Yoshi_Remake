using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{

    public float spd = 0.5f;
    Vector3 startpos;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newpos = Mathf.Repeat(Time.time*spd,Mathf.Abs(this.transform.localScale.x));
        transform.position = startpos + Vector3.right * newpos; //+ Vector3.down * newpos
    }
}
