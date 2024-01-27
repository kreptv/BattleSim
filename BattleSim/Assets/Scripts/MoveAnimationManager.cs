using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveAnimationManager : MonoBehaviour
{
    #region Singleton
    public static MoveAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found duplicate MoveAnimationManager in scene");
        }
    }
    #endregion

    public Animator attackHittingPlayer;
    public Animator attackHittingOpponent;

    public Animator playerCharacterAnimator;
    public Animator opponentCharacterAnimator;

    public void PlayAnimation(TypeSO type, int index, bool hittingPlayer)
    {
        if(type.typeName == "Basic")
        {
            if(index == 0)
            {
                TriggerAnimation("Normal 1 Trigger", hittingPlayer);
            }
        }
        else if (type.typeName == "Moist")
        {
            if (index == 0)
            {
                TriggerAnimation("Moist 1 Trigger", hittingPlayer);
            }
        }
        else if (type.typeName == "Leaf")
        {
            if (index == 0)
            {
                TriggerAnimation("Leaf 1 Trigger", hittingPlayer);
            }
        }
        else if (type.typeName == "Flame")
        {
            if (index == 0)
            {
                TriggerAnimation("Flame 1 Trigger", hittingPlayer);
            }
        }
        else if (type.typeName == "Maurice")
        {
            if (index == 0)
            {
                TriggerAnimation("Normal 1 Trigger", hittingPlayer);
            }
        }
    }

    private void TriggerAnimation(string triggerName, bool hittingPlayer)
    {
        if(hittingPlayer)
        {
            attackHittingPlayer.SetTrigger(triggerName);
            opponentCharacterAnimator.SetTrigger("Attack");
            playerCharacterAnimator.SetTrigger("Damaged");
        }
        else
        {
            attackHittingOpponent.SetTrigger(triggerName);
            playerCharacterAnimator.SetTrigger("Attack");
            opponentCharacterAnimator.SetTrigger("Damaged");
            
        }
    }

}
