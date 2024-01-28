using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour 
{
    #region Singleton
    public static BattleManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found duplicate BattleManager in scene");
        }
    }
    #endregion

    [Header("Koda is cringe for making me do this")]
    public GameManager gameManager;

    [Header("non-cringe fields")]
    public float baseMoveDelayTime = 0.5f;

    public Image playerHealthBar;
    public Image opponentHealthBar;

    public TextMeshProUGUI playerActionTextbox;
    public TextMeshProUGUI opponentActionTextbox;

    private AudioSource aSource;

    private MoveSO playerMove;
    private MoveSO opponentMove;

    private string playerActionText;
    private string opponentActionText;

    private CharacterScript player, opponent;

    private Coroutine battleCoroutine;

    public void GenerateBattle(CharacterScript p, CharacterScript opp)
    {
        player = p;
        opponent = opp;

        player.currentHP = player.hp;
        opponent.currentHP = opp.hp;

        player.atkBoost = 0;
        player.defBoost = 0;
        player.spdBoost = 0;
        opponent.atkBoost = 0;
        opponent.defBoost = 0;
        opponent.spdBoost = 0;

        SetHealthBars();

        aSource = gameManager.am.ass2;
        // TODO: Make sure all this works with Haley's battle scene stuff
    }

    public void PlayerSelectMove(int moveIndex)
    {
        playerMove = player.moveset[moveIndex];
        battleCoroutine = StartCoroutine(ComputeTurn());
    }

    IEnumerator ComputeTurn()
    {
        SelectOpponentMove();

        playerActionText = string.Empty;
        opponentActionText = string.Empty;
        playerActionTextbox.text = string.Empty;
        opponentActionTextbox.text = string.Empty;

        float playerEffectiveSpeed = (float)player.spd * ((float)player.spdBoost / 3f);
        float opponentEffectiveSpeed = (float)opponent.spd * ((float)opponent.spdBoost / 3f);

        // If player goes first
        if (playerEffectiveSpeed >= opponentEffectiveSpeed)
        {
            // Uses the player's move since the player is faster
            playerActionText = UseMove(player, opponent, playerMove, playerActionText);
            playerActionTextbox.text = playerActionText;
            if (playerMove.effects.Contains(MoveEffect.Damage))
            {
                MoveAnimationManager.Instance.PlayAnimation(playerMove.type, playerMove.animationIndex, false);
            }
            
            yield return new WaitForSeconds(baseMoveDelayTime + playerMove.animTime);
            if(CheckForDeath()) { yield break; }


            // Then uses the opponents move after waiting for the appropriate delay
            opponentActionText = UseMove(opponent, player, opponentMove, opponentActionText);
            opponentActionTextbox.text = opponentActionText;
            if (opponentMove.effects.Contains(MoveEffect.Damage))
            {
                MoveAnimationManager.Instance.PlayAnimation(opponentMove.type, opponentMove.animationIndex, true);
            }

            yield return new WaitForSeconds(baseMoveDelayTime + opponentMove.animTime);
            if (CheckForDeath()) { yield break; }
        }
        else
        {
            opponentActionText = UseMove(opponent, player, opponentMove, opponentActionText);
            opponentActionTextbox.text = opponentActionText;
            if (opponentMove.effects.Contains(MoveEffect.Damage))
            {
                MoveAnimationManager.Instance.PlayAnimation(opponentMove.type, opponentMove.animationIndex, true);
            }
                
            yield return new WaitForSeconds(baseMoveDelayTime + opponentMove.animTime);
            if (CheckForDeath()) { yield break; }

            playerActionText = UseMove(player, opponent, playerMove, playerActionText);
            playerActionTextbox.text = playerActionText;
            if (playerMove.effects.Contains(MoveEffect.Damage))
            {
                MoveAnimationManager.Instance.PlayAnimation(playerMove.type, playerMove.animationIndex, false);
            }
                
            yield return new WaitForSeconds(baseMoveDelayTime + playerMove.animTime);
            if (CheckForDeath()) { yield break; }
        }

        yield return new WaitForSeconds(1.0F);
        gameManager.OpenBattleScene();
    }

    private void SelectOpponentMove()
    {
        // TODO: Add real AI for opponent
        int selectedMoveIndex = Random.Range(0, opponent.moveset.Length);
        opponentMove = opponent.moveset[selectedMoveIndex];
    }

    private string UseMove(CharacterScript user, CharacterScript target, MoveSO move, string moveText)
    {
        aSource.clip = move.soundEffect;
        aSource.Play();
        moveText = user.charSO.characterName + " used " + move.moveName + "\n";
        // Calculate move damage
        if (move.effects.Contains(MoveEffect.Damage))
        {
            float effectiveAttack = (float)user.atk * (1.0f + ((float)user.atkBoost / 3.0f));
            float effectiveDefense = (float)target.def * (1.0f + ((float)target.defBoost / 3f));

            Debug.Log(target.charSO.characterName + " base defense: " + target.def + "\nEffective defense: " + effectiveDefense);

            float damage = effectiveAttack + move.damagePower / effectiveDefense;

            if (move.type.strongAgainst.Contains(target.charSO.characterType))
            {
                moveText += "It's highly effective\n";
                damage *= 1.5f;
            }
            else if (move.type.weakAgainst.Contains(target.charSO.characterType))
            {
                moveText += "It's sucks.\n";
                damage /= 1.5f;
            }

            damage = Mathf.Max(1, damage);

            Debug.Log(user.charSO.characterName + " dealing damage: " + (int)damage);

            target.currentHP = Mathf.Max((int)target.currentHP - (int)damage, 0);
            // TODO: Play animations

            SetHealthBars();
            
        }

        // TODO: calculate status
        if (move.effects.Contains(MoveEffect.Status))
        {
            // TODO: Status decreasing moves

            if(move.statusTarget == Target.Self) 
            {
                moveText += user.charSO.characterName + "'s " + move.statChanged + " increased by " + move.stagesChanged + " stage(s).\n";
                user.BoostStat(move.statChanged, move.stagesChanged);
            }
            else if(move.statusTarget == Target.Opponent)
            {
                moveText += opponent.charSO.characterName + "'s " + move.statChanged + " increased by " + move.stagesChanged + " stage(s).\n";
                target.BoostStat(move.statChanged, move.stagesChanged);
            }
            
        }

        return moveText;
    }

    private bool CheckForDeath()
    {
        if(player.currentHP <= 0)
        {
            StartCoroutine(gameManager.LoseBattle());
            StopCoroutine(battleCoroutine);
            return true;
        }
        else if(opponent.currentHP <= 0)
        {
            StartCoroutine(gameManager.WinBattle());
            player.LevelUp();
            StopCoroutine (battleCoroutine);
            return true;
        }
        return false;
    }

    private void SetHealthBars()
    {
        playerHealthBar.fillAmount = (float)player.currentHP / (float)player.hp;
        opponentHealthBar.fillAmount = (float)opponent.currentHP / (float)opponent.hp;
    }
}
