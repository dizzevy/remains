using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject uiPanel;
    public Transform inventoryPanel;
    public List<Slot> slots = new List<Slot>();
    public bool isInvOpen;
    public playerMovement playerMove;

    void Awake()
    {
    }
    void Start()
    {

        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            slots.Add(inventoryPanel.GetChild(i).GetComponent<Slot>());
        }

        uiPanel.SetActive(false);
        isInvOpen = false;

        
    }

    void Update()
    {
        InventoryOpener();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("I pressed, isInvOpen = " + isInvOpen);
        }
    }


    void InventoryOpener()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isInvOpen)
            {
                uiPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerMove.canLook = true;
            }
            else
            {
                uiPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerMove.canLook = false;
            }
            isInvOpen = !isInvOpen;
        }
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item == _item && (slot.amount + _amount) <= _item.MaxAmount)
            {
                slot.amount += _amount;
                slot.itemAmountText.text = slot.amount.ToString();
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.Icon);
                slot.itemAmountText.text = _amount.ToString();
                return;
            }
        }
    }
}
