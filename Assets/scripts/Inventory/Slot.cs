using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGameObject;
    public TMP_Text itemAmountText;

    void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemAmountText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGameObject.GetComponent<Image>().sprite = icon;

    }
}
