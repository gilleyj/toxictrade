using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public GameObject target;
    public Cargo cargo;
    public float arriveThreshold = 0.5f;
    public float orientationThreshold = 0.1f;
    public float linearSpeed = 0.4f;
    public float angularSpeed = 2.0f;
    public bool isTraveling = false;
    public float wallet = 1000.0f;

    private void Start () { 
        this.load();
    }

    void load() {
        this.cargo = gameObject.GetComponent(typeof (Cargo)) as Cargo;
    }

    public void Target (GameObject target) {
        this.target = target;
    }

    public float Distance () {
        if (target == null) {
            return 0f;
        }
        return Vector3.Distance (target.transform.position, gameObject.transform.position);
    }

    void Update () {
        if (this.target == null) {
            return;
        }
        if (this.Distance () > 0.5f) {
            this.isTraveling = true;
            // Move our position a step closer to the target.
            gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, target.transform.position, linearSpeed * Time.deltaTime);
            Debug.DrawLine (gameObject.transform.position, target.transform.position, Color.blue);
        } else {
            this.isTraveling = false;
            this.arrived();
        }

        Vector3 heading = Vector3.Normalize (target.transform.position - gameObject.transform.position);
        if (heading == Vector3.zero) {
            return;
        }
        float angle = Vector3.Dot (gameObject.transform.forward, heading);

        if (angle < 1 + -orientationThreshold || angle > 1 + orientationThreshold) {
            Quaternion targetRotation = Quaternion.LookRotation (heading, Vector3.up);
            gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation, targetRotation, angularSpeed * Time.deltaTime);

            Debug.DrawRay (gameObject.transform.position, -heading * linearSpeed, Color.red);
        }
    }

    private void arrived() {
        float cost;
        if(this.cargo == null) this.load();
        TradeEngine trader = target.GetComponent(typeof (TradeEngine)) as TradeEngine;
        List<Manifest> availablecrap = trader.getTrades();
        // lets just buy all their crap.
        availablecrap.ForEach(delegate (Manifest ware) {
            if(!ware.essential) {
                // buy stuff
                int buyamount = (int)Mathf.Floor(ware.amount * 0.90f) + 1;
                cost = trader.BuyGoods(ware.id, buyamount);
                if(cost>0.0f) {
                    this.cargo.AddCargoById(ware.id, buyamount);
                    this.wallet -= cost;
                }
            } else {
                // sell stuff
                if ( this.cargo.ById(ware.id) != null ) {
                    Debug.Log("We could sell " + ware.name);
                    int sellamount = (int)this.cargo.ById(ware.id).amount;
                    
                }
            }
        });
        // so garbage collection on availablecrap needs to happen... according to C# this happens automagically.
        // I don't believe it

    }
}
