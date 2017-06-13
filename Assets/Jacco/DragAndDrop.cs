using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using System;

public class DragAndDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public GameObject triggerList;
    public GameObject settingList;
    public Texture2D cursorTexture;

    public InputField naamInput;
    public InputField tijdInput;
    public Button toepasButton;

    
    private List<GameObject> triggers;
    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        triggers = new List<GameObject>();

        toepasButton.onClick.AddListener(ToepasButton_OnClick);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.SetCursor(null, hotSpot, cursorMode);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Outside the UI.");

            CreateEmptyTrigger();
        }
    }

    private void CreateEmptyTrigger()
    {
        GameObject textGO = new GameObject("Jooooo");
        textGO.transform.SetParent(triggerList.transform, false);

        Text tt = textGO.AddComponent<Text>();
        tt.color = Color.black;
        tt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        tt.text = "Random text";

        EventTrigger et = textGO.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(TriggerClickHandler);
        entry.callback.AddListener(call);
        et.triggers.Add(entry);
    }

    private void TriggerClickHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = data as PointerEventData;
        //GameObject pressed = pointerEventData.pointerPress;

        if (pointerEventData.button == PointerEventData.InputButton.Left && pointerEventData.clickCount == 2 && !settingList.activeSelf)
            OnLeftDoubleClick();
    }

    private void OnLeftDoubleClick()
    {
        // Show settings in panel and make it visible
        ShowSettingMenu(true);

        Debug.Log("Open trigger menu.");
    }

    private void ShowSettingMenu(bool show)
    {
        settingList.SetActive(show);
    }

    private void ToepasButton_OnClick()
    {
        Debug.Log("buttonclick werkt");

        string naam = naamInput.text;
        string tijd = tijdInput.text;

        Text tt = GameObject.Find("Jooooo").GetComponent<Text>();
        tt.text = naam;

        double result;

        if (double.TryParse(tijd, out result))
        {
            TimeTrigger trigger = new TimeTrigger(result);
        }
    }

    private void AddToTriggerList(GameObject trigger)
    {
        triggers.Add(trigger);

        RefreshTriggerList();
    }

    private void RemoveFromTriggerList(string triggerName)
    {
        GameObject removeObj = GameObject.Find(triggerName);

        triggers.Remove(removeObj);
        GameObject.Destroy(removeObj);

        RefreshTriggerList();
    }

    private void RefreshTriggerList()
    {
        foreach(GameObject obj in triggers)
        {

        }
    }
}
