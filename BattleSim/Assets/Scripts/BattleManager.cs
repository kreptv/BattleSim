using System.Collections;
using System.Collections.Generic;
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

    public float baseMoveDelayTime = 0.5f;

    public Image playerHealthBar;
    public Image opponentHealthBar;

    private MoveSO playerMove;
    private MoveSO opponentMove;

    [SerializeField]
    private CharacterScript player, opponent;

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

        // TODO: Make sure all this works with Haley's battle scene stuff
    }

    public void PlayerSelectMove(int moveIndex)
    {
        playerMove = player.moveset[moveIndex];
        StartCoroutine(ComputeTurn());
    }

    IEnumerator ComputeTurn()
    {
        SelectOpponentMove();

        // If player goes first
        if (player.spd >= opponent.spd)
        {
            // Uses the player's move since the player is faster
            UseMove(player, opponent, playerMove);
            yield return new WaitForSeconds(baseMoveDelayTime + playerMove.animTime);

            
            // Then uses the opponents move after waiting for the appropriate delay
            UseMove(opponent, player, opponentMove);
            yield return new WaitForSeconds(baseMoveDelayTime + opponentMove.animTime);
        }
        else
        {
            UseMove(opponent, player, opponentMove);
            yield return new WaitForSeconds(baseMoveDelayTime + opponentMove.animTime);

            UseMove(player, opponent, playerMove);
            yield return new WaitForSeconds(baseMoveDelayTime + playerMove.animTime);
        }
    }

    private void SelectOpponentMove()
    {
        // TODO: Add real AI for opponent
        int selectedMoveIndex = Random.Range(0, opponent.moveset.Length);
        opponentMove = opponent.moveset[selectedMoveIndex];
    }

    private void UseMove(CharacterScript user, CharacterScript target, MoveSO move)
    {
        // Calculate move damage
        if (move.effects.Contains(MoveEffect.Damage))
        {
            float effectiveAttack = (float)user.atk * (1.0f + ((float)user.atkBoost / 3.0f));
            float effectiveDefense = target.def * (1 + (target.defBoost / 3));

            if(move.type.strongAgainst.Contains(target.charSO.characterType))
            {
                effectiveAttack *= 2;
            } 
            else if (move.type.weakAgainst.Contains(target.charSO.characterType))
            {
                effectiveAttack /= 2;
            }

            Debug.Log("Effective attack: " +  effectiveAttack);

            float damage = effectiveAttack + move.damagePower / 2f * (100f / (100f + effectiveDefense));

            Debug.Log("Damage: " + damage + "\nEnemy HP: " + target.currentHP);

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
                user.BoostStat(move.statChanged, move.stagesChanged);
            }
            else if(move.statusTarget == Target.Opponent)
            {
                target.BoostStat(move.statChanged, move.stagesChanged);
            }

            // TODO: Play animations
        }
    }

    private void GameOver()
    {
        // TODO: Game over stuff
        Application.Quit();
    }

    private void BattleWon()
    {
        player.LevelUp();

        // TODO: Winning battle stuff
    }

    private void SetHealthBars()
    {
        playerHealthBar.fillAmount = (float)player.currentHP / (float)player.hp;
        Debug.Log("Player health bar fill: " +  playerHealthBar.fillAmount + "\nPlayer health: " + player.currentHP + "/" + player.hp);
        opponentHealthBar.fillAmount = (float)opponent.currentHP / (float)opponent.hp;
    }
}