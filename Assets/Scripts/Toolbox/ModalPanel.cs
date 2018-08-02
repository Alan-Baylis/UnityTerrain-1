using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalPanel : MonoBehaviour
{
    public enum ModalPanelType
    {
        Ok,   
        YesNo, 
        YesCancel, 
        Cancel,
        YesNoCancel
    }


    public GameObject ModalPanelObject;
    public Text Question;
    public Button YesButton;
    public Button NoButton;
    public Button CancelButton;

    private static ModalPanel _modalPanel;


    //Todo: https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-window

    void Awake()
    {
        _modalPanel = ModalPanel.Instance();

        ModalPanelObject.transform.GetChild(0).localPosition = Vector3.zero;
    }
    

    public static ModalPanel Instance()
    {
        if (!_modalPanel)
        {
            _modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!_modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }
        return _modalPanel;
    }

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void Choice(string question, ModalPanelType modalPanelType, UnityAction firstEvent=null)
    {
        ModalPanelObject.SetActive(true);

        this.Question.text = question;
        //Set a default actions
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(TestYesFunction);
        YesButton.onClick.AddListener(ClosePanel);
        YesButton.gameObject.SetActive(false);
        NoButton.onClick.RemoveAllListeners();
        NoButton.onClick.AddListener(TestNoFunction);
        NoButton.onClick.AddListener(ClosePanel);
        NoButton.gameObject.SetActive(false);
        CancelButton.onClick.RemoveAllListeners();
        CancelButton.onClick.AddListener(TestCancelFunction);
        CancelButton.onClick.AddListener(ClosePanel);
        CancelButton.gameObject.SetActive(false);

        switch (modalPanelType)
        {
            case ModalPanelType.YesNo:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                YesButton.onClick.RemoveAllListeners();
                YesButton.onClick.AddListener(firstEvent);
                YesButton.onClick.AddListener(ClosePanel);
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesCancel:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                YesButton.onClick.RemoveAllListeners();
                YesButton.onClick.AddListener(firstEvent);
                YesButton.onClick.AddListener(ClosePanel);
                YesButton.gameObject.SetActive(true);
                CancelButton.gameObject.SetActive(true);
                break;
            case ModalPanelType.Ok:
                YesButton.gameObject.SetActive(true);
                break;
            case ModalPanelType.Cancel:
                //  Announcement: A string and Cancel event;
                CancelButton.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesNoCancel:
                //  Yes/No/Cancel: A string, a Yes event, a No event and Cancel event;
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                CancelButton.gameObject.SetActive(true);
                break;

        }
    }



  

    void ClosePanel()
    {
        ModalPanelObject.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _modalPanel.Choice("Do you want to spawn this sphere?", ModalPanelType.YesNoCancel);
            Debug.Log("I'm after Dialog box");

            //For testing
            //UnityAction myYesAction = new UnityAction(TestYesFunction);
            //UnityAction myNoAction = new UnityAction(TestNoFunction);
            //UnityAction myCancelAction = new UnityAction(TestCancelFunction);
            //_modalPanel.Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
        }
    }


    //  These are wrapped into UnityActions
    void TestYesFunction()
    {
        Debug.Log("Yes Clicked!");
    }

    void TestNoFunction()
    {
        Debug.Log("No Clicked!");
    }

    void TestCancelFunction()
    {
        Debug.Log("Cancel Clicked!");
    }


}
