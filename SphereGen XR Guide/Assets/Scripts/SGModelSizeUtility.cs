using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{

    public class SGModelSizeUtility : MonoBehaviour
    {
        public struct TransformValues
        {
            public Vector3 Position { get; private set; }
            public Quaternion Rotation { get; private set; }
            public Vector3 ScaleLocal { get; private set; }
            public Vector3 ScaleLossy { get; private set; }
            public Vector3 ScaleWorld { get; private set; }
            public Transform Parent { get; private set; }

            public TransformValues(Transform transform)
            {
                Position = transform.position;
                Rotation = transform.rotation;
                ScaleLossy = transform.lossyScale;
                ScaleLocal = transform.localScale;
                Parent = transform.parent;

                transform.SetParent(null);
                ScaleWorld = transform.localScale;
                transform.SetParent(Parent);

            }

            public void ApplyAllValues(Transform transform)
            {
                ApplyPosition(transform);
                ApplyRotation(transform);

                transform.SetParent(Parent);
                //apply scale after setting the parent.
                ApplyLocalScale(transform);
            }

            public void ApplyPosition(Transform transform)
            {
                transform.position = Position;
            }

            public void ApplyRotation(Transform transform)
            {
                transform.rotation = Rotation;
            }

            public void ApplyLocalScale(Transform transform)
            {
                transform.localScale = ScaleLocal;
            }

            public void ApplyWorldScale(Transform transform)
            {
                transform.SetParent(null);
                transform.localScale = ScaleWorld;
                transform.SetParent(Parent);
            }

            public void ApplyParent(Transform transform)
            {
                transform.SetParent(Parent);
            }
        }
    }
}
