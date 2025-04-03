using System;
using System.Collections;
using Character;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MessageTutorial : MonoBehaviour
{
    [SerializeField] private float speedRunText = 0.05f;
    [SerializeField] private float timeDelayNextMessage = 0.5f;
    private int indexMessage = 0;
    public int IndexMessage => indexMessage;

    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private RectTransform popupTransform;
    [SerializeField] private TextMeshProUGUI txtMessage;
    [SerializeField] private GameObject objAvt;

    [SerializeField] private AudioClip[] audioSpeak;
    public AudioSource audioSource;
    public AudioSource audioBG;

    private bool isTyping = false;
    private bool canProceed = false;
    private Coroutine typingCoroutine;

    private bool CanShow = false;

    public string[] messagesEnglish = new[]
    {
        "Oh no. I’m just lost the way to go home.",
        "I'm going where?",
        "What should I do?",
        "Would you be able to assist me in finding the path, traveler?",
        "Press button A to make a left turn",
        "Press button D to make a right turn.",
        "You can press button W/Space to jump.",
        "Use the mouse to drag the map and adjust its position. Help me reach the teleport gate so I can find my way home!"
    };

    public string[] messagesVietNamese  = new[]
    {
        "Ôi không! Tôi vừa bị lạc đường về nhà rồi.",
        "Tôi đang đi đâu vậy?",
        "Tôi nên làm gì bây giờ?",
        "Này cậu, cậu có thể giúp tôi tìm đường về nhà không?",
        "Hãy nhấn phím A để rẽ trái.",
        "Hãy nhấn phím D để rẽ phải.",
        "Bạn có thể nhấn phím W hoặc Space để nhảy.",
        "Dùng chuột kéo bản đồ để điều chỉnh vị trí. Hãy giúp tôi đến cổng dịch chuyển để có thể trở về nhà!"
    };
    public bool useVietNamese;

    private string[] currentMessage;
    private Action actionDone;
    private void Start()
    {
        popupCanvasGroup.alpha = 0;
        popupTransform.localScale = Vector3.zero;
        if (useVietNamese)
        {
            currentMessage = messagesVietNamese;
        }
        else
        {
            currentMessage = messagesEnglish;
        }

        actionDone = ActionDoneTutorial;

    }

    public void ShowDisplayDialog()
    {
        DOVirtual.DelayedCall(0.5f, delegate
        {
            audioBG.Stop();
            CanShow = true;
            ShowMessage();
        });
    }
    private void Update()
    {
        if (!CanShow) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteTyping();
                return;
            }
            if(indexMessage < 4)
            {
                NextMessage();
                return;
            }
        }
        if (indexMessage >= 4)
        {
            if (CheckConditionForStep(indexMessage) && !isTyping)
            {
                switch (indexMessage)
                {
                    case 4:
                        PlayerController.Instance.AnimationPlayer.PlayAnimRun(-1f);
                        PlayerController.Instance.PlayerMovement.Move(-1f, 3f);
                        break;
                    case 5:
                        PlayerController.Instance.AnimationPlayer.PlayAnimRun(1f);
                        PlayerController.Instance.PlayerMovement.Move(1f, 3f);
                        break;
                    case 6:
                        PlayerController.Instance.AnimationPlayer.PlayAnimJump(true);
                        PlayerController.Instance.PlayerMovement.Jump(6f);
                        break;
                }
                NextMessage();
            }
        }
    }

    private void ShowMessage()
    {
        if (indexMessage >= currentMessage.Length) return;

        popupCanvasGroup.DOFade(1, 0.5f).OnComplete(delegate
        {
            PlayerController.Instance.AnimationPlayer.PlayAnimRun(0f);
            PlayerController.Instance.PlayerMovement.Move((indexMessage - 1) == 4?-1:1, 0f);
        });
        popupTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            objAvt.SetActive(true);
            typingCoroutine = StartCoroutine(ProcessingWriteText(speedRunText));
        });
    }

    IEnumerator ProcessingWriteText(float timer)
    {
        isTyping = true;
        canProceed = false;
        txtMessage.text = "";
        string message = currentMessage[indexMessage];

        for (int i = 0; i < message.Length; i++)
        {
            txtMessage.text += message[i];
            audioSource.PlayOneShot(audioSpeak[Random.Range(0, audioSpeak.Length)]);
            yield return new WaitForSeconds(timer);
        }

        isTyping = false;
        canProceed = true;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            audioSource.Stop();
            StopCoroutine(typingCoroutine);
        }
        txtMessage.text = currentMessage[indexMessage];
        isTyping = false;
        canProceed = true;
    }

    private void NextMessage()
    {
        indexMessage++;

        if (indexMessage >= currentMessage.Length)
        {
            popupCanvasGroup.DOFade(0, 0.5f).OnStart(delegate
            {
                objAvt.SetActive(false);
            }).OnComplete(delegate
            {
               actionDone?.Invoke();
            });
            popupTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }
        else
        {
            ShowMessage(); 
        }
    }

    private bool CheckConditionForStep(int step)
    {
        switch (step)
        {
            case 4:
                return Input.GetKeyDown(KeyCode.A);
            case 5:
                return Input.GetKeyDown(KeyCode.D);
            case 6:
                return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
            case 7:
                return Input.GetMouseButtonUp(0);
            default:
                return false;
        }
    }

    public bool FirstShowNoticeSkill
    {
        get => PlayerPrefs.GetInt("FirstShowNotice", 1) == 1;
        set => PlayerPrefs.SetInt("FirstShowNotice", value ? 1 : 0);
    }
    public void ShowMessageNoticeSkill()
    {
        if(!FirstShowNoticeSkill) return;
        indexMessage = 0;
        currentMessage = new[]
        {
            "Bạn có thể bấm phím X để lưa vị trí đang đứng, sau đó ấn phím X bạn sẽ được trở lại vị trí này."
        };
        CanShow = true;
        GameController.Instance.State = StateGame.ShowTutorial;
        ShowMessage();
        actionDone = () => { 
            FirstShowNoticeSkill = false;
            CanShow = false;
            GameController.Instance.State = StateGame.Playing;
        };
    }

    private void ActionDoneTutorial()
    {
        GameController.Instance.IsFirstPlay = false;
        CanShow = false;
        GameController.Instance.State = StateGame.Playing;
    }
}
