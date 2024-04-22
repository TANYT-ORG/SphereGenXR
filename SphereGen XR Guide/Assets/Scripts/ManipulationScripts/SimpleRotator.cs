using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public bool active;
    public enum Axis
    {
        Up, Right, Forward
    }
    public float speed;
    [SerializeField]
    public Axis RotationAxis;
    

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            DoRotation();
        }
    }

    public void DoRotation()
    {
        switch(RotationAxis)
        {
            case Axis.Up:
                transform.Rotate(transform.up, speed);
                break;
            case Axis.Right:
                transform.Rotate(transform.right, speed);
                break;
            case Axis.Forward:
                transform.Rotate(transform.forward, speed);
                break;
        }
    }    
}
