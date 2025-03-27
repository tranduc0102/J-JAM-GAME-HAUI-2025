using pooling;
using UnityEngine;

namespace Character
{
    public class SpecialSkill : MonoBehaviour
    {
        [Space] 
        [Header("Skill")] 
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
            isSettingPoint = true;
            objCheckpoint =
                PoolingManager.Spawn(boneReference, player.position + Vector3.up * 0.4f, Quaternion.identity, PlayerController.Instance.transform.parent);
            canBackCheckPoint = true;
        }

        private void BackToCheckPoint()
        {
            if (!isSettingPoint) return;
            player.position = new Vector3(objCheckpoint.transform.position.x, objCheckpoint.transform.position.y + 0.5f, objCheckpoint.transform.position.z);
            isSettingPoint = false;
            PoolingManager.Despawn(objCheckpoint.gameObject);
            canBackCheckPoint = false;
        }
    }
}