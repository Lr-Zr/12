using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nara
{
    public class PlayerAnimation : MonoBehaviour
    {
        Animator _animator;

        void Start()
        {
            _animator = transform.GetComponent<Animator>();
        }
        public void SetAnim(PlayerState State)
        {
            _animator.SetInteger("State",(int) State);
        }
        public void SetIsJump(bool isjump)
        {
            _animator.SetBool("IsJump", isjump);
        }
        public void SetIsDJump(bool isDjump)
        {
            _animator.SetBool("IsDJump", isDjump);
        }
        public void SetIsRunning(bool isRunning)
        {
            _animator.SetBool("IsRunning", isRunning);
        }
        public void SetIsAttack(bool isAttack)
        {
            _animator.SetBool("IsAttacking", isAttack);
        }
        public void SetIsSkill(bool isSkill)
        {
            _animator.SetBool("IsSkilling", isSkill);
        }


        public void SetIsOnesec(bool onesec)
        {
            _animator.SetBool("Onesec", onesec);
        }
        public void TriggerAtk()
        {
            _animator.SetTrigger("Attack");
           
        }
        public void TriggerSkill()
        {
            _animator.SetTrigger("Skill");

        
        }
        public void TriggerShield()
        {
            _animator.SetTrigger("Shield");
        }
        public void TriggerKnockOut()
        {
            _animator.SetTrigger("KnockOut");
        }
        public void TriggerStun()
        {

            _animator.SetTrigger("Stun");
        }
        public void TriggerReset()
        {
            _animator.ResetTrigger("Attack");
            _animator.ResetTrigger("Skill");
        }

        public void SetIsUpKey(bool isUpKey)
        {

            _animator.SetBool("UpKey", isUpKey);
        }
        public void SetIsDwKey(bool isDwKey)
        {

            _animator.SetBool("DwKey", isDwKey);
        }
        
        
    }

}