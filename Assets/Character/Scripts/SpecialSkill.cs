using pooling;
using UnityEngine;

namespace Character
{
    public class SpecialSkill : MonoBehaviour
    {
        [Space] 
        [Header("Skill")] 
        private Vector3 checkPointPos;
        private bool isSettingPoint;
        private bool canBackCheckPoint;

        public bool CanBackCheckPoint
        {
            set => canBackCheckPoint = value;
            get => canBackCheckPoint;
        }

        [SerializeField] private Transform player;
        [SerializeField] private Transform boneReference;
        private Transform objCheckpoint;
        public void SkillReturnSavePoint()
        {
            if (isSettingPoint)
            {
                BackToCheckPoint();
                return;
            }
            checkPointPos = transform.position;
            isSettingPoint = true;
            objCheckpoint =
                PoolingManager.Spawn(boneReference, player.position, Quaternion.identity);
            canBackCheckPoint = true;
        }

        private void BackToCheckPoint()
        {
            if (!isSettingPoint) return;
            player.position = new Vector3(checkPointPos.x, checkPointPos.y + 0.5f, checkPointPos.z);
            isSettingPoint = false;
            PoolingManager.Despawn(objCheckpoint.gameObject);
            canBackCheckPoint = false;
        }
    }
}