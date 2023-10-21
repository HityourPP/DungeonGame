using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected enum BossState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Fire,
        Hurt,
        Dead
    }


    protected virtual void SwitchState(BossState currentState,BossState nextState)
    {
        switch (currentState)
        {
            case BossState.Idle:
                ExitIdleState();
                break;
            case BossState.Walk:
                ExitWalkState();
                break;
            case BossState.Attack:
                ExitAttackState();
                break;
            case BossState.Run:
                ExitRunState();
                break;
            case BossState.Hurt:
                ExitHurtState();
                break;
            case BossState.Fire:
                ExitFireState();
                break;
        }
        switch (nextState)
        {
            case BossState.Idle:
                EnterIdleState();
                break;
            case BossState.Walk:
                EnterWalkState();
                break;
            case BossState.Run:
                EnterRunState();
                break;
            case BossState.Attack:
                EnterAttackState();
                break;
            case BossState.Dead:
                EnterDeadState();
                break;
            case BossState.Hurt:
                EnterHurtState();
                break;
            case BossState.Fire:
                EnterFireState();
                break;
        }
    }
//-----Idle State-------
    protected virtual void EnterIdleState()
    {
        
    }      
    protected virtual void UpdateIdleState()
    {
        
    }
    protected virtual void ExitIdleState()
    {
        
    }
//-----Walk State------
    protected virtual  void EnterWalkState()
    {
        
    }     
    protected virtual void UpdateWalkState()
    {
        
    }  
    protected virtual void ExitWalkState()
    {
        
    }
//-----RunToPlayer State------
    protected virtual  void EnterRunState()
    {
        
    }     
    protected virtual void UpdateRunState()
    {
        
    }  
    protected virtual void ExitRunState()
    {
        
    }
//-----Attack State------
    protected virtual void EnterAttackState()
    {
        
    }      
    protected virtual void UpdateAttackState()
    {
        
    }    
    protected virtual void ExitAttackState()
    {
        
    }  
//-----Fire State-----    
    protected virtual void EnterFireState()
    {
        
    }      
    protected virtual void UpdateFireState()
    {
        
    }    
    protected virtual void ExitFireState()
    {
        
    } 
//-----Dead State------   
    protected virtual void EnterDeadState()
    {
        
    }      
//-----Hurt State------
    protected virtual void EnterHurtState()
    {
        
    }
    protected virtual void UpdateHurtState()
    {
        
    }    
    protected virtual void ExitHurtState()
    {
        
    } 
}
