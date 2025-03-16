using UnityEngine;

namespace Character
{
    public class CheckGround : MonoBehaviour
    {
        public bool GroundChecked()
        {
            return Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ground"));
        }
        public void PlayAnimFalling()
        {
            PlayerController.Instance.AnimationPlayer.PlayAnimIsFall();
        }
    }
}