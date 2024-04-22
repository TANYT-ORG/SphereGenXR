using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationSGXR
{
    public bool TriggerAnimationLoop(string nameOfAnim);

    public Animator GetAnimator();
}
