using UnityEngine;

public class Bounded : MonoBehaviour {
    [Tooltip ("The boundaries of the world. The scale of this gameObject does not affect this.")]
    public Vector3 bounds = new Vector3 (1000, 1000, 1000);

    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube (transform.position, this.bounds);
    }
}