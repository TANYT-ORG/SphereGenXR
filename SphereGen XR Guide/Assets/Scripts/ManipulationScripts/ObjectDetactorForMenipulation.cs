using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    public class ObjectDetactorForMenipulation : MonoBehaviour
    {

        private void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = IndividualAssetHandler._instance.ARCam.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider.gameObject.tag == "Model")
                //    CustomManipulation.HostObjectForManipulation = hit.collider.gameObject;               

            }
        }
    }
}
