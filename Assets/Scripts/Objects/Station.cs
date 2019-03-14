using UnityEngine;

public class Station : MonoBehaviour {

    public float wallet = 1000.0f;

    private void Start () {
        Debug.Log ("<color=blue><b>" + gameObject.name.ToString () + "</b> Station Start Called</color>");
    }
}