using SphereGen.GuideXR;
using Unity.PolySpatial.InputDevices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Replaces the generic ObjectDetector script. Uses the new input package.
public class VisionOSInputManager : MonoBehaviour
{
    // TODO: Implement a more natural manipulation system for
    //       visionOS then the current one for mobile.
    CustomManipulation customManipulation;

    void Awake()
    {
        customManipulation = GetComponent<CustomManipulation>();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        // TODO: Detect multiple touches at once, instead of just one.
        // Do something if an EnhancedTouch is detected.
        if (Touch.activeTouches.Count > 0)
        {
            Touch activeTouch = Touch.activeTouches[0];
            SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouch);
            if (activeTouch.phase == TouchPhase.Began 
                || activeTouch.phase == TouchPhase.Moved)
            {
                // If the hand motion in visionOS was the "pinching kind" of touch,
                // then it could qualify as a valid touch input. Pointer input is
                // also valid.
                if (touchData.Kind == SpatialPointerKind.IndirectPinch 
                    || touchData.Kind == SpatialPointerKind.DirectPinch
                    || touchData.Kind == SpatialPointerKind.Pointer)
                {
                    Debug.Log($"Detected a visionOS touch. Kind: {touchData.Kind}\tPosition: {touchData.deltaInteractionPosition}\tObject: {touchData.targetObject}");
                    // TODO: Store touches, touch data, and selected objects
                    //       in a single object outside of the Update scope.
                    if (touchData.targetObject != null
                        && touchData.targetObject.TryGetComponent(out SelectedObject selectedObject))
                    {
                        customManipulation.HostObjectForManipulation = selectedObject.HostObject.gameObject;
                        ObjectDetector.SelectObject(touchData.targetObject);
                    }
                }
            }
        }
    }
}
