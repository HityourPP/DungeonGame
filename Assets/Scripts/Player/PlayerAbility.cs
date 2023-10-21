using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAbility : MonoBehaviour
{
    public UnityEvent abilityToActivate;

    public void ActivateAbility()
    {
        abilityToActivate.Invoke();
    }
}
