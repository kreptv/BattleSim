 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor.Build;

public class GameManager : MonoBehaviour
{

    // MENU SCENE BUTTONS //
    public Button StartButton;
    public Button CreditsButton;
    public Button BackButton;
    // MENU SCENE BUTTONS //

    // SCENE CANVASES //
    public GameObject MenuCanvas;
    public GameObject CreditsCanvas;
    public GameObject CharacterSelectCanvas;
    public GameObject BattleCanvas;
    // SCENE CANVASES //

    // CHARACTER SELECT SCENE BUTTONS //
    public Button BackToMenuButtonCSS;
    public Button GoButtonCSS;

                           // Rahdy 0, LillyFlynn 1, Foge 2,
                           // Felix 3, Jamie 4, Messy 5,
                           // Cosmos 6, Koda 7, Adrian 8,
                           // Zero 9, Skoryx 10, Hadrian 11

    public Button[] CharacterButtonsCSS;

    // CHARACTER SELECT SCENE BUTTONS //

    public CharacterScript[] Characters;
    public Sprite BlankSprite;


    public CharacterScript ActiveCharacter;
    private int ActiveCharacterIndex;
    public CharacterScript EnemyCharacter;
    public int PlayerProgression = 0;

    // CHARACTER SELECT SCENE UI  //
    public TextMeshProUGUI CharacterNameCSS;
    public Image FullBodyPreviewCSS;
    public TextMeshProUGUI DescriptionTextCSS;
    public TextMeshProUGUI StatsCSS;
    // CHARACTER SELECT SCENE UI  //

    // BATTLE SCENE BUTTONS //
    [Space(20)]
    [Header("Battle Scene Buttons")]
    public Button AttackButton;
    public Button DefendButton;

    public Button Attack1Button;
    public Button Attack2Button;
    public Button Attack3Button;

    public TextMeshProUGUI Attack1NameText;
    public TextMeshProUGUI Attack2NameText;
    public TextMeshProUGUI Attack3NameText;

    public TextMeshProUGUI Attack1DescriptionText;
    public TextMeshProUGUI Attack2DescriptionText;
    public TextMeshProUGUI Attack3DescriptionText;
    // BATTLE SCENE BUTTONS //


    // BATTLE SCENE UI  //

    public Image PlayerCharacterImage;
    public Image EnemyCharacterImage;

    public GameObject GeneralBattlePanel;
    public GameObject AttackBattlePanel;
    public GameObject EmptyBattlePanel;

    // BATTLE SCENE UI  //

    // END OF ROUND SCENE //
    public GameObject YouLostCanvas;
    public GameObject YouWinCanvas;
    public GameObject LevelUpCanvas;

    public Button FromLostSceneToCharacterSelectionButton;
    public Button FromWinSceneToCharacterSelectionButton;
    public Button FromLevelUpToCharacterSelectionButton;
    // END OF ROUND SCENE //






    // BATTLE SCENE UI  //


    void Start()
    {
        OpenMenu();
    }

    public void RemoveAllListeners()
    {
        // menu scene
        StartButton.onClick.RemoveListener(OpenCharacterSelectScene);
        CreditsButton.onClick.RemoveListener(OpenCredits);

        // credits scene
        BackButton.onClick.RemoveListener(OpenMenu);

        // character selection scene
        BackToMenuButtonCSS.onClick.RemoveListener(OpenMenu);
        GoButtonCSS.onClick.RemoveListener(OpenBattleScene);

        for (int i = 0; i < 12; i++)
        {
            int j = i;
            CharacterButtonsCSS[j].onClick.RemoveListener(delegate { CSSPreview(Characters[j], j); });

        }

        CharacterNameCSS.text = "No character selected";
        DescriptionTextCSS.text = "";
        StatsCSS.text = "";
        FullBodyPreviewCSS.sprite = BlankSprite;

        // battle scene

        AttackButton.onClick.RemoveListener(OpenAttackMenu);
        DefendButton.onClick.RemoveListener(UseDefend);

        Attack1Button.onClick.RemoveListener(delegate { UseAttack(0); });
        Attack2Button.onClick.RemoveListener(delegate { UseAttack(1); });
        Attack3Button.onClick.RemoveListener(delegate { UseAttack(2); });


        // End of round scene

        FromLevelUpToCharacterSelectionButton.onClick.RemoveListener(OpenCharacterSelectScene);
        FromLostSceneToCharacterSelectionButton.onClick.RemoveListener(OpenCharacterSelectScene);
        FromWinSceneToCharacterSelectionButton.onClick.RemoveListener(OpenCharacterSelectScene);


    }

public void OpenMenu()
    {
        RemoveAllListeners();
        StartButton.onClick.AddListener(OpenCharacterSelectScene);
        CreditsButton.onClick.AddListener(OpenCredits);

        MenuCanvas.SetActive(true);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(false);
    }

