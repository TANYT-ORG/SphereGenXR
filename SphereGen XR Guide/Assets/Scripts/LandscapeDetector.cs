using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeDetector : MonoBehaviour
{
    public GameObject UI_Portrait, UI_Landscape;
    public GameObject ScrollView;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void OnEnable()
    {

        if (UI_Landscape.activeInHierarchy)
        {
            ScrollView.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, 0);
            ScrollView.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, 0);
            ScrollView.GetComponent<RectTransform>().pivot = new Vector2(.5f, 0);
            ScrollView.GetComponent<RectTransform>().transform.localPosition = new Vector3(32, 346f, 0);
            ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 160);
            ScrollView.transform.localScale = new Vector3(4.2f, 4.2f, 4.2f);

        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void CopyAllInfoFromPortraitMode()
    {

    }
}
