using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    public class LookAtCam : MonoBehaviour
    {
       
        private Camera cam;

        void Start()
        {
            cam = IndividualAssetHandler._instance.ARCam;
        }

        // Update is called once per frame
        void Update()
        {           
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
          

        }
    }
}
