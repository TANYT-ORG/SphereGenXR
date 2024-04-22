
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MoveObject : MonoBehaviour
{
   
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();   
    Camera arCam;   
    GameObject SelectedObject;
    Vector2 touchStartPos;
    public float rotSpeed=5f;
    bool moveObj = false; //false to rotate
    Vector3 ScaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
    void Start()
    {
        m_RaycastManager= FindObjectOfType<ARRaycastManager>();
        arCam= FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        if (SelectedObject == null)
        {
            RaycastHit hit;
            Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Model")
                {
                    SelectedObject = hit.collider.gameObject;
                    //SelectedObject.GetComponent<Renderer>().material.color = Color.green;

                    touchStartPos = Input.GetTouch(0).position;
                }

            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            touchStartPos = Input.GetTouch(0).position;
        }
        
        else if (Input.touchCount == 1 && moveObj)
        {

            if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
            {
                
               if (Input.GetTouch(0).phase == TouchPhase.Moved && SelectedObject != null)
                {

                    MoveSelectedObject();

                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    SelectedObject.GetComponent<Renderer>().material.color = Color.red;
                    SelectedObject = null;

                }
            }
        }
        //Currently this script uses the transform of the object being manipulated, this means that the object will behave differently than expected when rotating from a different camera perspective
        //So to resolve this and to get rotations to operate as expected, rotations should be around an axis that aligns with the user camera view
        //Look up transform.rotatearound unity documentation, we want to use the main camera orientation as our axis, but we also need to move the point of the camera to the position of our object
        //So, create a dummy gameObject that has the same rotation and position as the main camera and updates whenever the camera updates
        //When doing a rotation follow this order
        //Move the dummy gameObject to the position of our object to rotate, then use the dummy.transform.up as your axis for x changes, and dummy.transform.left for your y changes. If this does not work please continue tto investigate how to use the camera's view to create rotations that make sense.
        
        else if (Input.touchCount==1 && !moveObj) //Rotate
        {                       
                if (touchStartPos.y > Input.touches[0].position.y)  
                {
                    Debug.Log(Input.touches[0].position.y + " Rotate Up " + touchStartPos.y);
                    //  transform.Rotate(Vector3.left, 100f * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(new Vector3((touchStartPos.y - Input.touches[0].position.y) * rotSpeed, 0f, 0f) + transform.rotation.eulerAngles);

                }
                else if (touchStartPos.y < Input.touches[0].position.y)
                {
                    Debug.Log(Input.touches[0].position.y + " Rotate Down " + touchStartPos.y);
                // transform.Rotate(Vector3.left, -100f * Time.deltaTime);
                // transform.rotation = Quaternion.Euler(-1.5f * rotSpeed, 0f, 0f) * transform.rotation;
                transform.rotation = Quaternion.Euler(new Vector3((touchStartPos.y - Input.touches[0].position.y) * rotSpeed, 0f, 0f) + transform.rotation.eulerAngles);
            }

                if (touchStartPos.x > Input.touches[0].position.x)
                {
                    Debug.Log("Rotate Left");
                //transform.Rotate(Vector3.up, 100f * Time.deltaTime);
                // transform.rotation = Quaternion.Euler(0f, 1.5f * rotSpeed, 0f) * transform.rotation;
                transform.rotation = Quaternion.Euler(new Vector3(0f, (touchStartPos.x - Input.touches[0].position.x) * rotSpeed, 0f) + transform.rotation.eulerAngles);

            }
                else if (touchStartPos.x < Input.touches[0].position.x)
                {
                    Debug.Log("Rotate Right");
                    //  transform.Rotate(Vector3.up, -100f * Time.deltaTime);
                  //  transform.rotation = Quaternion.Euler(0f, -1.5f * rotSpeed, 0f) * transform.rotation;
                transform.rotation = Quaternion.Euler(new Vector3(0f, (touchStartPos.x-Input.touches[0].position.x) * rotSpeed, 0f) + transform.rotation.eulerAngles);
            }
                else
                {
                    Debug.Log("Do Nothing");
                }
            
        }
        else if (Input.touchCount == 2)
        {
            // 2 Finger 
            // Scale
            Vector2 firstFiinger, secondFinger;
            float initialDistance = 1;
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                firstFiinger = Input.GetTouch(0).position;
                secondFinger = Input.GetTouch(1).position;
                initialDistance = Vector2.Distance(firstFiinger, secondFinger);
                ScaleChange = transform.localScale;
                Debug.Log("Initialize");
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float CurrentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                var factor = (CurrentDistance / initialDistance) / 2;
                transform.localScale = ScaleChange * factor;               
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && SelectedObject != null)
            {
               // SelectedObject.GetComponent<Renderer>().material.color = Color.red;
                SelectedObject = null;

            }

        }
     }


    public void MoveSelectedObject()
    {
        SelectedObject.transform.position = m_Hits[0].pose.position;
    }

    public void RotateSelectedObject()
    {
        moveObj = !moveObj;
    }

   

}
