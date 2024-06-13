using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Reflection;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace SphereGen.GuideXR
{
    public class AssetTimer
    {
        public AssetReference AssetReference { get; private set; }
        public float Timer { get; set; }

        public AssetTimer(AssetReference assetReference)
        {
            AssetReference = assetReference;
            Timer = 0f;
        }
    }

    public sealed class IndividualAssetHandler : MonoBehaviour
    {
        public ARRaycastManager m_RaycastManager;
        public static IndividualAssetHandler _instance { get; private set; }
        public Camera ARCam;
        public GameObject MainParent;
        public GameObject AnchorObject;
        List<MeshRenderer> anchorMRs;
        // public AssetReference previousRef,currentRef;
        //[SerializeField]
        //public List<AssetReference> AssetRefInOrder = new List<AssetReference>();
        public StoryBoardScriptablesObjects storyBoardScriptableObject;
        private readonly Dictionary<AssetReference, GameObject> _AsyncAssetHandleDic = new Dictionary<AssetReference, GameObject>();

        public MessageBoxHandler messageBoxHandler;

        public AssetReference PreviousAssetReference = null, CurrentAssetReference = null;
        public bool firstAssetIsImageScan = false;
        public Stack<AssetReference> previousStackRef = new Stack<AssetReference>();

        bool _didInitialPlacement = false;
        ARTrackedImageManager _trackedImageManager;
        ARPlaneManager _arPlaneManager;
        int _wentBackAStep = 0;
        List<AssetTimer> _metricTimers;
        float averageTime;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }

            if (firstAssetIsImageScan)
            {
                _trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
                _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            }
            _arPlaneManager = FindObjectOfType<ARPlaneManager>();
            _metricTimers = new List<AssetTimer>();
        }


        private void Start()
        {
            LoadFirstStory();
            //if (CurrentAssetReference == null)
            //{
            //    Debug.Log("Load the First Asset");
            //}
            //else
            //{
            //    Debug.Log("Its Not the First : " + CurrentAssetReference.SubObjectName);
            //}
        }

        public void LoadFirstStory()
        {
            /*if (storyBoardScriptableObject.AssetRefInOrder.Count > 0)
            {
                LoadAddressableAsset(0); //First index
            }*/
        }
        public void LoadAddressableAsset(int AssetRefIndex)
        {
            try
            {

                CurrentAssetReference = storyBoardScriptableObject.AssetRefInOrder[AssetRefIndex];
                if (_AsyncAssetHandleDic.ContainsKey(CurrentAssetReference))
                {
                    //Asset already loaded so just make it visible
                    HideAndShow(CurrentAssetReference,  true);
                }
                else
                {
                    CurrentAssetReference.InstantiateAsync().Completed += (obj) =>
                    {
                        _AsyncAssetHandleDic.Add(CurrentAssetReference, obj.Result);
                        obj.Result.transform.SetParent(MainParent.transform);

                        //obj.Result.GetComponent<StoryHandler>().ThisStoryBoardReference = AR;
                        //var aa = obj.Result.GetComponentsInChildren<IButtonInteract>();
                        //foreach (var item in aa)
                        //{
                        //    item.SetAssetHandler(AR);
                        //}
                        //HideAndShow(AR);
                        if(_didInitialPlacement && anchorMRs[0].enabled)
                        {
                            foreach(MeshRenderer mr in anchorMRs)
                            {
                                mr.enabled = false;
                            }
                            _trackedImageManager.enabled = false;
                            GravityAlignment();
                        }
                        AssetTimer aTimer = new AssetTimer(CurrentAssetReference);
                        _metricTimers.Add(aTimer);
                    };

                }

            }
            catch (Exception e)
            {

                Debug.LogError(" error2  " + e);
                messageBoxHandler.SetMessage("Error 2: ",e.ToString());
            }

       
        }

        public void ConfirmAnchor()
        {

        }

        public void LoadPreviousStoryboard()
        {
            if (previousStackRef.Count > 0)
            {
                int index = GetAssetRefIndex(previousStackRef.Pop());
                HideAndShow(CurrentAssetReference, false);
                LoadAddressableAsset(index);
                AudioHandler.Instance.PlayAudio("PreviousButton");
                _wentBackAStep++;
            }


        }
        public void LoadNextStoryBoard()
        {
            if (CurrentAssetReference != null)
            {
                int index = GetAssetRefIndex(CurrentAssetReference);
                index++;
                if (index < storyBoardScriptableObject.AssetRefInOrder.Count)
                {
                    previousStackRef.Push(CurrentAssetReference);
                    HideAndShow(CurrentAssetReference, false);
                    LoadAddressableAsset(index);
                    AudioHandler.Instance.PlayAudio("NextButton");
                }
                else
                {
                    Debug.LogError("Out Of Index");
                }
            }
            else
                Debug.LogError("No Current Asset Ref");
        }
        public void HideAndShow(AssetReference AR, bool isVisible)
        {
            GameObject storyBoard;
            if (_AsyncAssetHandleDic.TryGetValue(AR, out storyBoard))
            {
                storyBoard.SetActive(isVisible);
            }
        }

        public int GetAssetRefIndex(AssetReference AR)
        {
            return storyBoardScriptableObject.AssetRefInOrder.FindIndex(a => a == AR);
        }

        public void Update()
        {
            CheckAssetTimers();
            #region editorTesting
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.Keypad3))
            {
                LoadNextStoryBoard();
            }
            if (Input.GetKeyUp(KeyCode.Keypad1))
            {
                LoadPreviousStoryboard();
            }
            if (Input.GetKeyUp(KeyCode.Keypad5))
            {
                AnchorObject = Instantiate(_trackedImageManager.trackedImagePrefab, MainParent.transform);
                AnchorObject.name = _trackedImageManager.trackedImagePrefab.name;
                AnchorObject.transform.position = MainParent.transform.position;
                AnchorObject.transform.rotation = MainParent.transform.rotation;
                anchorMRs = new List<MeshRenderer>();
                foreach (MeshRenderer mr in AnchorObject.GetComponentsInChildren<MeshRenderer>())
                {
                    anchorMRs.Add(mr);
                }
                _didInitialPlacement = true;
            }
            if (Input.GetKeyUp(KeyCode.Keypad8))
            {
                foreach(string s in GetTimerResults())
                {
                    Debug.Log(s);
                }
            }
