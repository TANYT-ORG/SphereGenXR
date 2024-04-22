using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionObject : MonoBehaviour
{
    public Transform originalPosition;

   public bool resetFlag = false;
    private void Start()
    {
        originalPosition = transform;
    }

    public void ResetObject()
    {
      //  StartCoroutine(ScaleOverSeconds(originalPosition, 15f));
    }

    private void Update()
    {
        if(resetFlag)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, 2 * Time.deltaTime);
        }
    }

    public IEnumerator ScaleOverSeconds(Transform scaleTo, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;
        Debug.Log("Starting pos:" + startingPosition);
        while (elapsedTime < seconds)
        {
            Debug.Log("hh");
            transform.position = Vector3.Lerp(startingPosition, scaleTo.position, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
       //objectToScale.transform.position = scaleTo.position;
    }



}