    public void OpenCredits()
    {
        RemoveAllListeners();
        BackButton.onClick.AddListener(OpenMenu);

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(false);
    }

    public void OpenCharacterSelectScene()
    {
        RemoveAllListeners();
        BackToMenuButtonCSS.onClick.AddListener(OpenMenu);

        //

        LevelUpCanvas.SetActive(false);
        YouWinCanvas.SetActive(false);
        YouLostCanvas.SetActive(false);

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(true);
        BattleCanvas.SetActive(false);

        //

        for (int i = 0; i<12; i++)
        {
            int j = i;
            Characters[i] = CharacterButtonsCSS[i].gameObject.GetComponent<CharacterScript>();

            CharacterButtonsCSS[j].onClick.AddListener(delegate { CSSPreview(Characters[j], j); });

            if (Characters[i].isLocked) // character locked, so show lock image & disable button
            {
                CharacterButtonsCSS[i].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                CharacterButtonsCSS[i].interactable = false;
            }
            else // character unlocked, so hide lock image & enable button
            {
                CharacterButtonsCSS[i].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                CharacterButtonsCSS[i].interactable = true;
            }

        }

    }


    public void CSSPreview(CharacterScript character, int index)
    {

        CharacterNameCSS.text = character.charSO.characterName;
        DescriptionTextCSS.text = character.charSO.characterDescription;
        StatsCSS.text = "ATK: " + character.atk + Environment.NewLine +
            "DEF: " + character.def + Environment.NewLine +
            "SPD: " + character.spd + Environment.NewLine +
            "Element: " + character.charSO.characterType;
        FullBodyPreviewCSS.sprite = character.charSO.characterSprite;

        ActiveCharacter = Characters[index];
        ActiveCharacterIndex = index;

        GoButtonCSS.onClick.AddListener(InitiateBattleScene);


    }

    public void InitiateBattleScene()
    {
        FindNextEnemy();
        PlayerCharacterImage.sprite = ActiveCharacter.charSO.characterSprite;
        EnemyCharacterImage.sprite = EnemyCharacter.charSO.characterSprite;
        OpenBattleScene();

        BattleManager.Instance.GenerateBattle(ActiveCharacter, EnemyCharacter);
    }

    public void OpenBattleScene()
    {
        RemoveAllListeners();

        //
        AttackButton.onClick.AddListener(OpenAttackMenu);
        DefendButton.onClick.AddListener(UseDefend);
        //

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(true);

        GeneralBattlePanel.SetActive(true);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(false);
    }

    public void OpenAttackMenu()
    {
        RemoveAllListeners();

        //
        Attack1Button.onClick.AddListener(delegate { UseAttack(0); });
        Attack2Button.onClick.AddListener(delegate { UseAttack(1); });
        Attack3Button.onClick.AddListener(delegate { UseAttack(2); });
        //

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(true);

        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(true);
        EmptyBattlePanel.SetActive(false);

        InitializeAttackPannel();
    }

    public void UseAttack(int index)
    {
        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(true);

        BattleManager.Instance.PlayerSelectMove(index);

        // Check for enemy death
        // Check for player death
        OpenBattleScene();

    }

