using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSession))]
public class ResetSceneAndARSession : MonoBehaviour
{
    [SerializeField]
    ARSession arSesh;
    int touchStartCount = 0;

    private void Awake()
    {
        arSesh = GetComponent<ARSession>();
    }

    // Update is called once per frame
    void Update()
    {
        //Trigger Test Function after two finger push ended
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                touchStartCount = 2;
            }

            // Check if both touches ended in this frame
            if (touch1.phase == TouchPhase.Ended && touch2.phase == TouchPhase.Ended)
            {
                if (touchStartCount == 2)
                {
                    TestReset();
                }
            }
        }
    }

    void TestReset()
    {
        arSesh.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
