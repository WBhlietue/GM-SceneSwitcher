using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.PlayerData;
using UnityEngine.Events;

public class GMStoreManager : MonoBehaviour
{
    public BuyType type;
    public int price;
    public int priceStep;
    public string itemName;
    public GMPlayerData data;
    public UnityEvent buySuccess;
    public UnityEvent buyFail;
    private void Start()
    {
        SetPrice();
    }
    void SetPrice()
    {
        int p = data.intData[itemName + "-price"];
        if (p > 0)
        {
            price = p;
        }
    }
    public void Buy()
    {
        int money = data.intData["money"];
        if (money > price)
        {
            money -= price;
            data.intData["money"] = money;
            switch (type)
            {
                case BuyType.Unlock:
                    data.intData[itemName] = 1;
                    break;
                case BuyType.Upgrade:
                    data.intData[itemName]++;
                    break;
                case BuyType.Buy:
                    data.intData[itemName]++;
                    break;
            }
            data.intData[itemName + "-price"] += priceStep;
            SetPrice();
            if (buySuccess != null)
            {
                buySuccess.Invoke();
            }
        }
        else
        {
            if (buyFail != null)
            {
                buyFail.Invoke();
            }
        }
    }
    public enum BuyType
    {
        Unlock, Upgrade, Buy
    }
}
