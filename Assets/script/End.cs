using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // escapeで終了
        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
