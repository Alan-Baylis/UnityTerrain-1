using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

    public GUISkin Skin;

    //Messaging board variables
    private string _alert;
    private List<string> _messageBoardQueue = new List<string>();
    private float _windowWidth = 300;
    private int _messageLength = 5;
    private Rect _alertBox;
    private bool _coroutineExecuting = false;

    //Todo: delete this TestPrint
    public int TestPrint;

    //Stats box variables
    private int _coin;
    private int _gem;
    private int _live;
    private int _level;
    private int _experience;
    private int _rank;
    private string _name;

    //TODO: change font for messages

    private static GUIManager _GUIManager;

    public static GUIManager Instance()
    {
        if (!_GUIManager)
        {
            _GUIManager = FindObjectOfType(typeof(GUIManager)) as GUIManager;
            if (!_GUIManager)
                Debug.LogError("There needs to be one active GUIManager script on a GameObject in your scene.");
        }
        return _GUIManager;
    }

    void Start()
    {
        AddMessage("BLU: Welcome To AUSTIN Map");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddMessage("GRE: Test Line message in Green " + TestPrint++);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RemoveMessage();
        }
        if (!_coroutineExecuting)
            if (_messageBoardQueue.Count != 0)
                StartCoroutine(RemoveMessage(2, _messageBoardQueue.Count));
    }

    void OnGUI()
    {
        if (_alert != "")
            GUI.Box(_alertBox, _alert, Skin.GetStyle("box"));

        //GUI.Box(_alertBox, _alert, Skin.GetStyle("box"));
    }

    public void AddMessage(string message)
    {
        if (_messageBoardQueue.Count == _messageLength)
            _messageBoardQueue.RemoveAt(0);
        _messageBoardQueue.Add(message);
        _alertBox = BuildAlertBox();
    }

    private void RemoveMessage()
    {
        if (_messageBoardQueue.Count == 0)
            return;
        _messageBoardQueue.RemoveAt(0);
        _alertBox = BuildAlertBox();
    }

    IEnumerator RemoveMessage(float time,int cnt)
    {
        _coroutineExecuting = true;
           yield return new WaitForSeconds(time);
        // RemoveMessage execute after the delay and if the size of queue haven't change
        if (cnt == _messageBoardQueue.Count)
            RemoveMessage();
        _coroutineExecuting = false;
    }
    private Rect BuildAlertBox()
    {
        _alert = BuildAlertString();
        float windowHeight = Skin.box.CalcHeight(new GUIContent(_alert), _windowWidth);
        float x = (Screen.width - _windowWidth) / 2;
        float y = 20;
        return new Rect(x, y, _windowWidth, windowHeight);
    }

    private string BuildAlertString()
    {
        string alert = "";
        foreach (var msg in _messageBoardQueue)
        {
            string color;
            switch (msg.Substring(0, 4))
            {
                case "YEL:":
                    color = "Yellow";
                    break;
                case "BLK:":
                    color = "Black";
                    break;
                case "BLU:":
                    color = "Blue";
                    break;
                case "RED:":
                    color = "Red";
                    break;
                case "GRE:":
                    color = "Green";
                    break;
                default:
                    color = "White";
                    break;
            }
            alert += "<color=" + color + ">" + msg.Substring(5) + "</color>\n";
        }
        return alert.Trim();
    }



    //todo look at Displaymanager at the buttom https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-window
}
