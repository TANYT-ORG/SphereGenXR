using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchoringController : MonoBehaviour
{
    public string anchorIdentifier = "AnchorMarker";
    // Start is called before the first frame update
    void Start()
    {
        PlaceAtAnchor();
    }

    public void PlaceAtAnchor()
    {
        transform.parent = GameObject.Find("PlaceableObject").gameObject.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void DetachFromAnchor()
    {
        transform.parent = null;
    }
}
