using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace SphereGen.GuideXR
{

    public class CustomManipulation : MonoBehaviour
    {
        public bool UseBoundsCenter;
        [SerializeField]
        [Tooltip("How quickly the model rotates when dragged")]
        protected float _rotationSpeed = 360f;
        [SerializeField]
        [Tooltip("Max Scale Limit when dragged")]
        protected float _MaxScale = 2f;
        [SerializeField]
        [Tooltip("Min scale Limit when dragged")]
        protected float _MinScale = 0.25f;
        [Tooltip("Minimum change in distance between touches to trigger a scaling interaction")]
        protected float _scalingThreshold = 0.1f;
        [SerializeField]
        private float ScaleCurrent = 2.0f;
        [SerializeField]
        [Tooltip("True to Move And False To Rotate")]
        public bool MoveRotate = false;
        bool isScaling = false;
        float initialTouchDistance = 0f;
        public delegate void OnSelection(bool flag);
        public  event OnSelection OnSelect;
        float initialDistance = 1;
        private Camera arCam;
        private ARRaycastManager m_RaycastManager;
        private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
        private Transform _transformDummyCamera;
        private Transform _transformDummyParent;

        public  GameObject HostObjectForManipulation;
        private Vector3 ScaleChange = new Vector3(-0.01f, -0.01f, -0.01f);

        public delegate void selectDelegate();
        public static event selectDelegate SelectObejct;

      


        protected Transform TransformUserCamera
        {
            get
            {
                if (!Camera.main)
                {
                    Debug.LogError("No Main Camera Found");
                    return null;
                }
                else
                {
                    _transformDummyCamera.position = arCam.transform.position;
                    _transformDummyCamera.rotation = arCam.transform.rotation;
                    _transformDummyCamera.localScale = Vector3.one;
                    return _transformDummyCamera;
                }
            }
        }



        // Start is called before the first frame update
        void Start()
        {
            
            m_RaycastManager = IndividualAssetHandler._instance.m_RaycastManager;
           arCam = IndividualAssetHandler._instance.ARCam;

            if (_transformDummyParent == null)
                _transformDummyParent = new GameObject("_transformDummyParent").transform;
            if (_transformDummyCamera == null)
                _transformDummyCamera = new GameObject("_transformDummyCamera").transform;
           

        }

        public void SwitchRotateMove()
        {
            MoveRotate = !MoveRotate;
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                TouchInput();
            }
        }


        public void TouchInput()
        {


            if (HostObjectForManipulation == null)
                return;

            if (Input.touchCount == 1)
            {
                //if (MoveRotate)
                //{

                //    if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                //    {

                //        if (Input.GetTouch(0).phase == TouchPhase.Moved)
                //        {

                //            HostObjectForManipulation.transform.position = m_Hits[0].pose.position;

                //        }
                //    }
                //}

                //else
                //{

                Touch touchZero = Input.GetTouch(0);
                //check the delta and rotate on an axis based on it. 
                Vector3 rotation = Vector3.zero;

                //x Change/Rotation
                if (Mathf.Abs(touchZero.deltaPosition.x) > 17.5f)
                {
                    rotation.y = _rotationSpeed * Time.deltaTime;
                    rotation.y *= Mathf.Sign(touchZero.deltaPosition.x);
                    rotation.y *= -1f;
                }

                //y Change/Rotation
                if (Mathf.Abs(touchZero.deltaPosition.y) > 17.5f)
                {
                    rotation.x = _rotationSpeed * Time.deltaTime;
                    rotation.x *= Mathf.Sign(touchZero.deltaPosition.y);
                }

                //apply the manipulation
                ManipulateRotation(Quaternion.Euler(rotation), false);
                //  }
            }
            else if (Input.touchCount == 2)
            {
                // 2 Finger 
                // Scale
                Touch firstFinger, secondFinger;

                firstFinger = Input.GetTouch(0);
                secondFinger = Input.GetTouch(1);


                Vector2 touchZeroLastPosition = firstFinger.position - firstFinger.deltaPosition;
                Vector2 touchOneLastPosition = secondFinger.position - secondFinger.deltaPosition;

                //calculate what the fingers are doing, based on current position and their last known positions.
                float magnitudeLastTouchDelta = (touchZeroLastPosition - touchOneLastPosition).magnitude;

                //Current Position
                float magnitudeCurrentTouchDelta = (firstFinger.position - secondFinger.position).magnitude;

                float magnitudeDelta = magnitudeLastTouchDelta - magnitudeCurrentTouchDelta;




                if ((Mathf.Sign(firstFinger.deltaPosition.y) == Mathf.Sign(secondFinger.deltaPosition.y)) && (Mathf.Sign(firstFinger.deltaPosition.x) == Mathf.Sign(secondFinger.deltaPosition.x)))
                {

                    Vector3 velocity = Vector3.zero;
                    float moveSpeed = .05f;

                    float xChange = (firstFinger.deltaPosition.x + secondFinger.deltaPosition.x) / 2f;
                    float yChange = (firstFinger.deltaPosition.y + secondFinger.deltaPosition.y) / 2f;

                    if (Mathf.Abs(xChange) > 10f)
                        velocity.x = xChange;
                    if (Mathf.Abs(yChange) > 10f)
                        velocity.y = yChange;

                    velocity *= Time.deltaTime * moveSpeed;


                    HostObjectForManipulation.transform.Translate(velocity, Space.World);

                    //_text.text = "Translate";

                }

                //2 fingers moving in opposite directions (spreading out / away , or getting closer/towards each other) means we scale.
                //if the magnitude is positive, we should scale down, negative = Up.
                else if ((Mathf.Sign(firstFinger.deltaPosition.y) != Mathf.Sign(secondFinger.deltaPosition.y)) && (Mathf.Sign(firstFinger.deltaPosition.x) != Mathf.Sign(secondFinger.deltaPosition.x)))
                {
                    //scale based on the magnitude.

                    //determine how much the scale will be adjusted by.

                    float scaleIncrement = (.002f * Mathf.Abs(magnitudeDelta) * Mathf.Sign(magnitudeDelta) * -1f);//consistent based on amount moved.
                    float scale = ScaleCurrent + scaleIncrement;
                    ManipulateScale(scale);
                }





                //if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
                //{
                //    firstFiinger = Input.GetTouch(0).position;
                //    secondFinger = Input.GetTouch(1).position;
                //    //initialDistance = Vector2.Distance(firstFiinger, secondFinger);
                //  //  ScaleChange = transform.localScale;
                //    Debug.Log("Initialize");
                //}
                //else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                //{
                //    float CurrentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                //   // float diff = CurrentDistance - initialDistance;
                //    var factor = (CurrentDistance / initialDistance)/4;
                //    Debug.Log(initialDistance+ " Fac  "+CurrentDistance+"   tor: " + factor);
                //    Debug.Log("Scale change: " + ScaleChange);
                //    //if (CurrentDistance > initialDistance)
                //    //    Debug.Log("scale up ");
                //    //else
                //    //    Debug.Log("scale Down ");
                //    HostObjectForManipulation.transform.localScale =( ScaleChange * factor);

                //}
            }

            //if (Input.GetTouch(0).phase == TouchPhase.Ended)
            //{
            //   // HostObjectForManipulation = null;

            //}

        }

        public virtual void ManipulateScale(float scalePercent)
        {
            
            //lets determine what our scale is...
            //make sure we're witin the set limit
            ScaleCurrent = Mathf.Clamp(scalePercent, _MinScale, _MaxScale);
            //get our vector scale
            Vector3 scale = ScaleCurrent * HostObjectForManipulation.GetComponent<ManipulatedHostObject>().InitialTransform.ScaleWorld;
            //store the current values (parent specifically)
           SGModelSizeUtility.TransformValues currentValues = new SGModelSizeUtility.TransformValues(HostObjectForManipulation.transform);
            HostObjectForManipulation.transform.SetParent(null);
            //update the scale
            HostObjectForManipulation.transform.localScale = scale;
            //set the parent
            currentValues.ApplyParent(HostObjectForManipulation.transform);  
        }


        public void ManipulateRotation(Quaternion rotation, bool rotateOnGlobalAxis)
        {
            //store current values (specifically the parent)
           SGModelSizeUtility.TransformValues currentValues = new SGModelSizeUtility.TransformValues(HostObjectForManipulation.transform);

            //get the dummy in position
            if(UseBoundsCenter)
            {
                MeshRenderer mr = HostObjectForManipulation.GetComponent<MeshRenderer>();
                Bounds bounds = mr.bounds;
                Vector3 center = transform.TransformPoint(bounds.center);
                _transformDummyParent.position = center;
            }
            else
            {
                _transformDummyParent.position = HostObjectForManipulation.transform.position;
            }
            //set its parent to the camera so we can neutralize the rotation.
            _transformDummyParent.SetParent(TransformUserCamera);

            if (rotateOnGlobalAxis)
            {
                //neutralize / zero out the rotation
                _transformDummyParent.rotation = Quaternion.identity;
            }
            else
            {
                //neutralize / zero out the rotation
                _transformDummyParent.localRotation = Quaternion.identity;
            }

            //add the host transform to the dummy parent.
            HostObjectForManipulation.transform.SetParent(_transformDummyParent);

            if (rotateOnGlobalAxis)
            {
                //rotate the dummy parent
                _transformDummyParent.rotation = rotation;
            }
            else
            {
                //rotate the dummy parent
                _transformDummyParent.localRotation = rotation;
            }

            //restore the host transform parent
            currentValues.ApplyParent(HostObjectForManipulation.transform);
            //unparent the dummy
            _transformDummyParent.SetParent(null);

            // Debug.Log(HostObjectForManipulation.name + "Has been rotated");
            //If Require Freeze Inputs till this is not complete. Think Something Later
        }

        private void OnDestroy()
        {
            if (_transformDummyCamera)
                Destroy(_transformDummyCamera);
            if (_transformDummyParent)
                Destroy(_transformDummyParent);
        }



    }
}
