using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace SphereGen.GuideXR
{
    public class ObjectManipulation : MonoBehaviour
    {

       public ARRaycastManager m_RaycastManager;
        List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
        [SerializeField]
        Camera arCam;

       


        GameObject spawnedObject;

        // Start is called before the first frame update
        void Start()
        {
         //   m_RaycastManager = IndividualAssetHandler._instance.m_RaycastManager;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.touchCount == 0)
            //    return;
            RaycastHit hit;

#if UNITY_ANDROID && !UNITY_EDITOR
        Ray ray = arCam.ScreenPointToRay(Input.touches[0].position);
        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began )
            {
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Hit");
                    var objectScript = hit.collider.GetComponent<DragAndRotateCube>();
                         objectScript.isActive = !objectScript.isActive;
                }
            }
        }
#else
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("dddd");
                if (m_RaycastManager.Raycast(Input.mousePosition, m_Hits))
                {
                    Debug.Log("gggg");
                    Ray ray = arCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Hit");
                        var objectScript = hit.collider.GetComponent<DragAndRotateCube>();
                        objectScript.isActive = !objectScript.isActive;
                    }

                }
            }
#endif

        }


        public void MoveTheObject()
        {

        }


        Vector3 GetMousePositionInPlaneOfLauncher()
        {
            Plane p = new Plane(transform.right, transform.position);
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            float d;
            if (p.Raycast(r, out d))
            {
                Vector3 v = r.GetPoint(d);
                return v;
            }

            throw new UnityException("Mouse position ray not intersecting launcher plane");
        }

    }
}

