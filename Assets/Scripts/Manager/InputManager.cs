using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event EventHandler OnPauseGame;
    public event EventHandler OnAbilityRelease; //释放技能 
    public event EventHandler OnInteraction;    //与物品交互
    public event EventHandler OnTalk;           //对话 
    public event EventHandler OnMeleeAttack;           //对话 
    private PlayerInputActions playerInputActions;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Pause.performed += PauseOnPerformed;
        playerInputActions.Player.Ability.performed += AbilityOnPerformed;
        playerInputActions.Player.Interaction.performed += InteractionOnPerformed;
        playerInputActions.Player.Talk.performed += TalkOnPerformed;
        playerInputActions.Player.Melee.performed += MeleeOnperformed;
    }
    
    private void OnDestroy()
    {
        playerInputActions.Player.Pause.performed -= PauseOnPerformed;
        playerInputActions.Player.Ability.performed -= AbilityOnPerformed;
        playerInputActions.Player.Interaction.performed -= InteractionOnPerformed;
        playerInputActions.Player.Talk.performed -= TalkOnPerformed;
        playerInputActions.Player.Melee.performed -= MeleeOnperformed;
        playerInputActions.Disable();
    }
    
    private void MeleeOnperformed(InputAction.CallbackContext obj)
    {
        OnMeleeAttack?.Invoke(this, EventArgs.Empty);
    }


    private void TalkOnPerformed(InputAction.CallbackContext obj)
    {
        OnTalk?.Invoke(this, EventArgs.Empty);
    }
    private void InteractionOnPerformed(InputAction.CallbackContext obj)
    {
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }
    private void AbilityOnPerformed(InputAction.CallbackContext obj)
    {
        OnAbilityRelease?.Invoke(this, EventArgs.Empty);
    }
    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        OnPauseGame?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMoveDir()
    {
        Vector2 moveDir = Vector2.zero;
        moveDir = playerInputActions.Player.Move.ReadValue<Vector2>();
        return moveDir.normalized;
    }
}
