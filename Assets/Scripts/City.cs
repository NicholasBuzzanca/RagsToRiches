using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, Location {

    public enum Resources {None = -1, Fish = 0, Meat = 1, Vegetable = 2, Wool = 3, Silk = 4, Dye = 5, Wood = 6, Stone = 7, Iron = 8}

    public Resources export;
    public string cityName;

    public int foodStock;
    public int luxuryStock;
    public int materialStock;

    float lastUpdateTime;
    int noUpdate;

	// Use this for initialization
	void Start () {
        foodStock = 5;
        luxuryStock = 5;
        materialStock = 5;

        noUpdate = (int)export / 3;
        lastUpdateTime = Time.time;
	}
	
    public bool AIBuy()
    {
        if (noUpdate == 0)
        {
            if (foodStock > 0)
            {
                foodStock--;
                return true;
            }
            return false;
        }
        if (noUpdate == 1)
        {
            if (luxuryStock > 0)
            {
                luxuryStock--;
                return true;
            }
            return false;
        }
        else
        {
            if (materialStock > 0)
            {
                materialStock--;
                return true;
            }
            return false;
        }
    }

    public void AISell(Resources resource)
    {
        int type = (int)resource / 3;
        if (type == 0)
        {
            foodStock++;
        }
        if (type == 1)
        {
            luxuryStock++;
        }
        else
        {
            materialStock++;
        }
    }

    public int CostToBuy()
    {
        if (noUpdate == 0)
        {
            return BuyCost(foodStock);
        }
        if (noUpdate == 1)
        {
            return BuyCost(luxuryStock);
        }
        else
        {
            return BuyCost(materialStock);
        }
    }

    static int BuyCost(int stock)
    {
        int cost = (int)(-Mathf.Pow(stock + 15f, 2f) / 25f + 60f);
        return Mathf.Max(cost, 1);
    }

    public bool BuyOne()
    {
        if (noUpdate == 0)
        {
            if (foodStock > 0)
            {
                foodStock--;
                return true;
            }
            return false;
        }
        if (noUpdate == 1)
        {
            if (luxuryStock > 0)
            {
                luxuryStock--;
                return true;
            }
            return false;
        }
        else
        {
            if (materialStock > 0)
            {
                materialStock--;
                return true;
            }
            return false;
        }
    }

    public int GetPrice(Resources toSell, float dist)
    {
        if (toSell == export)
            return 0;
        UpdateStock();
        if((int)toSell < 3)
        {
            //food
            return GetVal(foodStock, dist);
        }
        else if ((int)toSell < 6)
        {
            //luxury
            return GetVal(luxuryStock, dist);
        }
        else
        {
            //material
            return GetVal(materialStock, dist);
        }
    }

    public int Sell(Resources toSell, float dist)
    {
        if (toSell == export)
            return 0;
        UpdateStock();
        if ((int)toSell < 3)
        {
            //food
            foodStock++;
            return GetVal(foodStock, dist);
        }
        else if ((int)toSell < 6)
        {
            //luxury
            luxuryStock++;
            return GetVal(luxuryStock, dist);
        }
        else
        {
            //material
            materialStock++;
            return GetVal(materialStock, dist);
        }
    }

    public void UpdateStock()
    {
        int eightSec = (int)((Time.time - lastUpdateTime)/8f);
        if (eightSec == 0)
            return;
        if (noUpdate != 0)
        {
            if (foodStock > 9)
                eightSec *= 2;
            else if (foodStock > 19)
                eightSec *= 4;
            foodStock = Mathf.Max(foodStock - eightSec, 0);
        }
        else
            foodStock = Mathf.Min(foodStock + eightSec * 5, 20);
        if (noUpdate != 1)
        {
            if (luxuryStock > 9)
                eightSec *= 2;
            else if (luxuryStock > 19)
                eightSec *= 4;
            luxuryStock = Mathf.Max(luxuryStock - eightSec, 0);
        }
        else
            luxuryStock = Mathf.Min(luxuryStock + eightSec * 5, 20);
        if (noUpdate != 2)
        {
            if (materialStock > 9)
                eightSec *= 2;
            else if (materialStock > 19)
                eightSec *= 4;
            materialStock = Mathf.Max(materialStock - eightSec, 0);
        }
        else
            materialStock = Mathf.Min(materialStock + eightSec * 5, 20);

        lastUpdateTime = Time.time;
    }

    static int GetVal(int stock, float dist)
    {
        float distBonus = Mathf.Pow(dist,.55f)*.1f;
        return Mathf.Max((int)(BuyCost(stock) * distBonus),1);
        //return (int)Mathf.Max((Mathf.Pow(stock + 8,-1)) * 1000f - 27f*distBonus,1);
    }
}
