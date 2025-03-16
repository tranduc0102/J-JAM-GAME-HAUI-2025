using System;
using _Scripts.Extension;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class SoundAndEffect
    {
        public ParticleSystem effect;
        public AudioClip audioClip;
    }
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Souce")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;

        [Header("Audio Clip")]
        [SerializeField] private AudioClip buttonClick;
        [SerializeField] private AudioClip toggleClickOn;
        [SerializeField] private AudioClip toggleClickOff;

        [Space] [Header("Audio in Game")]
        [SerializeField] private AudioClip musicBG;
        [SerializeField] private AudioClip musicInGame;
        [SerializeField] private AudioClip soundConnect;
        [SerializeField] private AudioClip soundWin;
        [SerializeField] private AudioClip soundFail;

        [Space] [Header("Audio in GamePlay")] 
        [SerializeField] private AudioClip slimeFall;
        [SerializeField] private AudioClip slimeJump;
        [SerializeField] private AudioClip slimeMoving;
        [SerializeField] private AudioClip slimeGetItem;
        [SerializeField] private AudioClip slimeUseSkill;
        
        
        protected override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }
        
        void Start()
        {
            SetMuteSounds();
            SetMuteMusic();
        }
        
        public void SetMuteSounds()
        {
            if (UI.UIController.Instance.UISetting.IsMuteSound)
            {
                soundSource.mute = true;
                return;
            }
            soundSource.mute = false;
        }
        public void SetMuteMusic()
        {
            if (UI.UIController.Instance.UISetting.IsMuteMusic)
            {
                musicSource.mute = true;
                return;
            }
            musicSource.mute = false;
        }

        public void SetVolume(float volume)
        {
            musicSource.volume = volume;
            soundSource.volume = volume;
        }

        public void PlaySoundButtonClick()
        {
            PlaySfx(buttonClick);
        }

        public void PlaySoundToggleClickOn()
        {
            PlaySfx(toggleClickOn);
        }

        public void PlaySoundToggleClickOff()
        {
            PlaySfx(toggleClickOff);
        }
        public void PlaySoundWin()
        {
            PlaySfx(soundWin);
        }
        public void PlaySoundlevelFail()
        {
            PlaySfx(soundFail);
        }
        public void PlayInGameMusic()
        {
            if (!musicSource.isPlaying)
            {
                if (musicInGame != null)
                {
                    musicSource.clip = musicInGame;
                    musicSource.DOFade(1f, 0.5f).OnPlay(() =>
                    {
                        musicSource.Play();
                    }).SetUpdate(true);
                }
            }
        }
        
        public void PlayMusicBG()
        {
            if (!musicSource.isPlaying)
            {
                if (musicInGame != null)
                {
                    musicSource.clip = musicBG;
                    musicSource.DOFade(1f, 0.5f).OnPlay(() =>
                    {
                        musicSource.Play();
                    }).SetUpdate(true);
                }
            }
        }

        public void StopMusic()
        {
            if (musicSource.isPlaying)
            {
                musicSource.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    musicSource.Stop();
                }).SetUpdate(true);
            }
        }

        public void StopSound()
        {
            if (soundSource.isPlaying)
            {
                if (soundSource.loop == true)
                {
                    soundSource.loop = false;
                }
                soundSource.Stop();
            }
        }

        public void PlaySfx(AudioClip audioClip, bool repeat = false)
        {
            if (UI.UIController.Instance.UISetting.IsMuteSound) return;

            if (audioClip != null)
            {
                if (repeat)
                {
                    soundSource.loop = true;
                    soundSource.clip = audioClip;
                    soundSource.Play();
                }
                else
                {
                    soundSource.loop = false;
                    soundSource.PlayOneShot(audioClip);
                }

            }
        }

        public void playSoundFall()
        {
            PlaySfx(slimeFall);
        }public void playSoundJump()
        {
            PlaySfx(slimeJump);
        }public void playSoundMoving()
        {
            PlaySfx(slimeMoving);
        }

        public void playSoundGetItem()
        {
            PlaySfx(slimeGetItem);
        }

        public void playSoundUseSkill()
        {
            PlaySfx(slimeUseSkill);
        }
    }
}