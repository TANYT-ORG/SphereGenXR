using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SGUI;
[ExecuteInEditMode]
public class LayoutTest : MonoBehaviour
{
    //public VerticalLayoutGroup vgBroken, vgWorking;
    public FlexGridLayout flexGrid;
    public Sprite imageToUse;
    public GameObject objectToUse;
    public bool addImage;
    public GameObject ContainerObject;
    RectTransform cRect;
    GameObject newGO;
    GameObject currentWorkingGO;
    // Start is called before the first frame update
    void Start()
    {
        //newGO = new GameObject();
        //newGO.AddComponent<Image>();
        //newGO.GetComponent<Image>().sprite = imageToUse;
        cRect = ContainerObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(addImage)
        {
            currentWorkingGO = Instantiate(objectToUse, flexGrid.transform);

            float newAdjustmentForContainer = currentWorkingGO.GetComponent<RectTransform>().rect.height;
            newAdjustmentForContainer += flexGrid.spacing.y;
            //newAdjustmentForContainer += vgWorking.spacing;
            newAdjustmentForContainer *= currentWorkingGO.transform.parent.localScale.y;
            Debug.Log(newAdjustmentForContainer);
            //set new value, using all old values and only adjusting height by adding our new value as calculated by it's proper scale relative to the container.
            cRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ContainerObject.GetComponent<RectTransform>().rect.height + newAdjustmentForContainer);
            addImage = false;
            
        }
    }
}
