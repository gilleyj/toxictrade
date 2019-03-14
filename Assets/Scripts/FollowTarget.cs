using System;
using UnityEngine;

[Serializable]
public struct PlaneClamp {
    public float near, far;
}

public class FollowTarget : MonoBehaviour {

    public GameObject target;
    public GameObject home;

    [Header ("General Props")]
    public float smoothTime = 0.3f;
    public float arrivalThreshold = 0.01f;

    [Header ("Zoom Props")]
    public float zoomTime = 0.1f;
    public float zoomSpeed = 1.0f;
    public float zoomAccel = 1.2f;
    public float zoomResetDelay = 0.3f;
    public PlaneClamp clamp = new PlaneClamp { near = 1f, far = 500f };

    private float startTime;
    private bool isZooming;
    private ZoomDir zoomDir;
    private ZoomDir lastZoomDir;
    private float lastZoomedTime;
    private float currZoomSpeed;
    new private GameObject camera;

    void Awake () {
        camera = Camera.main.gameObject;
        currZoomSpeed = this.zoomSpeed;
    }

    void LateUpdate () {
        if (target == null) {
            return;
        }
        float distance = Vector3.Distance (gameObject.transform.position, target.transform.position);
        if (distance >= this.arrivalThreshold) {
            move ();
        }

        float now = Time.time;
        bool resetZoomSpeed = (
            (this.isZooming && now - this.lastZoomedTime > this.zoomResetDelay) ||
            (this.isZooming && this.zoomDir != this.lastZoomDir) ||
            (!this.isZooming)
        );
        if (resetZoomSpeed) {
            this.isZooming = false;
            this.currZoomSpeed = this.zoomSpeed;
        } else {
            this.currZoomSpeed *= this.zoomAccel;
        }
        lastZoomDir = zoomDir;
    }

    public void Target (GameObject go) {
        if (this.target == go) {
            return;
        }

        this.target = go;
        this.startTime = Time.time;
    }

    public void Reset () {
        if (home != null) {
            this.Target (this.home);
        }
    }

    public void Zoom (float units) {
        isZooming = true;
        lastZoomedTime = Time.time;
        zoomDir = units > 0 ? ZoomDir.In : ZoomDir.Out;

        float delta = units * this.currZoomSpeed;
        float distance = camera.transform.localPosition.z + delta;
        Vector3 localPos = camera.transform.localPosition;
        localPos.z = Mathf.Clamp (distance, -clamp.far, -clamp.near);
        camera.transform.localPosition = localPos;
    }

    private void move () {
        float alpha = (Time.time - this.startTime) / smoothTime;

        // SmoothDamp modifies the velocity as this position reaches the target
        // position such that this object will reach its target in approximately
        // a "smoothTime" amount of time.
        gameObject.transform.position = Vector3.Lerp (
            gameObject.transform.position,
            target.transform.position,
            alpha);

        gameObject.transform.rotation = Quaternion.Lerp (
            gameObject.transform.rotation,
            target.transform.rotation,
            alpha);
    }
    enum ZoomDir { In, Out };
}