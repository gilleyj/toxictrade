using System;
using System.Collections.Generic;
using UnityEngine;

public class TradeEngine : MonoBehaviour {
    private ItemDatabase itemDatabase;
    private Cargo cargo;
    private Station station;
    private FactoryEngine factory;

    void Start () {
        this.load();
    }

    void load() {
        this.itemDatabase = GameObject.FindWithTag("DBItems").GetComponent(typeof (ItemDatabase)) as ItemDatabase;
        this.cargo = gameObject.GetComponent(typeof (Cargo)) as Cargo;
        this.station = gameObject.GetComponent(typeof (Station)) as Station;
        this.factory = gameObject.GetComponent(typeof (FactoryEngine)) as FactoryEngine;
    }

    /* Ship is buying goods from us (station) */
    public float BuyGoods(int itemID, int amount) {
        float cost = 0.0f;
        /* make sure we have enough and we won't sell essential items */
        if(this.cargo.HaveEnough(itemID, amount) && this.cargo.ById(itemID).essential!=true ) {
            cost = Mathf.CeilToInt(this.cargo.ById(itemID).value * this.cargo.ById(itemID).markup);
            this.cargo.RemoveCargo(itemID, amount);
            this.station.wallet += cost;
        }
        return cost;
    }

    /* Ship is selling goods TOO us (station) */
    public float SellGoods(int itemID, int amount) {
        float cost = 0.0f;
        /* costs are always rounded up, reasons */
        cost = Mathf.CeilToInt(this.cargo.ById(itemID).value * this.cargo.ById(itemID).markup);
        /* make sure we have enough money plus a bit of a buffer */
        if((this.station.wallet + 100.0f) > cost ) {
            this.cargo.AddCargoById(itemID, amount);
            this.station.wallet -= cost;
        } else cost = 0.0f;
        return cost;
    }

    public List<Manifest> getTrades() {
        /* make a disposable list of wares we (station) is willing to sell .. Make sure essentials cant be bought */
        if(this.cargo == null) this.load();
        if(this.cargo != null) {
            if (this.cargo.manifest != null || this.cargo.manifest.Count != 0) {
                this.cargo.manifest.ForEach(delegate (Manifest ware) {
                    if(ware.essential) {
                        // we pay more for what we want
                        ware.markup = 1.20f;
                    } else if(!ware.essential && this.factory.product.id == ware.id) {
                        // we pay less for what we make
                        ware.markup = 0.80f;
                    } else {
                        // we don't care about shit we don't care about
                        ware.markup = 1.00f;
                    }
                });
            }
        }
        return new List<Manifest>(this.cargo.manifest);
    }
}