    public void UseDefend()
    {
        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(true);

        // Player Defends
        // Enemy Attacks

        // Check for enemy death
        // Check for player death
        OpenBattleScene();

    }


    public void FindNextEnemy()
    {
        if (PlayerProgression == 0)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacterIndex)
            {
                case 0:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[1];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[2];
                    }
                    break;

                case 1:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[0];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[2];
                    }
                    break;

                case 2:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[0];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[1];
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = Characters[myRand];
                    break;
            }

        }

        if (PlayerProgression == 1)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacterIndex)
            {
                case 3:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[4];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[5];
                    }
                    break;

                case 4:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[3];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[5];
                    }
                    break;

                case 5:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[3];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[4];
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = Characters[myRand+3];
                    break;
            }

        }

        if (PlayerProgression == 2)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacterIndex)
            {
                case 6:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[7];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[8];
                    }
                    break;

                case 7:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[6];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[8];
                    }
                    break;

                case 8:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[6];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[7];
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = Characters[myRand + 6];
                    break;
            }

        }

        if (PlayerProgression == 3)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacterIndex)
            {
                case 9:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[10];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[11];
                    }
                    break;

                case 10:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[9];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[11];
                    }
                    break;

                case 11:
                    if (myRand == 0)
                    {
                        EnemyCharacter = Characters[9];
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = Characters[10];
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = Characters[myRand + 9];
                    break;
            }

        }

        if (PlayerProgression == 4)
        {
            EnemyCharacter = Characters[12]; // Maurice

        }



    }

    public void LoseBattle()
    {
        YouLostCanvas.SetActive(true);
        FromLostSceneToCharacterSelectionButton.onClick.AddListener(OpenCharacterSelectScene);
    }



    public void WinBattle()
    {
        // UNLOCK NEW CHARACTERS //

        PlayerProgression++;

        if (PlayerProgression == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Characters[i].isLocked = false;
            }
        }
        else if (PlayerProgression == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                Characters[i].isLocked = false;
            }
        }
        else if (PlayerProgression == 2)
        {
            for (int i = 0; i < 9; i++)
            {
                Characters[i].isLocked = false;
            }
        }
        else if (PlayerProgression == 3)
        {
            for (int i = 0; i < 9; i++)
            {
                Characters[i].isLocked = false;
            }
        }
        else if (PlayerProgression == 4)
        {
            for (int i = 0; i < 12; i++)
            {
                Characters[i].isLocked = false;
            }
        }
        // UNLOCK NEW CHARACTERS //

        if (PlayerProgression == 5)
        {
            // win
            YouWinCanvas.SetActive(true);
            FromWinSceneToCharacterSelectionButton.onClick.AddListener(OpenCharacterSelectScene);

            PlayerProgression = 0;
            for (int i = 3; i< 12; i++)
            {
                Characters[i].isLocked = true;
            }
        } else if (PlayerProgression < 5)
        {
            // level up
            LevelUpCanvas.SetActive(true);
            FromLevelUpToCharacterSelectionButton.onClick.AddListener(OpenCharacterSelectScene);
        }





    }


    private void InitializeAttackPannel()
    {
        Attack1Button.gameObject.GetComponent<Image>().color = ActiveCharacter.moveset[0].type.typeColor;
        Attack2Button.gameObject.GetComponent<Image>().color = ActiveCharacter.moveset[1].type.typeColor;
        Attack3Button.gameObject.GetComponent<Image>().color = ActiveCharacter.moveset[2].type.typeColor;

        Attack1NameText.text = ActiveCharacter.moveset[0].moveName;
        Attack2NameText.text = ActiveCharacter.moveset[1].moveName;
        Attack3NameText.text = ActiveCharacter.moveset[2].moveName;

        Attack1DescriptionText.text = ActiveCharacter.moveset[0].moveDescription;
        Attack2DescriptionText.text = ActiveCharacter.moveset[1].moveDescription;
        Attack3DescriptionText.text = ActiveCharacter.moveset[2].moveDescription;

    }
}
