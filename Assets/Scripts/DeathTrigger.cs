using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private GameObject _hint;
    [SerializeField] private Transform DoorExit;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject PlayerGO = GameObject.Find("Player");
        Health.HealthProcessor HP = PlayerGO.GetComponent<Health.HealthProcessor>();
        HP.TakeDamage(1000);
    }
}
