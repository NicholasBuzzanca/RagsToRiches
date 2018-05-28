using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mansion : MonoBehaviour {

    public static Mansion singleton;

    public float startValue;
    float currValue;

    public float volatility;

    public CanvasGroup cg;
    public Text text;

	// Use this for initialization
	void Start () {
        singleton = this;
        text.color = Color.black;
        currValue = startValue;
        DisplayPrice();
        InvokeRepeating("UpdatePrice",1f,.75f);
    }
	
	// Update is called once per frame
	void UpdatePrice () {
        float rnd = Random.Range(0f,1f);
        float change_percent = 2f * volatility * rnd;
        if (change_percent > volatility)
            change_percent -= 1.95f*(volatility);
        float change_amount = currValue * change_percent;
        currValue = currValue + change_amount;
        DisplayPrice();
    }

    void DisplayPrice()
    {
        text.text = "$" + ((int)currValue);
    }

    public void BuyMansion()
    {
        int gold = Inventory.singleton.gold;
        if(gold >= currValue)
        {
            //end game
            EndGameManager.singleton.OpenEndGame();
        }
    }
}
