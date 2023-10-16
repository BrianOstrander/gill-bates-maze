using System;
using UnityEngine;

namespace GillBates.View
{
    public class MouseBehaviour : MonoBehaviour
    {
        [SerializeField]
        float moveDuration;

        Vector3 moveBegin;
        Vector3 moveEnd;
        float? moveElapsed;

        public bool IsMoving => moveElapsed.HasValue;
        
        /// <summary>
        /// Move the mouse to the provided world position. 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isInstant"></param>
        public void Move(
            Vector3 position,
            bool isInstant = false
        )
        {
            if (isInstant)
            {
                transform.position = position;
                moveElapsed = null;
                return;
            }

            moveBegin = transform.position;
            moveEnd = position;
            moveElapsed = 0f;
        }

        void Update()
        {
            if (!moveElapsed.HasValue)
            {
                return;
            }

            moveElapsed = Mathf.Min(
                moveDuration,
                moveElapsed.Value + Time.deltaTime
            );

            transform.position = Vector3.Lerp(
                moveBegin,
                moveEnd,
                moveElapsed.Value / moveDuration
            );

            if (Mathf.Approximately(moveElapsed.Value, moveDuration))
            {
                moveElapsed = null;
            }
        }
        
    }
}