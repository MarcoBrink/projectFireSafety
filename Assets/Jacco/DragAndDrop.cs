using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    //Variabelen aanmaken om triggers en instellingen bij te houden
    public GameObject triggerList;
    public GameObject settingList;

    //Om de cursor texture bij te houden
    public Texture2D cursorTexture;

    //Onderdelen van een formulier om een naam en een tijd mee te geven en alles toe te passen
    public InputField naamInput;
    public InputField tijdInput;
    public Button toepasButton;

    //Private list om de triggers bij te houden
    private List<GameObject> triggers;
    //een vector2 voor een hotspot en automatisch de cursor mode af laten handelen
    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// Het openen en starten van de DragAndDrop editor
    /// </summary>
    private void Start()
    {
        //initialiseer de private list voor de triggers
        triggers = new List<GameObject>();
        //En controleren of de Toepas knop wordt aangeklikt
        toepasButton.onClick.AddListener(ToepasButton_OnClick);
    }

    /// <summary>
    /// Als er iets gesleept wordt de cursor afstellen
    /// </summary>
    /// <param name="eventData">het Drag event</param>
    public void OnDrag(PointerEventData eventData)
    {
        //De cursor de juiste vorm en kleuren e.d. meegeven
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    /// <summary>
    /// Wat te doen als het slepen stopt
    /// </summary>
    /// <param name="eventData">het EndDrag event</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //Cursor weer terugzetten
        Cursor.SetCursor(null, hotSpot, cursorMode);
        
        //Controleren of de cursor nog binnen de DragAndDrop omgeving staat
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Lege trigger aanmaken om het af te handelen
            CreateEmptyTrigger();
        }
    }

    /// <summary>
    /// Een lege trigger aanmaken om bepaalde acties af te handelen
    /// </summary>
    private void CreateEmptyTrigger()
    {
        //Gameobject aanmaken om tekst te laten zien
        GameObject textGO = new GameObject("Jooooo");
        textGO.transform.SetParent(triggerList.transform, false);

        //Vervolgens als tekst verwerken
        Text tt = textGO.AddComponent<Text>();
        tt.color = Color.black;
        tt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        tt.text = "Random text";

        //En aan de gebruiker presenteren om weg te klikken
        EventTrigger et = textGO.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(TriggerClickHandler);
        entry.callback.AddListener(call);
        et.triggers.Add(entry);
    }

    /// <summary>
    /// Klikken bij een trigger afhandelen
    /// </summary>
    /// <param name="data">Eventdata van de onClick event</param>
    private void TriggerClickHandler(BaseEventData data)
    {
        //Specifieke data over de pointer isoleren
        PointerEventData pointerEventData = data as PointerEventData;
        //GameObject pressed = pointerEventData.pointerPress;
        
        //Als de settingslist niet actief is en er dubbel geklikt wordt, afhandelen
        if (pointerEventData.button == PointerEventData.InputButton.Left && pointerEventData.clickCount == 2 && !settingList.activeSelf)
            OnLeftDoubleClick();
    }

    /// <summary>
    /// Afhandelen van dubbelklikken
    /// </summary>
    private void OnLeftDoubleClick()
    {
        // Settingspanel laten zien
        ShowSettingMenu(true);
    }

    /// <summary>
    /// Settingsmenu openen of sluiten
    /// </summary>
    /// <param name="show"></param>
    private void ShowSettingMenu(bool show)
    {
        //SetActive op True betekent laten zijn, op False betekent verbergen.
        settingList.SetActive(show);
    }

    /// <summary>
    /// Klikken op de toepasbutton verwerken
    /// </summary>
    private void ToepasButton_OnClick()
    {
        //Naam en Tijd vastzetten
        string naam = naamInput.text;
        string tijd = tijdInput.text;
        
        //De empty trigger terugzoeken
        Text tt = GameObject.Find("Jooooo").GetComponent<Text>();
        //En de naam neerzetten als de Triggertext
        tt.text = naam;

        //Double aanmaken...
        double result;
        //Om te kijken of de tijd goed is ingevuld
        if (double.TryParse(tijd, out result))
        {
            //Nieuwe trigger aanmaken om de tijd door te voeren
            TimeTrigger trigger = new TimeTrigger(result);
        }
    }

    /// <summary>
    /// Functie om Triggers aan de public lijst met Triggers toe te voegen
    /// </summary>
    /// <param name="trigger">Trigger om toe te voegen</param>
    private void AddToTriggerList(GameObject trigger)
    {
        //Trigger toevoegen
        triggers.Add(trigger);
        //Lijst verversen
        RefreshTriggerList();
    }

    /// <summary>
    /// Functie om een trigger uit de lijst te halen
    /// </summary>
    /// <param name="triggerName">Naam van de trigger om te verwijderen</param>
    private void RemoveFromTriggerList(string triggerName)
    {
        //Trigger zoeken en verwijzing maken
        GameObject removeObj = GameObject.Find(triggerName);
        //Trigger verwijderen en vernietigen, zodat deze geen geheugen blijft gebruiken
        triggers.Remove(removeObj);
        GameObject.Destroy(removeObj);
        //Lijst verversen
        RefreshTriggerList();
    }

    /// <summary>
    /// Functie om de Trigger list te verversen
    /// </summary>
    private void RefreshTriggerList()
    {
        //De hele lijst doorlopen, eigenlijk alleen om alles opnieuw te indexeren
        foreach(GameObject obj in triggers)
        {

        }
    }
}
