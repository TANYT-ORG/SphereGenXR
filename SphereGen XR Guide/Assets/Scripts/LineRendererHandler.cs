using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(UpdateLineRendererInEditor))]
    public class LineRendererHandler : MonoBehaviour
    {

        private GameObject Point1, Point2;

        private void Start()
        {
            //gameObject.GetComponent<LineRenderer>().SetPosition(0, Point1.transform.position);
            //gameObject.GetComponent<LineRenderer>().SetPosition(1, Point2.transform.position);
            Point1 = gameObject.GetComponent<UpdateLineRendererInEditor>().Point1;
            Point2 = gameObject.GetComponent<UpdateLineRendererInEditor>().Point2;


        }

        private void Update()
        {
            if (Point1 != null && Point2 != null)
            {
                gameObject.GetComponent<LineRenderer>().SetPosition(0, Point1.transform.position);
                gameObject.GetComponent<LineRenderer>().SetPosition(1, Point2.transform.position);
            }

        }

    }
}
