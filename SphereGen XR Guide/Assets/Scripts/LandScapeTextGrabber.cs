using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;
using TMPro;
public class LandScapeTextGrabber : MonoBehaviour
{
    private TextMeshProUGUI BodyText;
    private TextMeshProUGUI HeaderText;
    // Start is called before the first frame update
    void Start()
    {
        BodyText = GameObject.Find("Instruction Card Text").GetComponent<TextMeshProUGUI>();
        HeaderText = GameObject.Find("HeaderText").GetComponent<TextMeshProUGUI>();
        // BodyText.text = PBody.GetComponent<TextMeshProUGUI>().text.ToString();
        BodyText.text = GetComponentInParent<CardDisplay>().Card.BodyText.ToString();
        HeaderText.text = GetComponentInParent<CardDisplay>().Card.TitleText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
