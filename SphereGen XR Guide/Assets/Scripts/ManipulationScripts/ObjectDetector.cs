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
            manipulationScript = GetComponent<CustomManipulation>();
        }
        private void Update()
        {
            if (Input.touchCount == 1)
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // FIXME: Something here's causing a NullReferenceException. Possibly due to
                    //        the new Input System package?
                    // FIXME: Throws an UnassignedReferenceException due to ARCam not being
                    //        initalized for visionOS.
                    Debug.Log($"IndividualAssetHandler GameObject Name: {IndividualAssetHandler._instance.gameObject.name}");
                    Debug.Log($"Is IndividualAssetHandler Singleton Null? {IndividualAssetHandler._instance == null}");
                    Debug.Log($"Is IndividualAssetHandler ARCam Null? {IndividualAssetHandler._instance.ARCam == null}");
                    Ray ray = IndividualAssetHandler._instance.ARCam.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
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

        public static void SelectObject(GameObject HitObject)
        {
            DeselectAll();
            HitObject.GetComponent<SelectedObject>().OnObjectSelect(true);
        }
    }
}
