using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdController : MonoBehaviour
{
    public int AdCounter = 0;
    public bool AdsOn = false;
    public int nAttemptsBeforeAd = 3;
    public bool testmode = true;
    string gameId = "3390425";
    [SerializeField]
    int[] preAdAttemptsCurve;
    int currentAdCurvePoint;
    private void Start()
    {
        currentAdCurvePoint = 0;
        AdCounter = 0;
        Advertisement.Initialize(gameId,testmode);
        if(preAdAttemptsCurve.Length>0)
        {
            nAttemptsBeforeAd = preAdAttemptsCurve[currentAdCurvePoint];
        }
    }

    public void AdRunner()
    {
        if (AdsOn && AdCounter >= nAttemptsBeforeAd)
        {
            Debug.Log("Running Ad");
            AdCounter = 0;
            RunAd();
            if (currentAdCurvePoint < preAdAttemptsCurve.Length-1)
            {
                currentAdCurvePoint++;
                nAttemptsBeforeAd = preAdAttemptsCurve[currentAdCurvePoint];
            }
        }
    }

    void RunAd()
    {
        Advertisement.Show();
    }

}
