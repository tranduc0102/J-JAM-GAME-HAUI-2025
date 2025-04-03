using System;
using DG.Tweening;
using pooling;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Requiement")] 
        [SerializeField]
        private PlayerMovement movement;
        public PlayerMovement PlayerMovement => movement;

        [SerializeField] 
        private PlayerAnimation animationPlayer;
        public PlayerAnimation AnimationPlayer => animationPlayer;

        [SerializeField] 
        private CheckGround groundChecked;
        public CheckGround GroundChecked => groundChecked;
        
        [SerializeField] 
        private SpecialSkill specialSkill;
        public SpecialSkill Skill => specialSkill;

        [SerializeField] 
        private ChangeColorPlayer changeColorPlayer;
        
        [Header("Setting")]
        [Header("Setting Movement")]
        private float horizontal;
        [SerializeField] private float speedMove = 5f;
        [SerializeField] private float jumpForce = 8f;
        private bool canMove = true;
        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        private bool canJump = true;
        public bool CanJump
        {
            get => canJump;
            set => canJump = value;
        }

        [Header("Setting Collider 2D")]
        [SerializeField]
        private Collider2D collider2D;
        public Collider2D Collider2DPlayer => collider2D;

        [Header("Setting Layer")] 
        [SerializeField]
        private LayerMask layerMap;

        public static PlayerController Instance;
        private  void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            if(GameController.Instance.State != StateGame.Playing) return;
            if (isCheckMovePlay && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.W)))
            {
                isCheckMovePlay = false;
                ToggleMovementState(true);
            }
            if (!canMove) return;
            horizontal = Input.GetAxisRaw("Horizontal");
            animationPlayer.PlayAnimRun(horizontal);
            movement.Move(horizontal, speedMove);
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && groundChecked.GroundChecked() && canJump)
            {
                animationPlayer.PlayAnimJump(true);
                movement.Jump(jumpForce);
                canJump = false;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Skill.SkillReturnSavePoint();
            }
        }
        private void FixedUpdate()
        {
            VerifyStableLanding();
            if (!canMove) return;
            SetParentPlayer();
        }
        private void SetParentPlayer()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0f, layerMap);
            if (hit.collider != null)
            {
                this.transform.SetParent(hit.transform);
                changeColorPlayer.ChangeColor(hit.transform.GetChild(0).name);
            }
        }

        private bool isCheckMovePlay = false;

        private void VerifyStableLanding()
        {
            /*RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, Vector2.down, 0.1f);
            if (hit2D.Length >= 3)
            {
                ToggleMovementState(false);
                isCheckMovePlay = true;
            }*/
        }

        private bool isFalling;
        
        public void ToggleMovementState(bool enable)
        {
            if (isCheckMovePlay) return;
            movement.StopMove(enable);
            canMove = enable;
            collider2D.enabled = enable;
            animationPlayer.SetSpeedCurrentAnimation(enable? 1f:0f);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground") && specialSkill.CanBackCheckPoint &&
                canMove)
            {
                specialSkill.CanBackCheckPoint = false;
            }

            if (other.CompareTag("Finish"))
            {
                if (GameController.Instance.State != StateGame.Win)
                {
                    GameController.Instance.Win();
                    transform.DORotate(new Vector3(0, 360f, 0), 0.8f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutQuart);
                    transform.DOScale(0f, 0.8f)
                        .SetEase(Ease.InOutSine) 
                        .OnComplete(()=>gameObject.SetActive(false));
                }
            }

            if (other.CompareTag("Losse"))
            {
                DOVirtual.DelayedCall(1f, delegate
                {
                    GameController.Instance.Replay();
                });
            }

            if (other.CompareTag("Respawn"))
            {
                other.gameObject.SetActive(false);
                GameController.Instance.CheckCurrentStar();
            }
        }
    }
}
