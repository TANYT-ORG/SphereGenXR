using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    public class TriggerLinearStageAnimations : MonoBehaviour
    {
        public bool explode;
        public Animator stageAnimation;
        AnimationClip animClip;

        private void OnEnable()
        {
            AnimPlay();
        }
        void Start()
        {
            animClip = stageAnimation.runtimeAnimatorController.animationClips[1];
            Debug.Log("anim name: " + animClip.name);
            animClip.AddEvent(new AnimationEvent()
            {
                time = animClip.length,
                functionName = "OnCompletedAnimation"
            });

            animClip = stageAnimation.runtimeAnimatorController.animationClips[0];
            animClip.AddEvent(new AnimationEvent()
            {
                time = animClip.length,
                functionName = "OnCompletedAnimation"
            });
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                AnimPlay();
            }
            //    if(!explode)
            //    {
            //        //stageAnimation.enabled = false;

            //        stageAnimation.SetBool("Explode", false);
            //    }

            //    if(explode)
            //    {
            //       // stageAnimation.enabled = true;
            //        stageAnimation.SetBool("Explode", true);
            //    }
        }
        public void OnCompletedAnimation()
        {
            //Debug.Log("Animation Completed");
            stageAnimation.enabled = false;
        }

        public void AnimPlay()
        {
            foreach (var item in ObjectDetector.SelectableObjects)
            {
                item.GetComponent<SelectedObject>().ResetHostTransform();

            }
            stageAnimation.enabled = true;
            ObjectDetector.DeselectAll();
            if(explode)
            {
                stageAnimation.Play("ExplodedView", -1, 0.1f);
            }
            else
            {
                stageAnimation.Play("AssembledView", -1, 0.1f);
            }
            explode = !explode;
            //stageAnimation.SetBool("Explode", true);
        }

    }
}
