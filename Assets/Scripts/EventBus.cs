using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance;

    public UnityEvent<int> OnSoulCollect;
    public UnityEvent ShowUpgrades;
    public UnityEvent OnRelicCollect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
