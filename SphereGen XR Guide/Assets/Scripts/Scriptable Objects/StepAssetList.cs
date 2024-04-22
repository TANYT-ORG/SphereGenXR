using UnityEngine;

[CreateAssetMenu(fileName = "StepAssetList", menuName = "ScriptableObjects/StepAssetList")]
public class StepAssetList : ScriptableObject
{
    [System.Serializable]
    public class GameObjectInfo
    {
        public GameObject gameObject;
        public bool instantiateWithAnchor;
    }

    public GameObjectInfo[] gameObjects;
}
