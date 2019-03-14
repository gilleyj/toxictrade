using UnityEngine;

public class InputManager : MonoBehaviour {
    public FollowTarget follower;

    void Awake () { }

    // Update is called once per frame
    void Update () {
        if (follower != null) {
            this.handleFollowInput ();
        }
    }

    private void handleFollowInput () {
        if (Input.GetMouseButtonDown (0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {
                GameObject go = hit.transform.gameObject;
                follower.Target (go);
            }
        }

        if (Input.GetKeyUp (KeyCode.Escape) && follower.target != null) {
            follower.Reset ();
        }

        float wheelMvmt = Input.GetAxis ("Mouse ScrollWheel");
        if (wheelMvmt != 0f) {
            follower.Zoom (wheelMvmt);
        }
    }
}