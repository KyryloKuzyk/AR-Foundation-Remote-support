using System.Collections;
using ARLocation;
using UnityEngine;
using ARFoundationRemoteInput = ARFoundationRemote.Input;


/// <summary>
/// An example script showing how to provide GPS coordinates to the AR + GPS Location plugin.
/// Add this script to a GameObject with the ARLocationProvider script and make sure the ARLocationProvider.MockLocationData is populated.
/// Tested with AR + GPS Location 3.9.1.
/// </summary>
[RequireComponent(typeof(ARLocationProvider))]
public class ARFoundationRemoteToARGpsBridge : MonoBehaviour {
    ARLocationProvider locationProvider;


    IEnumerator Start() {
        locationProvider = GetComponent<ARLocationProvider>();
        if (locationProvider == null) {
            Debug.LogError($"Please add the {nameof(ARFoundationRemoteToARGpsBridge)} to the GameObject with the {nameof(ARLocationProvider)}.cs script attached.");
        }
        if (locationProvider.MockLocationData == null) {
            Debug.LogError($"Please populate {nameof(ARLocationProvider)}.{nameof(ARLocationProvider.MockLocationData)}.");
        }
        var options = locationProvider.Provider.Options;
        var desiredAccuracyInMeters = (float)(options?.AccuracyRadius ?? 10.0);
        var updateDistanceInMeters = (float)(options?.MinDistanceBetweenUpdates ?? 10.0);
        // only available in AR Foundation Remote 1.4.24-release.1/2.0.24-release.1
        /*while (!ARFoundationRemote.Runtime.ARFoundationRemoteUtils.isConnected) {
            yield return null;
        }*/
        ARFoundationRemoteInput.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
        yield break;
    }

    void Update() {
        if (locationProvider == null) {
            return;
        }
        var arFoundationRemoteLocation = ARFoundationRemoteInput.location;
        if (arFoundationRemoteLocation.status != LocationServiceStatus.Running) {
            return;
        }
        var data = arFoundationRemoteLocation.lastData;
        var mockLocationData = locationProvider.MockLocationData;
        if (mockLocationData == null) {
            return;
        }
        mockLocationData.Location = new Location(data.latitude, data.longitude, data.altitude);
    }
}
