using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public GameObject chil;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x <= Screen.width / 2)
            {
                transform.position = chil.transform.position*Time.deltaTime*2f;
            }
        } 
    }
}
