using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UISceneManager : MonoBehaviour
{

    public GameObject[] UIScenes;
    public int numScene;
    public int getPortraitNum = 0, getLandscapeNum =0;
    private int lastscene;
    public GameObject ReAnchorScene;
    public PlaceOnPlane POPScript;
    public GameObject AnchorAndResetCanvas;
    public float StepTimer;
    public float ClockTimer;
    public bool LandScapeMode, PortraitMode;
    private bool allDone = false, allDone2 = false;
    public GameObject AnchoringScene_Portrait, AnchoringScene_Landscape;
    public GetTimerText GTTScript;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in UIScenes)
        {
            go.SetActive(false);
            UIScenes[numScene].SetActive(true);
            numScene = 0;
        }
       

    }

    private void OnEnable()
    {
        //AnchorAndResetCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        StepTimer += Time.deltaTime;

        if (Screen.orientation == ScreenOrientation.Portrait)
        {
           
            PortraitMode = true;
            SwitchToPortrait();
            AnchoringScene_Landscape.SetActive(false); //Quick fix
            AnchoringScene_Portrait.SetActive(true);
            getPortraitNum = 0;
            allDone = false;
        }

        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            
            LandScapeMode = true;
            SwitchToLandScape();
            AnchoringScene_Landscape.SetActive(true);
            AnchoringScene_Portrait.SetActive(false);
            getLandscapeNum = 0;
            allDone2 = false;
        }
    }

    public void SwitchToPortrait()
    {
        if (PortraitMode && allDone == false)
        {
            foreach (GameObject go in UIScenes)
            {
                UIScenes[getPortraitNum].gameObject.transform.Find("ScreenUI_Portrait").gameObject.SetActive(true);
                UIScenes[getPortraitNum].gameObject.transform.Find("ScreenUI_Landscape").gameObject.SetActive(false);
                
                getPortraitNum++;
                if(getPortraitNum >= 11)
                {
                    allDone = true;
                }
            }

            GTTScript.MoveTextToPortrait();
        }
        else
        {
     
        }
    }

    public void SwitchToLandScape()
    {
        if (LandScapeMode == true && allDone2 == false)
        {
            foreach (GameObject go in UIScenes)
            {
                UIScenes[getLandscapeNum].gameObject.transform.Find("ScreenUI_Portrait").gameObject.SetActive(false);
                UIScenes[getLandscapeNum].gameObject.transform.Find("ScreenUI_Landscape").gameObject.SetActive(true);
                
                getLandscapeNum++;

                if (getLandscapeNum >= 11)
                {
                    allDone2 = false;
                }
            }

            GTTScript.MoveTextToLandscape();
        }
        else
        {

        }
    }

    public void MoveForwardInScene()
    {
        numScene++;
        foreach (GameObject go in UIScenes)
        {
            go.SetActive(false);
            UIScenes[numScene].SetActive(true);
        }
        ClockTimer = Mathf.Round(StepTimer * 100f) / 100f;
        StepTimer = 0f;
        UIScenes[numScene].gameObject.GetComponent<CardDisplay>().TimerUI.text = UIScenes[numScene].gameObject.name + ": "+ ClockTimer + " s";
    }

    public void MoveBackInScene()
    {
        numScene--;
        foreach (GameObject go in UIScenes)
        {
            go.SetActive(false);
            UIScenes[numScene].SetActive(true);
        }
    }

    public void Reset()
    {
        StepTimer = 0f;
        numScene = 1;
        foreach (GameObject go in UIScenes)
        {
            go.SetActive(false);
            UIScenes[numScene].SetActive(true);
            UIScenes[numScene].gameObject.GetComponent<CardDisplay>().TimerUI.text = UIScenes[numScene].gameObject.name + ": " + 0 + " s";
        }
    }

    public void GrabLastScene()
    {
        lastscene = numScene;
        POPScript.ArPlaneManager.enabled = true;
        foreach (GameObject go in UIScenes)
        {
            go.SetActive(false);
        }
        ReAnchorScene.SetActive(true);
        Debug.Log(lastscene);
    }

    public void ConfirmReAnchorPoint()
    {
        ReAnchorScene.SetActive(false);
        UIScenes[lastscene].SetActive(true);
        POPScript.ArPlaneManager.enabled = false;
    }

    public void TurnOnAnchorAndReset()
    {
        AnchorAndResetCanvas.SetActive(true);
    }
}
