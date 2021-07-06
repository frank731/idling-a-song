using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatVisualiser : MonoBehaviour
{
    public float loopLength;
    public List<float> beatValues;
    public GameObject beatTick;
    public List<GameObject> instruments;
    public List<GameObject> activeInstruments;
    public List<AudioSource> tracks;
    public List<int> upgradeCosts;
    public List<int> upgradeIdleCosts;
    public List<int> upgradeBoosts;
    public List<int> upgradeIdleBoosts;
    public GameObject upgradeButton;
    public TextMeshProUGUI upgradeCostText;
    public GameObject upgradeIdleButton;
    public TextMeshProUGUI upgradeIdleCostText;
    public float travelTime;
    public Transform spawnPoint;
    public Transform endPoint;
    public bool isActive;
    public bool unlocked = false;
    public string usedKey;
    public float multiplier = 1;
    public float moneyPerHit = 1;
    public float autoHitTime;
    public TextMeshProUGUI multiplierText;
    public InRangeChecker inRangeChecker;
    public Animator greyOutAnimator;
    public Transform xSpawn;
    public GameObject xPopup;
    public bool upgradeFirst = false;
    private Conducter conducter;
    private int nextInd = 0;
    private int beatCount;
    private int tickInd;
    private int misses = 0;
    private int hits = 0;
    private int inactiveHits = 0;
    public int idleMultiplier = 1;
    private int idleCost;
    private int trackCost;
    private bool idleUpgrade = false;
    private ObjectPooler objectPooler;
    private GameManager gameManager;
    GameObject copy;
    void Start()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPooler.SharedInstance;
        conducter = Conducter.Instance;
        beatCount = beatValues.Count;
        copy = beatTick;
        BeatIndicatorMovement beatIndicatorMovement = copy.GetComponent<BeatIndicatorMovement>();
        beatIndicatorMovement.beatTime = travelTime;
        beatIndicatorMovement.beatMarker = endPoint;
        tickInd = objectPooler.AddObject(copy, 0);
        for(int i = 0; i < beatCount; i++)
        {
            beatValues[i] += loopLength;
        }
        UpdateMultiText();
        if(upgradeFirst) UpgradeTrack(false);
        trackCost = upgradeCosts[0];
        idleCost = upgradeIdleCosts[0];
        upgradeIdleCostText.text = "Cost: " + idleCost + "<sprite index= 0>";
    }

    void Update()
    {
        if(conducter.songPos >= beatValues[nextInd] - travelTime)
        {
            beatValues[nextInd] += loopLength;
            nextInd++;
            nextInd %= beatCount;
            GameObject tick = objectPooler.GetPooledObject(tickInd, spawnPoint);
            tick.transform.SetParent(transform.parent);
            tick.transform.localScale = new Vector3(1, 1, 1);
            tick.GetComponent<Image>().enabled = true;
        }
    }

    private void Hit()
    {
        gameManager.AddMoney(moneyPerHit * multiplier);
    }

    private void UpdateMultiText()
    {
        multiplierText.text = "Current Multiplier: " + multiplier.ToString();
    }

    public void UpgradeTrack(bool playSfx = true)
    {
        if(gameManager.GetMoney() >= trackCost)
        {
            if (playSfx) gameManager.sfxPlayer.PlayOneShot(gameManager.accepted);
            gameManager.AddMoney(trackCost * -1);
            if (upgradeCosts.Count > 0)
            {
                upgradeCosts.RemoveAt(0);
                if (upgradeCosts.Count > 0) trackCost = upgradeCosts[0];
                else trackCost *= 3;
                instruments[0].GetComponent<Image>().material = null;
                instruments[0].GetComponent<Animator>().enabled = true;
                activeInstruments.Add(instruments[0]);
                instruments.RemoveAt(0);
                tracks[0].mute = false;
                tracks.RemoveAt(0);
                moneyPerHit = upgradeBoosts[0];
                upgradeBoosts.RemoveAt(0);
                if (instruments.Count <= 0)
                {
                    gameManager.AddFinished();
                }
            }
            else
            {
                trackCost *= 3;
                moneyPerHit *= 2;
            }
            upgradeCostText.text = "Cost: " + trackCost + "<sprite index= 0>";
            
        }
        else gameManager.sfxPlayer.PlayOneShot(gameManager.notAccepted);
    }

    public void UpgradeIdle()
    {
        if (gameManager.GetMoney() >= idleCost)
        {
            gameManager.sfxPlayer.PlayOneShot(gameManager.accepted);
            gameManager.AddMoney(idleCost * -1);
            
            if (upgradeIdleCosts.Count > 0)
            {
                upgradeIdleCosts.RemoveAt(0);
                if (upgradeIdleCosts.Count > 0) idleCost = upgradeIdleCosts[0];
                else idleCost *= 3;
                if (!idleUpgrade)
                {
                    autoHitTime = upgradeIdleBoosts[0];
                }
                else
                {
                    idleMultiplier = upgradeIdleBoosts[0];
                }
                idleUpgrade = !idleUpgrade;
                upgradeIdleBoosts.RemoveAt(0);
            }
            else
            {
                idleCost *= 3;
                idleMultiplier *= 2;
            }
            
            upgradeIdleCostText.text = "Cost: " + idleCost + "<sprite index= 0>";
            
        }
        else gameManager.sfxPlayer.PlayOneShot(gameManager.notAccepted);
    }
    public void OnPlayButton()
    {
        if (!isActive)
        {
            greyOutAnimator.SetTrigger("Fade Out");
            gameManager.sfxPlayer.PlayOneShot(gameManager.accepted);
            isActive = true;
        }
        else
        {
            greyOutAnimator.SetTrigger("Fade In");
            gameManager.sfxPlayer.PlayOneShot(gameManager.notAccepted);
            isActive = false;
            inactiveHits = 0;
        }
    }

    public void Press(string needed)
    {
        if (needed == usedKey && isActive && unlocked)
        {
            if (inRangeChecker.inRangeTicks.Count > 0)
            {
                Hit();
                hits++;
                if (hits % 5 == 0)
                {
                    multiplier = Mathf.Clamp(multiplier + 1, 1, 15);
                    UpdateMultiText();
                }
                misses = 0;
                inRangeChecker.RemoveTick();
                //gameManager.sfxPlayer.PlayOneShot(gameManager.hit);
                foreach (GameObject instrument in activeInstruments)
                {
                    instrument.GetComponent<Animator>().SetTrigger("Play");
                }
            }
            else
            {
                Instantiate(xPopup, xSpawn);
                //xObj.transform.SetParent(xSpawn);
                //Debug.Log(xObj.name);
                //xObj.SetActive(true);
                //xObj.GetComponent<Animator>().SetTrigger("Move");
                gameManager.sfxPlayer.PlayOneShot(gameManager.notAccepted, 0.5f);
                misses++;
                hits = 0;
                if (misses >= 1) { 
                    multiplier = 1;
                    UpdateMultiText();
                }
            }
        }
    }

    public void OnPressZ()
    {
        Press("Z");
    }

    public void OnPressX()
    {
        Press("X");
    }

    public void OnPressC()
    {
        Press("C");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tick") && !isActive)
        {
            inactiveHits++;
            if(inactiveHits >= autoHitTime)
            {
                gameManager.AddMoney(moneyPerHit * idleMultiplier);
                inactiveHits = 0;
            }
        }
    }

}
