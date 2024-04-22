using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Step Card")]
public class StepCardSO : ScriptableObject

{
    public string TitleText;
    [TextArea(10, 10)]
    public string BodyText;
    //public int StepNum;
    public GameObject ObjToPlace;
    public Sprite Image;
    public List<GameObjectCustomInfo> Holders = new List<GameObjectCustomInfo>();
}

