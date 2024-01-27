 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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


    public int ActiveCharacter;
    public int EnemyCharacter;
    public int PlayerProgression = 0;

    // CHARACTER SELECT SCENE UI  //
    public TextMeshProUGUI CharacterNameCSS;
    public Image FullBodyPreviewCSS;
    public TextMeshProUGUI DescriptionTextCSS;
    public TextMeshProUGUI StatsCSS;
    // CHARACTER SELECT SCENE UI  //

    // BATTLE SCENE BUTTONS //
    public Button AttackButton;
    public Button DefendButton;

    public Button Attack1Button;
    public Button Attack2Button;
    public Button Attack3Button;
    // BATTLE SCENE BUTTONS //


    // BATTLE SCENE UI  //

    public Image PlayerCharacterImage;
    public Image EnemyCharacterImage;

    public GameObject GeneralBattlePanel;
    public GameObject AttackBattlePanel;
    public GameObject EmptyBattlePanel;





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

        Attack1Button.onClick.RemoveListener(UseAttack1);
        Attack2Button.onClick.RemoveListener(UseAttack2);
        Attack3Button.onClick.RemoveListener(UseAttack3);


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

        CharacterNameCSS.text = character.charName;
        DescriptionTextCSS.text = character.charDescription;
        StatsCSS.text = "ATK: " + character.atk + Environment.NewLine +
            "DEF: " + character.def + Environment.NewLine +
            "SPD: " + character.spd + Environment.NewLine +
            "Element: " + character.element;
        FullBodyPreviewCSS.sprite = character.characterImage;

        ActiveCharacter = index;

        GoButtonCSS.onClick.AddListener(InitiateBattleScene);


    }

    public void InitiateBattleScene()
    {
        FindNextEnemy();
        PlayerCharacterImage.sprite = Characters[ActiveCharacter].characterImage;
        EnemyCharacterImage.sprite = Characters[EnemyCharacter].characterImage;
        OpenBattleScene();
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
        Attack1Button.onClick.AddListener(UseAttack1);
        Attack2Button.onClick.AddListener(UseAttack2);
        Attack3Button.onClick.AddListener(UseAttack3);
        //

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(true);

        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(true);
        EmptyBattlePanel.SetActive(false);
    }

    public void UseAttack1()
    {
        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(true);

        // Check player vs enemy speed for who goes first
        // Player Attacks
        // Enemy Attacks

        // Check for enemy death
        // Check for player death
        OpenBattleScene();

    }

    public void UseAttack2()
    {
        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(true);

        // Check player vs enemy speed for who goes first
        // Player Attacks
        // Enemy Attacks

        // Check for enemy death
        // Check for player death
        OpenBattleScene();

    }

    public void UseAttack3()
    {
        GeneralBattlePanel.SetActive(false);
        AttackBattlePanel.SetActive(false);
        EmptyBattlePanel.SetActive(true);

        // Check player vs enemy speed for who goes first
        // Player Attacks
        // Enemy Attacks

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

            switch (ActiveCharacter)
            {
                case 0:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 1;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 2;
                    }
                    break;

                case 1:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 0;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 2;
                    }
                    break;

                case 2:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 0;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 1;
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = myRand;
                    break;
            }

        }

        if (PlayerProgression == 1)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacter)
            {
                case 3:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 4;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 5;
                    }
                    break;

                case 4:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 3;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 5;
                    }
                    break;

                case 5:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 3;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 4;
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = myRand+3;
                    break;
            }

        }

        if (PlayerProgression == 2)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacter)
            {
                case 6:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 7;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 8;
                    }
                    break;

                case 7:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 6;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 8;
                    }
                    break;

                case 8:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 6;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 7;
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = myRand + 6;
                    break;
            }

        }

        if (PlayerProgression == 3)
        {
            int myRand = UnityEngine.Random.Range(0, 2);
            Debug.Log(myRand);

            switch (ActiveCharacter)
            {
                case 9:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 10;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 11;
                    }
                    break;

                case 10:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 9;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 11;
                    }
                    break;

                case 11:
                    if (myRand == 0)
                    {
                        EnemyCharacter = 9;
                    }
                    else if (myRand == 1)
                    {
                        EnemyCharacter = 10;
                    }
                    break;

                default:
                    myRand = UnityEngine.Random.Range(0, 3);
                    EnemyCharacter = myRand + 9;
                    break;
            }

        }

        if (PlayerProgression == 4)
        {
            EnemyCharacter = 12; // Maurice

        }








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





    }



}
