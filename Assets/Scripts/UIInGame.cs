using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject game;
    public void ShowDisplayHome()
    {
        home.SetActive(true);
        game.SetActive(false);
    }

    public void ShowDisPlayGame()
    {
        home.SetActive(false);
        game.SetActive(true);
    }
}