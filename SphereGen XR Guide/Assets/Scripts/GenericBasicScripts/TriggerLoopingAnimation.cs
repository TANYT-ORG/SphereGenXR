using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoopingAnimation : MonoBehaviour, IAnimationSGXR
{
    public Animator animator;
    public string clipName;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(clipName != null)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
            {
                animator.Play(clipName, 0, 0f);
            }
        }
    }

    //returns true if animclip exists false if not
    public bool TriggerAnimationLoop(string nameOfAnim)
    {
        AnimationClip clipToPlay = FindClip(nameOfAnim, animator.runtimeAnimatorController);
        if(clipToPlay != null)
        {
            clipName = nameOfAnim;
            animator.Play(clipName, 0, 0f); //set start time to 0
            return true;
        }
        else
        {
            return false;
        }
        
    }

    AnimationClip FindClip(string name, RuntimeAnimatorController controller)
    {
        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
