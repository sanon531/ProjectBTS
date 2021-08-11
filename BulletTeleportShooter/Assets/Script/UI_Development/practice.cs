using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializableField]
pracDictionary Dictionary;


public class Item
{
    private string name;
    private int str;
    private int dex;

    public Item(string _name, int _str, int _dex)
    {
        this.name = _name;
        this.str = _str;
        this.dex = _dex;
    }

    public void Show()
    {
        Debug.Log(this.name);
        Debug.Log(this.str);
        Debug.Log(this.dex);
        Debug.Log("실행 중");
    }
}


public class practice : MonoBehaviour
{
    Dictionary<string, Item> itemMap;

    void Start()
    {
        itemMap = new Dictionary<string, Item>();

        string name;

        name = "김 준";
        itemMap.Add(name, new Item(name, 1, 2));

        

        name = "박찬민";
        itemMap.Add(name, new Item(name, 3, 4));

        name = "이중원";
        itemMap.Add(name, new Item(name, 5, 6));

        name = "권정훈";
        itemMap["권정훈"] = new Item(name, 7, 8);


        name = "강문혁";
        itemMap["강문혁"] = new Item(name, 9, 10);

        if (itemMap.ContainsKey("권정훈"))
        {
            Item item = itemMap["권정훈"];
            item.Show();
        }

    }
}
