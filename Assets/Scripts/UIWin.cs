using System;
using _Scripts.Extension;
using DG.Tweening;
using UnityEngine;

public class UIWin : MonoBehaviour
{
    [SerializeField] private UIAppear dime;
    [SerializeField] private UIAppear popup;


    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 500);
    }

    public void DisplayWin(bool enable, Action onClosed = null)
    {
        if (enable)
        {
            dime.gameObject.SetActive(true);
            popup.gameObject.SetActive(true);
        }
        else
        {
            dime.gameObject.SetActive(false);
            popup.gameObject.SetActive(false);
            DOVirtual.DelayedCall(0.1f, ()=>onClosed?.Invoke());
        }
    }
}