using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    public class ManipulatedHostObject : MonoBehaviour
    {
        public SelectedObject selectedObject;
        public SGModelSizeUtility.TransformValues InitialTransform;

        [Tooltip("True to Move And False To Rotate")]
        public bool RotateTranslateToggle = false; //
        private void Awake()
        {
            InitialTransform = new SGModelSizeUtility.TransformValues(gameObject.transform);
        }

        // Start is called before the first frame update
        void Start()
        {
           
        }

        public void SetToInitialPosition()
        {
            gameObject.transform.position = InitialTransform.Position;
            gameObject.transform.rotation = InitialTransform.Rotation;
            gameObject.transform.localScale = InitialTransform.ScaleLocal;
        }
    }
}
