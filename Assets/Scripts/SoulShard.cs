using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoulShard : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _clip;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventBus.Instance.OnSoulCollect.Invoke(1);
        gameObject.SetActive(false);
        _audio.PlayOneShot(_clip);
    }
}
