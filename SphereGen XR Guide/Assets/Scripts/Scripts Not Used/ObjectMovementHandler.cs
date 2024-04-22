using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectMovementHandler : MonoBehaviour
{
    private Vector2 touchPosition = default;
    public Camera arcam;
    public ARRaycastManager ARraycastmanager;
    public float speed=3;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.touchCount>0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    touchPosition = touch.position;

        //    if(touch.phase==TouchPhase.Moved)
        //    {
        //        Ray ray = arcam.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit))
        //        {

        //            DragAndRotateCube dr = hit.transform.GetComponent<DragAndRotateCube>();
        //            if (dr != null)
        //            {

        //                // transform.Rotate(0f, screenTouch.deltaPosition.x, 0f);
        //                //  hit.transform.position = new Vector3(hit.transform.position.x + touch.deltaPosition.x * speed,hit. transform.position.y,hit. transform.position.z + touch.deltaPosition.y * speed);
        //                //   Vector3 pos = screenTouch.position;




        //                Debug.Log("LLLLLLLLLLLLLLLLLL");
        //                if (ARraycastmanager.Raycast(touchPosition, m_Hits))
        //                {

        //                    Pose p = m_Hits[0].pose;

        //                        Debug.Log("fghjk");

        //                        // transform.Rotate(0f, screenTouch.deltaPosition.x, 0f);
        //                        // hit.transform.position = new Vector3(hit.transform.position.x + touch.deltaPosition.x * speed, hit.transform.position.y, hit.transform.position.z + touch.deltaPosition.y * speed);
        //                        //   Vector3 pos = screenTouch.position;
        //                        hit.transform.position = p.position;




        //                }
        //            }
        //        }

        //    }

        //}
        ////if (Input.GetMouseButtonDown(0))
        ////{
        ////    Debug.Log("dddd");
        ////    if (m_RaycastManager.Raycast(Input.mousePosition, m_Hits))
        ////    {
        ////        Debug.Log("gggg");
        ////        Ray ray = arCam.ScreenPointToRay(Input.mousePosition);
        ////        if (Physics.Raycast(ray, out hit))
        ////        {
        ////            Debug.Log("Hit");
        ////            var objectScript = hit.collider.GetComponent<DragAndRotateCube>();
        ////            objectScript.isActive = !objectScript.isActive;
        ////        }

        ////    }
        ////}
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Moved)
        {
            return;
        }
        //if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        //{
        //    return;
        //}

        Vector2 touchPos = Input.GetTouch(0).position;
        Debug.Log("Touched the Screen ");//+EventSystem.current.currentSelectedGameObject.name);
        if (ARraycastmanager.Raycast(touchPos, m_Hits))
        {
            if (m_Hits.Count > 0)
            {
                Pose p = m_Hits[0].pose;
                Debug.Log("gggg");
                transform.position = p.position;


            }

        }
    }
}
