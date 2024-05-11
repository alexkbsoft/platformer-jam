using System.Collections;
using UnityEngine;

namespace Melee
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject weaponAnimator;
        [SerializeField] private AudioSource weaponSound;
        [SerializeField] private float attackCooldown = 0.3f;

        protected bool _isCooldown;
        
        [field: SerializeField] public float Damage { get; set; }
        
        protected void AnimateSword()
        {
            weaponAnimator.SetActive(false);
            weaponAnimator.SetActive(true);
        }  
        
        protected void PlaySoundWeapon() => weaponSound.Play();

        protected IEnumerator Cooldown()
        {
            _isCooldown = true;
            yield return new WaitForSeconds(attackCooldown);
            _isCooldown = false;
        }
    }
}