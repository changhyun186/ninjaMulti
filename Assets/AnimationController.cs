using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator { get; private set; }
    public int throwSkill { get; private set; }
    public int reflectSkill { get; private set; }
    public int dashSkill { get; private set; }
    public int attackSkill { get; private set; }
    public int lopeSkill { get; private set; }
    public int isGround { get; private set; }

    public int isReload { get; private set; }
    public int isSit { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        throwSkill = Animator.StringToHash("throw");
        reflectSkill = Animator.StringToHash("reflect");
        dashSkill = Animator.StringToHash("dash");
        isGround = Animator.StringToHash("isGround");
        attackSkill = Animator.StringToHash("attack");
        isReload = Animator.StringToHash("reload");
        isSit = Animator.StringToHash("sit");
    }
    
    public void Throw()
    {
        animator.SetTrigger(throwSkill);
    }

    public void Reflect()
    {
        animator.SetTrigger(reflectSkill);
    }

    public void Dash()
    {
        animator.SetTrigger(dashSkill);
    }

    public void Attack()
    {
        animator.SetTrigger(attackSkill);
    }

    public void Lope()
    {
        animator.SetTrigger(lopeSkill);
    }

    public void Reload()
    {
        animator.SetTrigger(isReload);
    }
    
    public bool Sit()
    {
        bool sit= !animator.GetBool(isSit);
        animator.SetBool(isSit,sit);
        return sit;
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool(isGround, true);
    }
    private void OnCollisionStay(Collision collision)
    {
        animator.SetBool(isGround, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        animator.SetBool(isGround, false);
    }
}
