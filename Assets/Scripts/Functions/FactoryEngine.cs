using System.Collections.Generic;
using UnityEngine;

public class FactoryEngine : MonoBehaviour {
    private ItemDatabase itemDatabase;
    private Cargo cargo;

    public Item product = new Item ();
    public float manufactureCycleSecs = 20.0f;
    public float timer = 0.0f;
    public float cooldownSecs = 5.0f;
    public bool powerOn = false;
    public bool lastPower = true;
    public int state = 0;

    void Start () {
        // randomize the build process a bit for fun
        this.manufactureCycleSecs = Random.Range (10, 30);
        this.cooldownSecs = Random.Range (2, 8);

        // Use tags to find the item inventory database object and get the itemdatabase component of it
        this.itemDatabase = GameObject.FindWithTag("DBItems").GetComponent(typeof (ItemDatabase)) as ItemDatabase;
        this.cargo = gameObject.GetComponent(typeof (Cargo)) as Cargo;

        // find all the buildable products
        List<int> potential_products = new List<int> ();
        this.itemDatabase.items.ForEach (delegate (Item potential_product) {
            if (potential_product.components == null || potential_product.components.Count != 0) {
                potential_products.Add(potential_product.id);
            }
            if(Random.Range(1,10)>6) {
                this.cargo.AddCargoById(potential_product.id, Random.Range(1,20), false);
            }
        });

        //
        // pick a product and preload our cargo so we can make 20 or so
        this.product = this.itemDatabase.ById(potential_products[Random.Range(0, potential_products.Count)]);
        this.product.components.ForEach (delegate (Component ingredient) {
            this.cargo.AddCargoById(ingredient.id, ingredient.amount * 20, true);
        });

        // start building
        this.startBuild ();

    }
    void Update () {
        // see if we can power on
        this.powerOn = this.hasIngredients ();

        // the process state machine
        if (this.powerOn) {
            this.timer += Time.deltaTime;
            if (this.state == 2 && this.timer > (manufactureCycleSecs + cooldownSecs)) {
                this.cycleComplete ();
            } else if (this.state == 1 && this.timer > manufactureCycleSecs) {
                this.finishBuild ();
            } else if (this.state == 0) startBuild ();
        }

        // if the power state changed we might want to do something
        if (this.lastPower != this.powerOn) {
            // Debug.Log(gameObject.name + " power from " + this.lastPower + " to " + this.powerOn);
            this.lastPower = this.powerOn;
        }
    }

    public bool hasIngredients () {
        if (product.components == null || product.components.Count == 0) {
            return true;
        }
        // see if we have ingredients available.
        // I should remodle this so that stuff is slowly taken out of the manifest while it works
        // but EH.
        bool haveStuff = true;
        product.components.ForEach (delegate (Component ingredient) {
            if (this.cargo.ById (ingredient.id) == null) haveStuff = false;
            else if (this.cargo.ById (ingredient.id).amount < ingredient.amount) haveStuff = false;
        });
        return haveStuff;
    }

    void startBuild () {
        // Debug.Log(gameObject.name + ": Starting to build " + product.Name + " " + manufactureCycleSecs);
        this.timer = 0.0f;
        this.state = 1;
    }

    void finishBuild () {
        // Debug.Log(gameObject.name + ": Finished Building " + product.Name);
        // sanity check to make sure the thing we're making has components
        if (product.components == null || product.components.Count != 0) {
            // for each of it's components remove from the manifest
            product.components.ForEach (delegate (Component ingredient) {
                // Debug.Log(gameObject.name + ": Removing Resource " + ingredient.Amount + " " + ingredient.Item.Name);
                this.cargo.RemoveCargo (ingredient.id, ingredient.amount);
            });
        }
        // Debug.Log(gameObject.name + ": Add Product " + product.Name);
        // add that product! woo
        this.cargo.AddCargo (this.product, 1);
        this.state = 2;
    }

    void cycleComplete () {
        // Debug.Log(gameObject.name + ": Cooldown Complete ");
        this.timer = 0.0f;
        this.state = 0;
    }
}