using System;
using _Scripts;
using Character;
using DG.Tweening;
using UnityEngine;

public class GameController : Singleton<GameController>
{
   protected override void Awake()
   {
      base.KeepAlive(false);
      base.Awake();
   }

   private StateGame state = StateGame.WaitingChoiceLevel;

   public StateGame State
   {
      get => state;
      set => state = value;
   }

   [SerializeField] private SpawnLevel spawnLevel;
   private int amoutStar;
   private int level;
   public SpawnLevel SpawnLevel => spawnLevel;



   public bool IsFirstPlay
   {
      get => PlayerPrefs.GetInt("Tut", 1) == 1;
      set => PlayerPrefs.SetInt("Tut", value ? 1 : 0);
   }
   private void Start()
   {
      level = PlayerPrefs.GetInt("Level_" + level, 1);
      if (IsFirstPlay)
      {
         state = StateGame.ShowTutorial;
         return;
      }
      state = StateGame.WaitingChoiceLevel;
   }
   public void PlayGame(int indexLevel)
   {
      AnimationTranslate.Instance.StartLoading(delegate
      {
         level = indexLevel;
         AnimationTranslate.Instance.DisplayLoading(false);
         spawnLevel.SpawmLevel(indexLevel - 1);
         amoutStar = MapLevelManager.Instance.ListBtn[level - 1].AmountStar;
         if (amoutStar > 0)
         {
            FindTeleport();
         }
         state = StateGame.Playing;
         _Scripts.UI.UIController.Instance.UIInGame.ShowDisPlayGame();
      });
      AudioManager.Instance.StopMusic();
      DOVirtual.DelayedCall(2f, delegate
      {
         AudioManager.Instance.PlayInGameMusic();
         if (IsFirstPlay)
         {
            state = StateGame.ShowTutorial;
            _Scripts.UI.UIController.Instance.Message.ShowDisplayDialog();
         }
         if (level == 5)
         {
            _Scripts.UI.UIController.Instance.Message.ShowMessageNoticeSkill();
         }
      });
   }

   public void BackHome()
   {
      AudioManager.Instance.PlaySoundButtonClick();
      state = StateGame.WaitingChoiceLevel;
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            AnimationTranslate.Instance.DisplayLoading(false);
            _Scripts.UI.UIController.Instance.UIInGame.ShowDisplayHome();
            spawnLevel.DestroyMap();
            AudioManager.Instance.StopMusic();
         });
      });
      DOVirtual.DelayedCall(2f, delegate
      {
         AudioManager.Instance.PlayMusicBG();
      });
     
   }
   public void NextLevel()
   {
      AudioManager.Instance.PlaySoundButtonClick();
      state = StateGame.WaitingChoiceLevel;
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            /*UIController.Instance.UIInGame.ShowDisplayHome();*/
            spawnLevel.SpawmLevel(level - 1);
            if (level == 5)
            {
               DOVirtual.DelayedCall(2f, delegate
               {
                  _Scripts.UI.UIController.Instance.Message.ShowMessageNoticeSkill();
               });
            }
            amoutStar = MapLevelManager.Instance.ListBtn[level - 1].AmountStar;
            if (amoutStar > 0)
            {
               FindTeleport();
            }
            AnimationTranslate.Instance.DisplayLoading(false);
            state = StateGame.Playing;
         });
      });
   }

   public void Replay()
   {
      if (state == StateGame.Win)
      {
         level -= 1;
         state = StateGame.Playing;
      }
      AudioManager.Instance.PlaySoundButtonClick();
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            AnimationTranslate.Instance.DisplayLoading(false);
            spawnLevel.SpawmLevel(level - 1);
            amoutStar = MapLevelManager.Instance.ListBtn[level - 1].AmountStar;
            if (amoutStar > 0)
            {
               FindTeleport();
            }
         });
      });
      
   }

   public void Win()
   {
      if(state == StateGame.Win) return;
      state = StateGame.Win;
      DOVirtual.DelayedCall(1f, delegate
      {
         AudioManager.Instance.PlaySoundWin();
         _Scripts.UI.UIController.Instance.UIWin.DisplayWin(true);
         level += 1;
         MapLevelManager.Instance.ListBtn[level - 1].IsLock = true;
         amoutStar = MapLevelManager.Instance.ListBtn[level - 1].AmountStar;
      });
   }

   public void CheckCurrentStar()
   {
      amoutStar -= 1;
      if (amoutStar <= 0)
      {
         if (telePort)
         {
            telePort.SetActive(true);
         }
      }
   }

   private GameObject telePort;
   private void FindTeleport()
   {
      telePort = null;
      DOVirtual.DelayedCall(0.2f, delegate
      {
         telePort = GameObject.FindGameObjectWithTag("Finish");
         if (telePort)
         {
            telePort.SetActive(false);
            Debug.LogError("OK");
         }
      });
   }
   
}
