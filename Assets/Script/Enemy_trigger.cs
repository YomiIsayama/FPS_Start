using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_trigger : MonoBehaviour
{
    public static bool is_trigger = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("patrol single is "+ is_trigger);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == "Player")
        {
            is_trigger = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        is_trigger = false;
    }
}
