using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase Instance;
    public List<Item> items = new List<Item> ();
    private static readonly string itemsFile = "items.json";

    private void Awake () {
        if (Instance != null) {
            Destroy (gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad (gameObject);

        this.Load ();
    }

    public void Save() {
        string path = Path.Combine(Application.streamingAssetsPath, itemsFile);
        File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    public void Load () {
        string path = Path.Combine(Application.streamingAssetsPath, itemsFile);
        using (FileStream filestream = File.OpenRead(path)) {
            StreamReader reader = new StreamReader(filestream);
            string json = reader.ReadToEnd();
            ItemList data = JsonUtility.FromJson<ItemList> (json);
            this.items = data.items;
        }
    }

    public Item ById (int id) {
        return this.items.Find(Item => Item.id == id);
    }

    public Item ByName (string name) {
        return this.items.Find(Item => Item.name.ToLower () == name.ToLower ());
    }

}

[Serializable]
public class Item {
    public int id;
    public string name;
    public string code;
    public string description;
    public float value = 1.0f;
    public float mass = 1.0f;
    public float volume = 1.0f;
    public bool is_mineable = false;
    public bool is_product = false;
    public List<Component> components = new List<Component> ();

    public Item () { }

    public Item (int id, string name, string code, string description, float value = 1.0f, float mass = 1.0f, float volume = 1.0f, bool is_mineable = false, bool is_product = false) {
        this.id = id;
        this.name = name;
        this.name = code;
        this.value = value;
        this.mass = mass;
        this.volume = volume;
        this.is_mineable = is_mineable;
        this.is_product = is_product;
    }

}

[Serializable]
public class Component {
    public int id;
    public int amount;

    public Component (int itemID, int amount) {
        this.id = itemID;
        this.amount = amount;
    }
}

[Serializable]
public class ItemList {
    public List<Item> items = new List<Item> ();
}
