using System;
using _Scripts.Extension;
using DG.Tweening;
using UnityEngine;

public class UIWin : MonoBehaviour
{
    [SerializeField] private UIAppear dime;
    [SerializeField] private UIAppear popup;

    private Vector3 oldPosTxtWin;
    [SerializeField] private Transform txtWin;
    [SerializeField] private CanvasGroup _btnReplay;
    [SerializeField] private CanvasGroup _btnNextLevel;
    private Sequence sequenceShowBtn;

    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 500);
        oldPosTxtWin = txtWin.localPosition; // Sửa từ position -> localPosition
    }

    private void OnDestroy()
    {
        sequenceShowBtn?.Kill(); // Kiểm tra null trước khi Kill()
    }

    public void DisplayWin(bool enable, Action onClosed = null)
    {
        sequenceShowBtn?.Kill(true);
        sequenceShowBtn = DOTween.Sequence(); // Luôn khởi tạo mới

        if (enable)
        {
            dime.gameObject.SetActive(true);
            popup.gameObject.SetActive(true);

            sequenceShowBtn.Append(txtWin.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack));
            sequenceShowBtn.Append(_btnReplay.transform.DOScale(Vector3.one, 1f));
            sequenceShowBtn.Join(_btnReplay.DOFade(1, 0.5f));
            sequenceShowBtn.Append(_btnNextLevel.transform.DOScale(Vector3.one, 1f));
            sequenceShowBtn.Join(_btnNextLevel.DOFade(1, 0.5f));
            sequenceShowBtn.Play();
        }
        else
        {
            sequenceShowBtn.Append(_btnReplay.DOFade(0, 0.5f));
            sequenceShowBtn.Join(_btnNextLevel.DOFade(0, 0.5f));
            sequenceShowBtn.AppendCallback(() =>
            {
                dime.gameObject.SetActive(false);
                popup.gameObject.SetActive(false);
                onClosed?.Invoke();
                ResetUI();
            });

            sequenceShowBtn.Play();
        }
    }

    private void ResetUI()
    {
        _btnReplay.transform.localScale = Vector3.zero;
        _btnReplay.alpha = 0f;

        _btnNextLevel.transform.localScale = Vector3.zero;
        _btnNextLevel.alpha = 0f;

        txtWin.localPosition = oldPosTxtWin;
    }
}
