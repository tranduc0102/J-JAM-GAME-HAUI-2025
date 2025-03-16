using System;
using _Scripts.Extension;
using Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class UISetting : MonoBehaviour
    {
        [SerializeField] private UIAppear dime;
        [SerializeField] private UIAppear popup;

        public bool IsMuteMusic
        {
            get => PlayerPrefs.GetInt("IsMuteMusic", 0) == 1;
            set
            {
                if (value == false)
                {
                    PlayerPrefs.SetInt("IsMuteMusic", 0);
                }
                else
                {
                    PlayerPrefs.SetInt("IsMuteMusic", 1);
                }
            }
        }

        public bool IsMuteSound
        {
            get => PlayerPrefs.GetInt("IsMuteSound", 0) == 1;
            set
            {
                if (value == false)
                {
                    PlayerPrefs.SetInt("IsMuteSound", 0);
                }
                else
                {
                    PlayerPrefs.SetInt("IsMuteSound", 1);
                }
            }
        }

        public bool IsOffVibration
        {
            get => PlayerPrefs.GetInt("IsOffVibration", 0) == 1;
            set
            {
                if (value == false)
                {
                    PlayerPrefs.SetInt("IsOffVibration", 0);
                }
                else
                {
                    PlayerPrefs.SetInt("IsOffVibration", 1);
                }
            }
        }

        [Header("Toggle")] [SerializeField] private Toggle toggleSound;
        [SerializeField] private Toggle toggleMusic;
        [SerializeField] private Toggle toggleVibrate;

        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 500);
        }
        public void DisplaySetting(bool enable, Action onClosed = null)
        {
            if (enable)
            {
                dime.gameObject.SetActive(true);
                popup.gameObject.SetActive(true);
                /*if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.gameObject.SetActive(false);
                }*/
            }
            else
            {
                popup.Close(delegate { onClosed?.Invoke(); }, true);
                dime._Close(true);
                /*DOVirtual.DelayedCall(0.3f, delegate
                {
                    if (PlayerController.Instance != null)
                    {
                        PlayerController.Instance.gameObject.SetActive(true);
                    }
                });*/
            }
        }

        public void ButtonMusicClick()
        {
            AudioManager.Instance.PlaySoundButtonClick();
            IsMuteMusic = !IsMuteMusic;
            AudioManager.Instance.SetMuteMusic();
        }

        public void ButtonSoundClick()
        {
            AudioManager.Instance.PlaySoundButtonClick();
            IsMuteSound = !IsMuteSound;
            AudioManager.Instance.SetMuteSounds();
        }

        public void ButtonVibrationClick()
        {
            if (IsOffVibration == toggleVibrate.isOn)
            {
                IsOffVibration = !IsOffVibration;
            }
        }

        public void OnButtonSettingClick()
        {
            AudioManager.Instance.PlaySoundButtonClick();
            DisplaySetting(true);
        }

        public void OnButtonCloseClick()
        {
            AudioManager.Instance.PlaySoundButtonClick();
            DisplaySetting(false);
        }

        public void InActive()
        {
            DisplaySetting(false);
        }
    }
}