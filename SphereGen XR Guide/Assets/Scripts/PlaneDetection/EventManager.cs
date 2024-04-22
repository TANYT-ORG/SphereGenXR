using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnAnchorPlaced();
    public static event OnAnchorPlaced onAnchorPlaced;
    public static void RaiseOnAnchorPlaced() => onAnchorPlaced?.Invoke();
}
