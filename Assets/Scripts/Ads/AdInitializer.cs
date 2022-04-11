using UnityEngine;
using GoogleMobileAds.Api;

public class AdInitializer : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });
    }
}
