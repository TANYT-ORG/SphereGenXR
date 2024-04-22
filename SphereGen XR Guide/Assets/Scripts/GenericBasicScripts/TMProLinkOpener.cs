using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class TMProLinkOpener : MonoBehaviour, IPointerClickHandler
{
    public string LinkToUse;
    public void OnLinkSelection(TMP_Text text, TMP_LinkInfo linkInfo)
    {
        Debug.Log(linkInfo);
        // Get the URL from the link tag
        //string url = linkInfo.GetLinkID();

        // Open the URL in the device's default web browser
       // Application.OpenURL(url);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text textComponent = eventData.pointerPress.GetComponent<TMP_Text>();
        if (textComponent == null)
        {
            return;
        }

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textComponent, eventData.position, eventData.pressEventCamera);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textComponent.textInfo.linkInfo[linkIndex];
            string linkText = linkInfo.GetLinkID();
            string pattern = @"(?:http|https):\/\/[\w\-]+(?:\.[\w\-]+)+[\w\-\._~:\/\?#\[\]@!\$&'\(\)\*\+,;=]+";
            Match match = Regex.Match(linkText, pattern);
            string linkUrl = null;
            if (match.Success)
            {
                 linkUrl = match.Value;
            }
            else
            {
                match = Regex.Match(LinkToUse, pattern);
                if(match.Success)
                {
                    linkUrl = match.Value;
                }
            }

            if(linkUrl != null)
            {
                Application.OpenURL(linkUrl);
            }
        }
    }
}
