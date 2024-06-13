using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    UnityEvent placementUpdate;

    [SerializeField]
    GameObject visualObject;
    public GameObject Panel;
    public GameObject PrefabAtAnchorSpot;
    public ARPlaneManager ArPlaneManager;
    public Vector3 AnchorPosForModels;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    public void Start()
    {
        EventManager.onAnchorPlaced += ShowConfirmPopUp;
        ArPlaneManager = GetComponent<ARPlaneManager>();
        placementUpdate.AddListener(DiableVisual);

    }
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null)
            placementUpdate = new UnityEvent();

        placementUpdate.AddListener(DiableVisual);
        Debug.Log($"PlaceOnPlane Object Name: { gameObject.name }");
        ArPlaneManager.enabled = true;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void OnDestroy()
    {
        EventManager.onAnchorPlaced -= ShowConfirmPopUp;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    

                }

                else
                {
                    spawnedObject.transform.position = hitPose.position;

                }
                placementUpdate.Invoke();

            }

        }
    }

    public void ShowConfirmPopUp()
    {
        //Panel.SetActive(true);
    }

    public void UserConfirmedAnchor()
    {
        //Panel.SetActive(false);
        spawnedObject.SetActive(false);
        //Instantiate(PrefabAtAnchorSpot, spawnedObject.transform.localPosition, Quaternion.identity);
        AnchorPosForModels = spawnedObject.transform.localPosition;
        Debug.Log(AnchorPosForModels + "Confirmed");
        ArPlaneManager.enabled = false;
        Debug.Log("Disable AR Plane");
    }

    public void DiableVisual()
    {
        visualObject.SetActive(false);
    }

    public void EnableVisual()
    {
        spawnedObject.SetActive(true);
        visualObject.SetActive(true);
    }


    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}