using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Bounded))]
public class StationsManager : MonoBehaviour {

    public Station prefabStation;

    public StationsData data;

    public List<Station> stations;

    public Vector3 bounds;

    public void Awake () {
        this.stations = new List<Station> ();
        foreach (Transform child in gameObject.transform) {
            this.stations.Add (child.GetComponent<Station> ());
        }
    }

    public void Load () {
        this.Clear ();
        stations = new List<Station> ();
        foreach (var stationData in data.stations) {
            Station station = Instantiate (
                prefabStation,
                stationData.position,
                Quaternion.Euler (stationData.rotation));

            station.name = stationData.name;
            station.transform.SetParent (transform);

            this.stations.Add (station);
        }
        bounds = GetComponent<Bounded> ().bounds;
    }

    public void Clear () {
        GameObject[] tmpArr = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < tmpArr.Length; i++) {
            tmpArr[i] = gameObject.transform.GetChild (i).gameObject;
        }
        foreach (var child in tmpArr) {
            DestroyImmediate (child);
        }
    }

}