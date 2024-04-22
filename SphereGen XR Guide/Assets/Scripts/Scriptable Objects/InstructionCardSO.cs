using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Instruction Card", menuName = "ScriptableObjects/UI/Instruction Card")]
public class InstructionCardSO : ScriptableObject
{
    [TextArea] public string instructionText;
    public Sprite backgroundSprite;
    public Sprite headerBackgroundSprite;
    public Sprite headerBorderSprite;
    public string headerText;
    public List<InstructionCardButtonData> buttonDataList = new List<InstructionCardButtonData>();
}

[System.Serializable]
public class InstructionCardButtonData
{
    public Sprite buttonImage;
    public string buttonText;
    public UnityEvent onClickEvent;
}
