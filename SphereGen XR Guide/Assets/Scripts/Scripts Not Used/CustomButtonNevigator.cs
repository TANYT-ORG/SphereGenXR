using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButtonNevigator : MonoBehaviour
{
  
  //  [SerializeField] // Remove Serialze later
    //private AssetReference CurrentAssetReference;

    //[SerializeField]
    //private int NextAssetRefIndex;

    
    //public void SetAssetHandler(IndividualAssetHandler handler,AssetReference currentAssetReference)
    //{
        
    //    CurrentAssetReference = currentAssetReference;
    //}

    //private void Start()
    //{
       
    //    gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    //}
   
    //private void OnButtonClick()
    //{
    //  //  IndividualAssetHandler._instance.PreviousAssetReference = CurrentAssetReference;
    //    IndividualAssetHandler._instance.previousStackRef.Push(CurrentAssetReference);
    //    IndividualAssetHandler._instance.LoadAddressableAsset(NextAssetRefIndex);
    //}
}
