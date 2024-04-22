using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AssetHandler : MonoBehaviour
{

    [SerializeField]
    private List< AssetReference> AssetReferencePrefab;

    [SerializeField]
    private GameObject[] AllAsset;

    [SerializeField]
    private int previousAssetIndex = 0;

    [SerializeField]
    private int currentAssetIndex = 0;

    private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _AsyncOperationHandleDic=new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
    private readonly Dictionary<AssetReference, GameObject> _AsyncAssetHandleDic = new Dictionary<AssetReference,GameObject>();
    [SerializeField]
    private Button previousBtn, NextBtn;
    
    

    void Start()
    {
        AllAsset = new GameObject[AssetReferencePrefab.Count];
        //  Addressables.InitializeAsync().Completed += AddressableLoading_Complete;
        currentAssetIndex = 0;
        LoadAddressableAsset(currentAssetIndex);
        Debug.Log("ss: " + UnityEngine.AddressableAssets.Addressables.BuildPath);
        Debug.Log("ss: " + UnityEngine.AddressableAssets.Addressables.RuntimePath);

    }



    private void AddressableLoading_Complete(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("onj Names: " + obj.Result.LocatorId);
        foreach (var item in AssetReferencePrefab)
        {
            item.InstantiateAsync().Completed += (go) =>
            {
                Debug.Log("gggg: " + go.Result.name);
               // AllAsset.Add(go.Result);
            };
           
        }
        

    }
    public void LoadNext()
    {
        currentAssetIndex++;
        if (currentAssetIndex <= AssetReferencePrefab.Count) 
            LoadAddressableAsset(currentAssetIndex);
    }

    public void LoadPrevious()
    {
        if(previousAssetIndex>0) // Doesn't require to check
        {
            LoadAddressableAsset(previousAssetIndex);
        }
    }

    public void LoadAddressableAsset(int i)
    {
        try
        {
            if (AllAsset[i] == null)
            {
                AssetReference AR = AssetReferencePrefab[i];
                if (_AsyncOperationHandleDic.ContainsKey(AR))
                {

                    Debug.Log("It Is Already Loaded");
                    HideAndShow(i);

                }
                else
                {
                    AsyncOperationHandle<GameObject> op = Addressables.LoadAssetAsync<GameObject>(AR);
                    _AsyncOperationHandleDic[AR] = op;
                    Debug.Log("hhhhhhh");
                    op.Completed += (operation) =>
                    {
                        
                        Debug.Log("Model is Loaded: " + op.Result.name);
                        CheckAssetAvailable(i); // model is loaded
                    };
                }
            }
            else
            {
                Debug.Log("Already Loaded");

                HideAndShow(i);
            }
        }
        catch (Exception e)
        {

            Debug.LogError(" error2  " + e);
        }
    }

    public void HideAndShow(int nextIndex)
    {
        previousAssetIndex = currentAssetIndex;
        currentAssetIndex = nextIndex;
        Debug.Log(previousAssetIndex + "  ggggggggggggg    " + currentAssetIndex);
        AllAsset[previousAssetIndex].gameObject.SetActive(false);
        AllAsset[nextIndex].gameObject.SetActive(true);

        if (currentAssetIndex >= AssetReferencePrefab.Count)
        {
            NextBtn.gameObject.SetActive(false);
           
        }
        else
        {
            
            NextBtn.gameObject.SetActive(true);
        }
        if (currentAssetIndex <= 0)
        {
            previousBtn.gameObject.SetActive(false);
        }
        else
        {
            previousBtn.gameObject.SetActive(true);
        }
    }

    public void CheckAssetAvailable(int i)
    {
        try
        {
            AssetReference AR = AssetReferencePrefab[i];
            AsyncOperationHandle<GameObject> op;
            if (_AsyncOperationHandleDic.TryGetValue(AR, out op))
            {
                if (op.IsDone)
                {
                    GameObject obj = Instantiate(op.Result);
                    AllAsset[i] = obj;
                    HideAndShow( i);


                }
                else
                {
                    Debug.Log("dddd");
                }
            }
        }
        catch (Exception e)
        {

            Debug.LogError(" error1 " + e);
        }
    }
}
