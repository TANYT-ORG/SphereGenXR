using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace SphereGen.GuideXR
{
    public class ARFloorTapPlacementHandler : MonoBehaviour
    {
        [SerializeField]
        ARRaycastManager _raycastManager;
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        [SerializeField]
        Camera arCamera;
        [SerializeField]
        private GameObject MainParent;

        [SerializeField]
        private GameObject ObjectToPlace;

        [SerializeField]
        private bool Move = false;
        [SerializeField]
        private IndividualAssetHandler individualAssetHandler;
        public bool IsLoadFirstStory = false;
        private void Start()
        {
            ObjectToPlace.GetComponent<Renderer>().enabled = false;
        }

        void Update()
        {
            if (Move)
            {
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }

                Vector2 touchPos = Input.GetTouch(0).position;
                Debug.Log("Touched the Screen");
                if (_raycastManager.Raycast(touchPos, hits, TrackableType.Planes))
                {
                    if (hits.Count > 0)
                    {
                        //if (!IsARFloorSet)
                        //{
                        //    IsARFloorSet = true;
                        //}
                        if (ObjectToPlace != null)
                        {
                            ObjectToPlace.GetComponent<Renderer>().enabled = true;
                            ObjectToPlace.transform.position = hits[0].pose.position;
                        }

                    }
                }
            }
        }



        public void FloorMoveToogle()
        {
            if (Move)
            {
                MainParent.transform.position = gameObject.transform.position;
                if (!IsLoadFirstStory)
                {
                    Debug.Log("Load the First Asset");
                    individualAssetHandler.LoadFirstStory();
                    IsLoadFirstStory = true;
                }
                
            }
            Move = !Move;
            
            gameObject.GetComponent<Renderer>().enabled = Move;
            gameObject.GetComponent<BoxCollider>().enabled = Move;
        }
    }
}