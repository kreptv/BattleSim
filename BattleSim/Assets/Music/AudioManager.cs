using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public GameManager gm;

    public AudioSource ass;

    public AudioClip battle1;

    public AudioClip battle2;

    public AudioClip battle3;

    public AudioClip battle4;

    public AudioClip battle5;

    public AudioClip battle6;

    public AudioClip menu;


    public void MenuSceneOpened()
    {
        if (ass.clip != menu)
        {
            ass.clip = menu;
            ass.Play();
        }

    }
    public void CharacterSelectSceneOpened()
    {
        MenuSceneOpened(); // redirect to menu audio
    }
    public void BattleSceneOpened()
    {

        if (gm.PlayerProgression == 0 && ass.clip!=battle1)
        {
            ass.clip = battle1;
            ass.Play();

        }
        else if (gm.PlayerProgression == 1 && ass.clip != battle2)
        {
            ass.clip = battle2;
            ass.Play();

        }
        else if (gm.PlayerProgression == 2 && ass.clip != battle3)
        {
            ass.clip = battle3;
            ass.Play();

        }
        else if (gm.PlayerProgression == 3 && ass.clip != battle4)
        {
            ass.clip = battle4;
            ass.Play();

        }
        else if (gm.PlayerProgression == 4 && ass.clip != battle5)
        {
            ass.clip = battle5;
            ass.Play();

        }
        else if (gm.PlayerProgression == 5 && ass.clip != battle6)
        {
            ass.clip = battle6;
            ass.Play();

        }




    }
    public void EndOfRoundSceneOpened()
    {
        ass.Stop();
    }




}
