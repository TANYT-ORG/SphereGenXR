using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingHandler : MonoBehaviour
{
    [SerializeField] private GameObject anchoredObjectPrefab;

    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            SpawnAnchoredObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                SpawnAnchoredObject(trackedImage);
            }
        }
    }

    private void SpawnAnchoredObject(ARTrackedImage trackedImage)
    {
        GameObject anchoredObject = Instantiate(anchoredObjectPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
        anchoredObject.transform.parent = trackedImage.transform;
    }
}
