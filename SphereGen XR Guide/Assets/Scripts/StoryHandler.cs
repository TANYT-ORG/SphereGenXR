using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SphereGen.GuideXR
{
    public class StoryHandler : MonoBehaviour,IObjectInteracted
    {
        [HideInInspector]
        public AssetReference ThisStoryBoardReference;
        [HideInInspector]
        public CustomManipulation CM;
        

        public void OnNextButtonClick()
        {
            Debug.Log("Nextbutton Click");
            if (ThisStoryBoardReference != null)
            {
                int index = IndividualAssetHandler._instance.GetAssetRefIndex(ThisStoryBoardReference);
                index++;
                if (index < IndividualAssetHandler._instance.storyBoardScriptableObject.AssetRefInOrder.Count)
                {
                    IndividualAssetHandler._instance.previousStackRef.Push(ThisStoryBoardReference);
                    IndividualAssetHandler._instance.LoadAddressableAsset(index);
                }
                else
                {
                    Debug.Log("No More StoryBoard");
                }
            }
            else
                Debug.LogError("No Current Asset Ref");


        }

        public void OnPreviousButtonClick()
        {
            // IndividualAssetHandler._instance.LoadPreviousStoryboard();
            if (IndividualAssetHandler._instance.previousStackRef.Count > 0)
            {
                int index = IndividualAssetHandler._instance.GetAssetRefIndex(IndividualAssetHandler._instance.previousStackRef.Pop());
                IndividualAssetHandler._instance.LoadAddressableAsset(index);
            }
            else
            {
                Debug.Log("At First StoryBoard");
            }
        }

        public void OnCustomButtonClick(int NextAssetRefIndex)
        {
           
            int index = IndividualAssetHandler._instance.GetAssetRefIndex(ThisStoryBoardReference);
            int index1 = IndividualAssetHandler._instance.GetAssetRefIndex(IndividualAssetHandler._instance.previousStackRef.Peek());
            if (IndividualAssetHandler._instance.previousStackRef.Peek() != ThisStoryBoardReference)
            {
                IndividualAssetHandler._instance.previousStackRef.Push(ThisStoryBoardReference);
                Debug.Log("Added to Stack");
            }
            else
            {
                Debug.Log("Same StoryBoard Is Reloaded");
            }
                IndividualAssetHandler._instance.LoadAddressableAsset(NextAssetRefIndex);
            

        }

        public void ObjectInteracted()
        {
           
        }

       
    }

}
