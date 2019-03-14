using UnityEngine;

[RequireComponent (typeof (Bounded))]
public class World : MonoBehaviour {
    public static World Instance;

    public GameObject prefabShip;
    public StationsManager stationsMgr;

    private Vector3 bounds;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        }
        if (Instance != this) {
            Destroy (gameObject);
            return;
        }
        this.bounds = GetComponent<Bounded> ().bounds;
    }

    // Start is called before the first frame update
    void Start () {
        // why this?  to remind me to make the company and ships array
        float newX = Random.Range (-4.0f, 4.0f);
        float newY = Random.Range (-2.0f, 2.0f);
        float newZ = Random.Range (-3.0f, 3.0f);
        Vector3 newPos = new Vector3 (newX, newY, newZ);
        this.prefabShip = Instantiate (prefabShip, Vector3.zero, Quaternion.identity);
        this.prefabShip.name = "StupidMobile";
        this.prefabShip.GetComponent<Ship> ().Target (randomStation ().gameObject);

    }

    // Update is called once per frame
    void Update () {
        Ship ship = this.prefabShip.GetComponent<Ship> ();
        if (!ship.isTraveling) {
            if (this.stationsMgr == null) {
                Debug.LogError ("wtf where did the station go");
                return;
            }
            ship.Target (randomStation ().gameObject);
        }

    }

    private Station randomStation () {
        if (stationsMgr.GetComponent<StationsManager> () == null) {
            Debug.LogError ("wtf where did the script on the station go");
            return null;
        } else if (stationsMgr.stations == null) {
            Debug.LogError ("wtf where did the stations list go");
            return null;

        }

        int targetStation = Random.Range (0, stationsMgr.stations.Count);
        return stationsMgr.stations[targetStation];
    }
}