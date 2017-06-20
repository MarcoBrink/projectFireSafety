using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : MonoBehaviour
{
    /// <summary>
    /// The internal message value.
    /// </summary>
    private string _Message;

    /// <summary>
    /// The internal title value;
    /// </summary>
    private string _Title;

    /// <summary>
    /// The title of the popup.
    /// </summary>
    public string Title
    {
        get
        {
            return _Title;
        }
        set
        {
            _Title = value;

            //Alleen als het object actief is in de hiërarchie ook de tekst in de werkbalk aanpassen
            if (gameObject.activeInHierarchy)
            {
                transform.GetChild(0).Find("PopupTitleBar").Find("Title").GetComponent<Text>().text = _Title;
            }
        }
    }

    /// <summary>
    /// The message of the popup.
    /// </summary>
    public string Message
    {
        get
        {
            return _Message;
        }
        set
        {
            _Message = value;
            //Alleen als het object actief is in de hiërarchie ook de tekst in de werkbalk aanpassen
            if (gameObject.activeInHierarchy)
            {
                transform.GetChild(0).Find("PopupContent").Find("MessagePanel").Find("Message").GetComponent<Text>().text = _Message;
            }
        }
    }

    /// <summary>
    /// Constructor for a message popup.
    /// </summary>
    /// <param name="title">The title of the popup.</param>
    /// <param name="message">The message in the popup.</param>
    public static MessagePopup CreateMessagePopup(string title, string message)
    {
        //De popup zelf aanmaken
        MessagePopup popup = Instantiate(Resources.Load<MessagePopup>("Popups/Popup"));
        //vervolgens de titel en het bericht vastzetten
        popup.Title = title;
        popup.Message = message;
        //Popup teruggeven
        return popup;
    }

    /// <summary>
    /// The code that is executed when the "ok"-button is pressed. Removes the popup.
    /// </summary>
    public void OkayButtonPress()
    {
        //Message en Title verwijderen
        this.Message = null;
        this.Title = null;
        //En het object vernietigen
        Destroy(this.gameObject);
    }
}
