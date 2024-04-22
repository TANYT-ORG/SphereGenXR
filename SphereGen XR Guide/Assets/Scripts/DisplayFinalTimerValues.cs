using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SphereGen.GuideXR;
using SGUI;
using TMPro;

[RequireComponent(typeof(FlexGridLayout))]
public class DisplayFinalTimerValues : MonoBehaviour
{
    [SerializeField]
    GameObject ElementToInsert, ContentContainer;
    IndividualAssetHandler _iah;
    RectTransform _cRect;
    GameObject _goClone;
    FlexGridLayout _flexLayout;
    // Start is called before the first frame update
    void Start()
    {
        _iah = IndividualAssetHandler._instance;
        _flexLayout = GetComponent<FlexGridLayout>();
        _cRect = ContentContainer.GetComponent<RectTransform>();
        AddElementToFlexGrid("Here are some measurements of how long you took on each step of this demonstration.");
        List<string> results = _iah.GetTimerResults();
        AddElementToFlexGrid("You spent an average of " + _iah.GetTimerAverage() + " seconds on each part.");
        //strip the last result because it's for this actual step
        for(int i = 0; i < results.Count-1; i++)
        {
            AddElementToFlexGrid(results[i]);
        }
    }

    void AddElementToFlexGrid(string s)
    {
        _goClone = Instantiate(ElementToInsert, _flexLayout.transform);

        float newAdjustmentForContainer = _goClone.GetComponent<RectTransform>().rect.height;
        newAdjustmentForContainer += _flexLayout.spacing.y;
        //newAdjustmentForContainer += vgWorking.spacing;
        newAdjustmentForContainer *= _goClone.transform.parent.localScale.y;
        Debug.Log(newAdjustmentForContainer);
        //set new value, using all old values and only adjusting height by adding our new value as calculated by it's proper scale relative to the container.
        _cRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ContentContainer.GetComponent<RectTransform>().rect.height + newAdjustmentForContainer);
        _goClone.GetComponent<TextMeshProUGUI>().text = s;
    }
}
