using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    private float money = 0;
    public TextMeshProUGUI moneyText;
    private int finishedTracks = 0;
    public GameObject melodyBlock;
    public GameObject bassBlock;
    public GameObject melodyButton;
    public GameObject bassButton;
    public AudioSource sfxPlayer;
    public AudioClip accepted;
    public AudioClip notAccepted;
    public AudioClip hit;
    public BeatVisualiser beatVisualiserMelody;
    public BeatVisualiser beatVisualiserBass;
    public int melodyCost;
    public int bassCost;
    public GameObject settingsScreen;
    public Animator transitionAnimator;
    public Animator winAnimator;

    private void Start()
    {
        Invoke("StartTransition", 1f);    
    }

    private void UpdateMoneyText()
    {
        moneyText.text = money.ToString() + "<sprite index= 0>"; 
    }

    public float GetMoney()
    {
        return money;
    }
    public void AddMoney(float newMoney)
    {
        money += newMoney;
        UpdateMoneyText();
    }
    public void AddFinished()
    {
        finishedTracks++;
        if(finishedTracks >= 3 && winAnimator.enabled == false)
        {
            winAnimator.enabled = true;
        }
    }

    public void PurchaseMelody()
    {
        if (money >= melodyCost)
        {
            money -= melodyCost;
            Destroy(melodyBlock);
            Destroy(melodyButton);
            sfxPlayer.PlayOneShot(accepted);
            beatVisualiserMelody.unlocked = true;
            beatVisualiserMelody.UpgradeTrack();
        }
        else sfxPlayer.PlayOneShot(notAccepted);
    }

    public void PurchaseBass()
    {
        if (money >= bassCost)
        {
            money -= bassCost;
            Destroy(bassBlock);
            Destroy(bassButton);
            sfxPlayer.PlayOneShot(accepted);
            beatVisualiserBass.unlocked = true;
            beatVisualiserBass.UpgradeTrack();
        }
        else sfxPlayer.PlayOneShot(notAccepted);
    }

    public void OnCancel()
    {

        if (settingsScreen.activeInHierarchy)
        {
            settingsScreen.SetActive(false);
        }
        else
        {
            settingsScreen.SetActive(true);
        }
    }

    public void SetFullscreen(bool isFull)
    {
        if (isFull) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    private void StartTransition()
    {
        transitionAnimator.enabled = true;
    }
}
