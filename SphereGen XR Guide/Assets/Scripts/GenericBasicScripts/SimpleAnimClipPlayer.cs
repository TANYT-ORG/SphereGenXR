using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleAnimClipPlayer : MonoBehaviour
{
    public string TargetGameObjectName, NameOfClipToPlay;
    bool _setup;
    TriggerLoopingAnimation _tla;
    Animator _anim;

    // Start is called before the first frame update
    void Awake()
    {

    }

    public void LateUpdate()
    {
        if (!_setup)
        {
            Animator[] animList = FindObjectsOfType<Animator>(true);
            foreach (Animator a in animList)
            {
                if (a.gameObject.name == TargetGameObjectName + "(Clone)" || a.gameObject.name == TargetGameObjectName)
                {
                    _tla = a.GetComponent<TriggerLoopingAnimation>();
                    _anim = _tla.GetAnimator();
                    DoAnimation();
                    _setup = true;
                }
                
            }
            if (!_tla)
            {
                Debug.LogError("Could not find proper script to trigger anim clip...");
            }
        }
    }

    private void OnEnable()
    {
        DoAnimation();
    }

    void DoAnimation()
    {
        if (_tla != null)
        {
            bool success = _tla.TriggerAnimationLoop(NameOfClipToPlay);
            if (!success)
            {
                Debug.LogErrorFormat("Was not able to play {0} on animation controller {1}", NameOfClipToPlay, _tla.GetAnimator().name);
            }
        }
    }

    private void OnDisable()
    {
        // Get the length of the animation clip
        float clipLength = _anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == NameOfClipToPlay).length;

        // Specify the starting time in seconds (from the end of the clip)
        float startTime = clipLength;

        // Play the animation clip from the starting time
        _anim.Play(NameOfClipToPlay, 0, startTime);
        _tla.clipName = null;
    }
}
