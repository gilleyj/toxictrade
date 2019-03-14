using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "StationSystemData", menuName = "trade/StationSystemData", order = 0)]
public class StationsData : ScriptableObject {
    public List<StationData> stations;
}

[Serializable]
public class StationData {
    public string name;
    public Vector3 position;
    public Vector3 rotation;
}