#endif
                #endregion
        }
        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (!_didInitialPlacement)
            {
                if (eventArgs.added.Count > 0)
                {
                    //AnchorObject = Instantiate(_trackedImageManager.trackedImagePrefab, MainParent.transform);
                    AnchorObject = eventArgs.added[0].gameObject;
                    AnchorObject.name = _trackedImageManager.trackedImagePrefab.name;
                    AnchorObject.transform.parent = MainParent.transform;
                    GravityAlignment();

                    //AnchorObject.transform.position = eventArgs.added[0].transform.position;
                    //AnchorObject.transform.rotation = eventArgs.added[0].transform.rotation;
                    _didInitialPlacement = true;
                    anchorMRs = new List<MeshRenderer>();
                    foreach(MeshRenderer mr in AnchorObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        anchorMRs.Add(mr);
                    }
                    Debug.LogFormat("{0} placed.", AnchorObject.name);
                    AudioHandler.Instance.PlayAudio("AnchorFound");
                }
            }
            else if(anchorMRs[0].enabled)
            {
                AnchorObject.transform.position = eventArgs.updated[0].transform.position;
                AnchorObject.transform.rotation = eventArgs.updated[0].transform.rotation;
                GravityAlignment();
            }
        }    

        public void ToggleAnchorObject()
        {
            if (anchorMRs[0].enabled)
            {
                foreach (MeshRenderer mr in anchorMRs)
                {
                    mr.enabled = false;
                }
                _trackedImageManager.enabled = false;
                GravityAlignment();
                HideAndShow(CurrentAssetReference, true);
            }
            else
            {
                foreach (MeshRenderer mr in anchorMRs)
                {
                    mr.enabled = true;
                }
                HideAndShow(CurrentAssetReference, false);
                _trackedImageManager.enabled = true;
            }
            AudioHandler.Instance.PlayAudio("UpdateAnchor");
        }
        private ARTrackedImage[] CreateTrackedImages(int count)
        {
            ARTrackedImage[] trackedImages = new ARTrackedImage[count];

            for (int i = 0; i < count; i++)
            {
                GameObject imageGO = new GameObject("Image" + (i + 1));
                ARTrackedImage trackedImage = imageGO.AddComponent<ARTrackedImage>();
                trackedImages[i] = trackedImage;
            }

            return trackedImages;
        }
        public void OnDestroy()
        {
            if(_trackedImageManager != null)
            {
                _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
        }

        public void ResetScenario()
        {
            ARSession arSession = FindObjectOfType<ARSession>();
            if (arSession)
            {
                Debug.Log("Resetting AR Session!");
                arSession.Reset();
                AudioHandler.Instance.PlayAudio("AnchorFound");
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void CheckAssetTimers()
        {
            // Update the timers for each loaded asset reference
            foreach (AssetTimer assetTimer in _metricTimers)
            {
                if (_AsyncAssetHandleDic.ContainsKey(assetTimer.AssetReference))
                {
                    if (_AsyncAssetHandleDic[assetTimer.AssetReference].activeInHierarchy)
                    {
                        assetTimer.Timer += Time.deltaTime;
                    }
                }
            }
        }
        public List<string> GetTimerResults()
        {
            List<string> results = new List<string>();

            foreach (AssetTimer assetTimer in _metricTimers)
            {
                AssetReference assetReference = assetTimer.AssetReference;
                float timer = assetTimer.Timer;
                GameObject go;
                _AsyncAssetHandleDic.TryGetValue(assetReference, out go);
                TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.RoundToInt(timer));
                string goName = go.name;
                string cloneSuff = "(Clone)";
                if(goName.EndsWith(cloneSuff))
                {
                     goName = goName.Substring(0, goName.Length - cloneSuff.Length);
                }
                averageTime += timeSpan.Seconds;
                string result = $"{goName}: {timeSpan.Seconds} seconds";
                results.Add(result);
            }

            return results;
        }

        public int GetTimerAverage()
        {
            return (int)(averageTime / _metricTimers.Count);
        }


        public void GravityAlignment()
        {
            ARPlane horizontalPlane = FindHorizontalPlane();

            if (horizontalPlane != null)
            {
                Vector3 gravityDirection = horizontalPlane.normal;
                Quaternion gravityRotation = Quaternion.FromToRotation(AnchorObject.transform.up, gravityDirection);

                AnchorObject.transform.rotation = gravityRotation;
            }
            //Vector3 gravityDirection = Input.gyro.gravity.normalized;
            //Quaternion gravityRotation = Quaternion.FromToRotation(Vector3.up, gravityDirection);

            //AnchorObject.transform.rotation = gravityRotation;
        }

        private ARPlane FindHorizontalPlane()
        {
            foreach (ARPlane plane in _arPlaneManager.trackables)
            {
                if (plane.alignment == PlaneAlignment.HorizontalUp)
                {
                    return plane;
                }
            }

            return null;
        }
    }
}
