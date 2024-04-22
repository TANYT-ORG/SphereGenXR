using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.UI;
public class CardDisplay : MonoBehaviour
{
    public StepCardSO Card;

    public TextMeshProUGUI TitleTextUI;
    //  public TextMeshProUGUI StepCardUI;
    public TextMeshProUGUI BodyTextUI;
    public TextMeshProUGUI TimerUI;
    // public GameObject ImageUI;
    private Sprite imageUiSprite;
    public SOHolder SOHolderScript;
    public GameObject[] PlaceableObjects;
    public PlaceOnPlane POPScript;
   
    public string prefabFolder = "Assets/prefab";
    private string targetPrefabName = "MyPrefab";
    private GameObject loadedPrefab;
    private GameObject lastspawned;
  
    // Start is called before the first frame update
    void Start()
    {
        //ImageUI.GetComponent<SpriteRenderer>().sprite = Card.Image; //Get sprite UI and take the SO sprite and slap it into the UI.
        TitleTextUI.text = Card.TitleText; //Get the UI text change it to the Card's text from the SO
        BodyTextUI.text = Card.BodyText; // Get body text, change it to the SO.
        //StepCardUI.text = "Step Number: " + Card.StepNum.ToString(); //Get the UI text, get the step number's int, change it to string, then display it
        
    }

    void OnEnable()
    {
        Debug.Log("LoadingNewModel");
        Debug.Log(POPScript.AnchorPosForModels);
        if (POPScript.AnchorPosForModels != null)
        {
            Card.ObjToPlace.transform.localPosition = POPScript.AnchorPosForModels;
            Vector3 targetPostition = new Vector3(Card.ObjToPlace.transform.position.x, this.transform.position.y, Card.ObjToPlace.transform.position.z);
            this.transform.LookAt(targetPostition);
            lastspawned =  Instantiate(Card.ObjToPlace, Card.ObjToPlace.transform.localPosition, Quaternion.identity);
        }
        else
        {

        }
    }

    void OnDisable()
    {
        Debug.Log("Disabling Model");
        Destroy(lastspawned);
    }

  
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Get all the objects in the scene with a specific tag, add them to Holder w/ their variables.
        {
            Card.Holders = new List<GameObjectCustomInfo>(); //Everytime you call this function, clear the list so we get the most recent update of all the GameObjects locations. 
            PlaceableObjects = GameObject.FindGameObjectsWithTag("PlaceableObject");
            foreach(GameObject go in PlaceableObjects)
            {
                // Get the original name of the GameObject
                string originalName = go.name;

                // Use regular expression to match a number at the end of the name
                Match match = Regex.Match(originalName, @"\s*\(\d+\)$");

                if (go.name.EndsWith("(Clone)")) 
                {
                    string newName = go.name.Substring(0, go.name.Length - "(Clone)".Length);
                    go.name = newName;

                }

                if (match.Success)
                {
                    // Remove the matched number from the end of the name
                    string newName = originalName.Substring(0, match.Index).Trim();
                   
                        // Assign the new name to the GameObject
                        go.name = newName;

                    string[] prefabPaths = Directory.GetFiles(prefabFolder, "*.prefab", SearchOption.AllDirectories);

                    foreach (string prefabPath in prefabPaths)
                    {
                        string prefabName = Path.GetFileNameWithoutExtension(prefabPath);

                        if (prefabName == newName)
                        {
                            Debug.Log("Prefab found: " + prefabPath);
                            //loadedPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

                            if (loadedPrefab != null)
                            {
                                Card.ObjToPlace = loadedPrefab;
                            }
                            else
                            {
                                Debug.LogError("Failed to load prefab: " + prefabPath);
                            }

                            break;
                        }
                    }
                }
                else //This is needed incase the GO in the scene does not have a (clone) at the end or a (int) at the end. Basically if it has its normal name.
                {
                    string[] prefabPaths = Directory.GetFiles(prefabFolder, "*.prefab", SearchOption.AllDirectories);

                    foreach (string prefabPath in prefabPaths)
                    {
                        string prefabName = Path.GetFileNameWithoutExtension(prefabPath);

                        if (prefabName == go.gameObject.name)
                        {
                            Debug.Log("Prefab found: " + prefabPath);
                           // loadedPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

                            if (loadedPrefab != null)
                            {
                                Card.ObjToPlace = loadedPrefab;
                            }
                            else
                            {
                                Debug.LogError("Failed to load prefab: " + prefabPath);
                            }

                            break;
                        }
                    }
                }

                AddSO(loadedPrefab, go.transform.position, go.transform.rotation.eulerAngles, go.transform.localScale); //Get the GO, Pos, Trans, and Rot from each gameobject in the scene
            }
        }

        if (Input.GetKeyDown(KeyCode.L)) //Destory every gameobject in the scene

        {
            GameObject[] ObjInScene = GameObject.FindGameObjectsWithTag("PlaceableObject");
            foreach(GameObject obj in ObjInScene)
            {
                //Save the updated info before destroying the gameobjects into the SO, then destroy.
                Destroy(obj);
                
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) //Load prefabs from the SO Holders section
        {
            foreach(GameObjectCustomInfo goCI in Card.Holders)
            {
                GameObject CI = Instantiate(goCI.ObjectToPlace, goCI.ObjectToPlace.transform.position, Quaternion.identity);
                CI.transform.position = goCI.ObjTransform; //Local Position
                CI.transform.localScale = goCI.ObjScale;
                CI.transform.localEulerAngles = new Vector3(goCI.ObjRotation.x, goCI.ObjRotation.y, goCI.ObjRotation.z);
            }
        }
    }

    public void AddSO(GameObject GO, Vector3 GOTransform, Vector3 GORotation, Vector3 GOScale) //Each time a new node is made, call this.
    {
        GameObjectCustomInfo bucket = new GameObjectCustomInfo();
        //bucket.BarcodeType = WebCamScript.BarcodesType;
        //bucket.BarcodeValue = BarCodeInput.GetComponent<TMP_InputField>().text; //Text Field
        bucket.ObjectToPlace = GO; //Get Barcode number from webcam.
        bucket.ObjTransform = GOTransform;
        bucket.ObjRotation = GORotation;
        bucket.ObjScale = GOScale;
        Card.Holders.Add(bucket);
    }
}
//Anchor points, everything spawns in as a child of that Anchor point.
//SO might also need a list of scriptable objects. List of "connections" that the SO can interact with.
//step that we came from, and the step that we are going to. 
//Mobile scanning, plane adding