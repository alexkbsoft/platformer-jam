using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private float _damage;

   private Transform _thisTR;

   private void Start()
   {
      _thisTR = GetComponent<Transform>();
      StartCoroutine(Destroyer());
   }

   private void FixedUpdate()
   {
      _thisTR.position += _thisTR.up * _speed * Time.deltaTime;
   }

   IEnumerator Destroyer()
   {
      yield return new WaitForSeconds(3f);
      Destroy(this.gameObject);
   }
}
