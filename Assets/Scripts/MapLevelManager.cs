using System.Collections.Generic;
using UnityEngine;

public class MapLevelManager : Singleton<MapLevelManager>
{
    protected override void Awake()
    {
        base.KeepAlive(false);
        base.Awake();
    }

    [SerializeField] private List<ButtonLevel> listBtn;
    public List<ButtonLevel> ListBtn => listBtn;
}