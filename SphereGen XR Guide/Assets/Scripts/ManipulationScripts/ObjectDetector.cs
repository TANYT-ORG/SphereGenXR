using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    [RequireComponent(typeof(CustomManipulation))]
    public class ObjectDetector : MonoBehaviour
    {
        public static HashSet<GameObject> SelectableObjects = new HashSet<GameObject>();
        CustomManipulation manipulationScript;

        public void Start()
        {
            manipulationScript = gameObject.GetComponent<CustomManipulation>();
        }
        private void Update()
        {

            if (Input.touchCount == 1)
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    RaycastHit hit;
                    // FIXME: Something here's causing a NullReferenceException. Possibly due to
                    //        the new Input Manager package?
                    Ray ray = IndividualAssetHandler._instance.ARCam.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        // if (hit.collider.gameObject.tag == "Model")
                        if (hit.collider.gameObject.GetComponent<SelectedObject>())
                        {
                            SelectedObject newItem = hit.collider.gameObject.GetComponent<SelectedObject>();
                            Debug.Log("hit.: " + hit.collider.gameObject.name);
                            manipulationScript.HostObjectForManipulation = newItem.HostObject.gameObject;
                            SelectObject(hit.collider.gameObject);
                        }

                    }
                }
        }


        public static void DeselectAll()
        {
            foreach (var item in SelectableObjects)
            {
                item.GetComponent<SelectedObject>().OnObjectSelect(false);
            }
        }

        public void SelectObject(GameObject HitObject)
        {
            DeselectAll();
            HitObject.GetComponent<SelectedObject>().OnObjectSelect(true);
        }
    }
}
