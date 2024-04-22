using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    [ExecuteInEditMode]
    public class UpdateLineRendererInEditor : MonoBehaviour
    {
        [SerializeField]
        public GameObject Point1, Point2;
        void Awake()
        {
           
        }

        void Update()
        {
            if (Point1 != null && Point2 != null)
            {
                gameObject.GetComponent<LineRenderer>().SetPosition(0, Point1.transform.position);
                gameObject.GetComponent<LineRenderer>().SetPosition(1, Point2.transform.position);
            }
        }
    }
}
