using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SphereGen.GuideXR
{
    public class MessageBoxHandler : MonoBehaviour
    {
        [SerializeField]
        private bool DevelopmentBuild;

        [SerializeField]
        private TMP_Text ButtonText;

        [SerializeField]
        private Button Button;

        [SerializeField]
        private TMP_Text DescriptionMsg;

        public delegate void ButtonOnClickEvent();
        

        private void Start()
        {
            gameObject.SetActive(false);

        }

        public void SetMessage(string ActualDescriptionMsgToShow,string ExceptionMsg, string buttonText = "OK", Action Callback=null)
        {
            if (DevelopmentBuild)
            {
                DescriptionMsg.text = ActualDescriptionMsgToShow +" \n "+ExceptionMsg;
            }
            else
            {
                DescriptionMsg.text = ActualDescriptionMsgToShow;
            }
            ButtonText.text = buttonText;
            Button.onClick.AddListener(() => HideMessageBox());
            if(Callback!=null)
            {
                Button.onClick.AddListener(() => Callback());
            }

            gameObject.SetActive(true);


        }

        public void HideMessageBox()
        {
            gameObject.SetActive(false);
        }


    }
}
