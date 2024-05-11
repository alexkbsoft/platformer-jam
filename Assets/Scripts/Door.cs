using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Door : MonoBehaviour
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
        _hint.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E)){
            EnterDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _hint.SetActive(false);
    }

    private void EnterDoor()
    {
        if (_clip) _audio.PlayOneShot(_clip);
        GameObject playerGO = GameObject.Find("Player");
        playerGO.transform.position = DoorExit.position;
    }
}
