using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Replaces the generic ObjectDetector script. Uses the new input manager.
public class VisionOSInputManager : MonoBehaviour
{
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        // Do something if an EnhancedTouch is detected.
        if (Touch.activeTouches.Count > 0 
            && Touch.activeTouches[0].phase == TouchPhase.Began)
        {
            var touchData = EnhancedSpatialPointerSupport.GetPointerState(Touch.activeTouches[0]);
            // If the hand motion in visionOS was the "pinching kind" of touch,
            // then it could qualify as a valid touch input. Pointer input is
            // also valid.
            if (touchData.Kind == SpatialPointerKind.IndirectPinch 
                || touchData.Kind == SpatialPointerKind.IndirectPinch
                || touchData.Kind == SpatialPointerKind.Pointer)
            {
                // TODO: Implement "Object Detection."
                Debug.Log("Detected a visionOS touch.");
            }
        }
    }
}
