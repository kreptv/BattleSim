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

    // CHARACTER SELECT SCENE UI  //
    public TextMeshProUGUI CharacterNameCSS;
    public Image FullBodyPreviewCSS;
    public TextMeshProUGUI DescriptionTextCSS;
    public TextMeshProUGUI StatsCSS;
    // CHARACTER SELECT SCENE UI  //

    // BATTLE SCENE BUTTONS //

    // BATTLE SCENE BUTTONS //


    // BATTLE SCENE UI  //

    public Image PlayerCharacterImage;
    public Image EnemyCharacterImage;

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
            CharacterButtonsCSS[j].onClick.RemoveListener(delegate { CSSPreview(Characters[j]); });

        }

        CharacterNameCSS.text = "No character selected";
        DescriptionTextCSS.text = "";
        StatsCSS.text = "";
        FullBodyPreviewCSS.sprite = BlankSprite;

        // battle scene


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

            CharacterButtonsCSS[j].onClick.AddListener(delegate { CSSPreview(Characters[j]); });

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


    public void CSSPreview(CharacterScript character)
    {

        CharacterNameCSS.text = character.charName;
        DescriptionTextCSS.text = character.charDescription;
        StatsCSS.text = "ATK: " + character.atk + Environment.NewLine +
            "DEF: " + character.def + Environment.NewLine +
            "SPD: " + character.spd + Environment.NewLine +
            "Element: " + character.element;
        FullBodyPreviewCSS.sprite = character.characterImage;

        GoButtonCSS.onClick.AddListener(OpenBattleScene);


    }

    public void OpenBattleScene()
    {
        RemoveAllListeners();
        //PlayerCharacterImage = ;
        //EnemyCharacterImage = dfasd;


        //

        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        CharacterSelectCanvas.SetActive(false);
        BattleCanvas.SetActive(true);
    }



}
