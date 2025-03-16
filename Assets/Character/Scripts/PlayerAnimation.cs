using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator AnimatorPlayer => animator;
        private bool isJump;
        public void PlayAnimRun(float speed)
        {
            if (isJump)
            {
                return;
            }
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }

        public void PlayAnimJump(bool jumpping)
        {
            isJump = true;
            animator.SetTrigger("Jumpping");
            StartCoroutine(WaitForAnimationStart());
            
        }

        private bool isFall;
        public void PlayAnimIsFall()
        {
            if (!isFall)
            {
                isFall = true;
                animator.SetTrigger("IsFall");
                DOVirtual.DelayedCall(1.2f, delegate
                {
                    isFall = false;
                });
            }
        }
        public void SetSpeedCurrentAnimation(float speed)
        {
            animator.speed = speed;
        }
        private IEnumerator WaitForAnimationStart()
        { 
            yield return new WaitForSeconds(0.9f);
            PlayerController.Instance.CanJump = true;
            isJump = false;
        }

    }
}