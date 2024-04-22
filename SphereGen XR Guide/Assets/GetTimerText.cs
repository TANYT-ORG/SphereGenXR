using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GetTimerText : MonoBehaviour
{
    public GameObject[] PContent;
    public GameObject LContent, PContents;
    public GameObject Portrait, Landscape;
    private int i;
    public GameObject ScrollView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void MoveTextToPortrait()
    {
        foreach (GameObject go in PContent)
        {
           
            if (Portrait.activeInHierarchy == true)
            {
                //go.transform.parent = PContents.transform;
                go.transform.SetParent(PContents.transform);
            }
        }
    }

    public void MoveTextToLandscape()
    {
        foreach (GameObject go in PContent)
        {
            if (Landscape.activeInHierarchy == true)
            {
                //go.transform.parent = LContent.transform;
                go.transform.SetParent(LContent.transform);
            }
          
        }
    }
}
