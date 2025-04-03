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
        [Header("Slider Sound FX")]
        [SerializeField] private Slider _sliderSoundFX;
        [SerializeField] private Image iconSound;
        public Sprite iconOnSoundFX;
        public Sprite iconOffSoundFX;
        [Header("Slider Music")]
        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Image iconMusic;
        public Sprite iconOnMusic;
        public Sprite iconOffMusic;
        private Action _actionClosed;
        
        [SerializeField] private Button replay;


        public float VolumeSound
        {
            get => PlayerPrefs.GetFloat("VolumeSound", 1);
            set => PlayerPrefs.SetFloat("VolumeSound", value);
        }
        public float VolumeMusic
        {
            get => PlayerPrefs.GetFloat("VolumeMusic", 1);
            set => PlayerPrefs.SetFloat("VolumeMusic", value);
        }
        
        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 500);
        }

        private void OnEnable()
        {
            _sliderMusic.value = VolumeMusic;
            _sliderSoundFX.value = VolumeSound;
        }

        private void Start()
        {
            UpdateSoundIcon(VolumeSound);
            UpdateMusicIcon(VolumeMusic);

            _sliderSoundFX.onValueChanged.AddListener(UpdateSoundVolume);
            _sliderMusic.onValueChanged.AddListener(UpdateMusicVolume);
        }

        private void UpdateSoundIcon(float volume)
        {
            iconSound.sprite = volume == 0f ? iconOffSoundFX : iconOnSoundFX;
        }

        private void UpdateMusicIcon(float volume)
        {
            iconMusic.sprite = volume == 0f ? iconOffMusic : iconOnMusic;
        }

        private void UpdateSoundVolume(float value)
        {
            VolumeSound = value;
            UpdateSoundIcon(value);
    
            bool isMuted = value == 0f;
            IsMuteSound = isMuted;
    
            if (isMuted)
            {
                AudioManager.Instance.SetVolumeSoundSource(0f);
            }
            else
            {
                AudioManager.Instance.SetMuteSounds();
                AudioManager.Instance.SetVolumeSoundSource(value);
            }
        }

        private void UpdateMusicVolume(float value)
        {
            VolumeMusic = value;
            UpdateMusicIcon(value);
    
            bool isMuted = value == 0f;
            IsMuteMusic = isMuted;
    
            if (isMuted)
            {
                AudioManager.Instance.SetVolumeMusicSource(0f);
            }
            else
            {
                AudioManager.Instance.SetMuteMusic();
                AudioManager.Instance.SetVolumeMusicSource(value);
            }
        }
        public void DisplaySetting(bool enable, Action onClosed = null)
        {
            if(GameController.Instance.State == StateGame.ShowTutorial)return;
            if (enable)
            {
                dime.gameObject.SetActive(true);
                popup.gameObject.SetActive(true);
                if (GameController.Instance.State == StateGame.Playing)
                {
                    replay.gameObject.SetActive(true);
                }
                else
                {
                    replay.gameObject.SetActive(false);
                } 
            }
            else
            {
                popup.Close(delegate { onClosed?.Invoke(); }, true);
                dime._Close(true);
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
            
        }

        public void OnButtonSettingClick()
        {
            AudioManager.Instance.PlaySoundButtonClick();
            DisplaySetting(true);
            if (GameController.Instance.State == StateGame.Playing)
            {
            }
            else
            {
            } 
                
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