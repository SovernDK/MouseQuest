using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool lookAtCamera;

    private void Update() 
    {
        if (lookAtCamera)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 direction = cameraPosition - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                transform.rotation = targetRotation;
            }
        }
    }
}
