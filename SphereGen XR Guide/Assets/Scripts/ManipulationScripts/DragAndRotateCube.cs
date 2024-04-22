using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotateCube : MonoBehaviour
{
    public bool isActive = false;
    Color activeColor = new Color();
    
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshRenderer>().material.color != null)
        {
            GetComponent<MeshRenderer>().material.color = activeColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            //activeColor = Color.red;

            if (Input.touchCount == 1)
            {
                Debug.Log("I'm Touched ");
                Touch screenTouch = Input.GetTouch(0);

                if (screenTouch.phase == TouchPhase.Moved)
                {
                    // transform.Rotate(0f, screenTouch.deltaPosition.x, 0f);
                    transform.position = new Vector3(transform.position.x + screenTouch.deltaPosition.x * speed, transform.position.y,transform.position.z+ screenTouch.deltaPosition.y * speed);
                 //   Vector3 pos = screenTouch.position;
                    
                }

                if (screenTouch.phase == TouchPhase.Ended)
                {
                    isActive = false;
                }

            }

        }
        else
        {
            //activeColor = Color.white;
        }
        //Why is this called every single update?
        //if(GetComponent<MeshRenderer>().material.color != null)
        //{
        //    GetComponent<MeshRenderer>().material.color = activeColor;
        //}
    }
}