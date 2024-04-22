using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    public class StepAssetPlacer : MonoBehaviour
    {
        [SerializeField] private StepAssetList gameObjectsList;
        GameObject[] _go = null;
        AnchoringController _ac;
        bool setup = false;
        public void Start()
        {
            InitialSetup();
        }
        public void InitialSetup()
        {
            _go = new GameObject[gameObjectsList.gameObjects.Length];
            if(_go.Length == 0)
            {
                _go = null;
                return;
            }
            int i = 0;
            foreach (var gameObjectInfo in gameObjectsList.gameObjects)
            {
                _go[i] = CheckIfExists(gameObjectInfo.gameObject);
                if (_go[i] != null)
                {
                    _go[i].SetActive(true);
                    continue;
                }
                if (gameObjectInfo.instantiateWithAnchor)
                {
                    _go[i] = Instantiate(gameObjectInfo.gameObject, Vector3.zero, Quaternion.identity);
                    _ac = _go[i].AddComponent<AnchoringController>();
                    _ac.PlaceAtAnchor();
                }
                else
                {
                    _go[i] = Instantiate(gameObjectInfo.gameObject, transform.position, transform.rotation);
                }
            }
            _ac = null;
            setup = true;
        }

        public GameObject CheckIfExists(GameObject go)
        {
            foreach(ReusableContent rc in FindObjectsOfType<ReusableContent>(true))
            {
                if(rc.gameObject.name == go.name + "(Clone)")
                {
                    return rc.gameObject;
                }
            }
            return null;
        }

        public void OnDisable()
        {
            DisableStepAssets();
        }

        public void OnEnable()
        {
            if(setup)
            {
                ReEnableStepAssets();
            }
        }

        public void DisableStepAssets()
        {
            if(_go != null)
            {
                foreach (GameObject go in _go)
                {
                    go.SetActive(false);
                }
            }    
        }

        public void ReEnableStepAssets()
        {
            if (_go != null)
            {
                foreach (GameObject go in _go)
                {
                    go.SetActive(true);
                    _ac = go.GetComponent<AnchoringController>();
                    if (_ac != null)
                    {
                        _ac.PlaceAtAnchor();
                    }
                }
            }
        }

        public void ResetAssets()
        {
            //need to figure out how to handle issues with reusable content....
            foreach(GameObject go in _go)
            {
                Destroy(go);
            }
            InitialSetup();
        }
    }

}
