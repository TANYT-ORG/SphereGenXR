using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SphereGen
{
    [CreateAssetMenu(fileName = "StoryBoardRefDataInOrder", menuName = "ScriptableObjects/StoryBoardScriptablesObjects", order = 1)]
    public class StoryBoardScriptablesObjects : ScriptableObject
    {
        [SerializeField]
        public List<AssetReference> AssetRefInOrder = new List<AssetReference>();

    }
}
