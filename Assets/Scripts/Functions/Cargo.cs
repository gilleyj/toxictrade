using System;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour {
    private ItemDatabase itemDatabase;
    public List<Manifest> manifest = new List<Manifest>();

    void Start() {
        this.load();
    }

    void load() {
        this.itemDatabase = GameObject.FindWithTag("DBItems").GetComponent(typeof (ItemDatabase)) as ItemDatabase;
    }

    public Manifest ById (int id) {
        return this.manifest.Find(Manifest => Manifest.item.id == id);
    }

    public Manifest ByName (string name) {
        return this.manifest.Find(Manifest => Manifest.item.name.ToLower() == name.ToLower());
    }

    public void AddCargoById (int itemID, int amount, bool essential = false) {
        if(this.itemDatabase == null) this.load();
        if(this.itemDatabase != null) this.AddCargo(this.itemDatabase.ById(itemID), amount, essential);
        else Debug.Log("ItemDatabase not initialized or found? for factory at " + gameObject.name);
   }

    public void AddCargo (Item item, int amount, bool essential = false) {
        if (this.ById(item.id) != null) {
            this.ById(item.id).amount += amount;
            this.ById(item.id).essential = essential;
        } else {
            this.manifest.Add(new Manifest(item, amount, essential));
        }
        // this should be made into a cleanup call, to itearate through manifest and clean any cargo amount < 1
        if (this.ById(item.id).amount < 1 && !this.ById(item.id).essential ) {
            this.manifest.Remove(this.ById (item.id));
        }
    }

    public void RemoveCargoByItem (Item item, int amount) {
        this.RemoveCargo(item.id, amount);
    }

    public bool HaveEnough(int itemID, int amount) {
        return (this.ById(itemID).amount >= amount);
    }

    public void RemoveCargo (int itemID, int amount) {
        if (this.ById(itemID) != null) {
            this.ById(itemID).amount -= amount;
        }
        // this should be made into a cleanup call, to itearate through manifest and clean any cargo amount < 1
        if (this.ById(itemID).amount < 1 && !this.ById(itemID).essential ) {
            this.manifest.Remove(this.ById(itemID));
        }
    }
}

[Serializable]
public class Manifest {
    public Item item;
    public int amount;
    public bool essential;
    public float markup = 1.0f;

    public string name {
        get { return item.name; }
    }

    public int id {
        get { return item.id; }
    }

        public float value {
        get { return item.value * markup; }
    }

    public Manifest (Item item, int amount, bool essential = false) {
        this.item = item;
        this.amount = amount;
        this.essential = essential;
    }

}
