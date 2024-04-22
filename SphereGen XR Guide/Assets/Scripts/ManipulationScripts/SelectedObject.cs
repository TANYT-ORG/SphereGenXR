using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    [RequireComponent(typeof(BoxCollider))]
    public class SelectedObject : MonoBehaviour
    {
               
        [SerializeField]
        [Tooltip("the model to be Manipulated when Touched and dragged and pinced in/out to scale")]
        public Transform HostObject;

        private void Awake()
        {
            if (!HostObject)
            {
                HostObject = gameObject.transform;
                Debug.LogWarning("Host Set to Self as Host. It has a parent named: " + HostObject.parent.name);
            }
            HostObject.gameObject.AddComponent<ManipulatedHostObject>();
            HostObject.gameObject.GetComponent<ManipulatedHostObject>().selectedObject = this;
            OnObjectSelect(false);
        }

        private void Start()
        {
          
            //Debug.LogWarning("Hash: " + gameObject.GetHashCode());
            foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }
            ObjectDetector.SelectableObjects.Add(gameObject);
        }

        public void OnObjectSelect(bool flag)
        {
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = flag;
            }
        }
    
       public void ResetHostTransform()
        {
            HostObject.GetComponent<ManipulatedHostObject>().SetToInitialPosition();
        }

        public void OnDestroy()
        {
            ObjectDetector.SelectableObjects.Remove(gameObject);
        }


    }
}
