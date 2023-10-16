using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GillBates.View
{
    public class MouseBehaviour : MonoBehaviour
    {
        [SerializeField]
        float moveDuration;

        [SerializeField]
        AudioClip[] idleSounds = Array.Empty<AudioClip>();

        [SerializeField]
        AudioClip chewSound;
        
        [SerializeField]
        AudioSource idleSoundSource;

        [SerializeField]
        AudioSource chewSoundSource;
        
        Vector3 moveBegin;
        Vector3 moveEnd;
        float? moveElapsed;

        public bool IsMoving => moveElapsed.HasValue;

        int nextIdleSound;
        
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

            if (!idleSoundSource.isPlaying && 0 < idleSounds.Length)
            {
                nextIdleSound = (nextIdleSound + 1) % idleSounds.Length;
                idleSoundSource.clip = idleSounds[nextIdleSound];
                idleSoundSource.Play();
            }
        }

        public void BeginChewing()
        {
            if (chewSound == null)
            {
                return;
            }

            chewSoundSource.clip = chewSound;
            chewSoundSource.loop = true;
            chewSoundSource.Play();
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