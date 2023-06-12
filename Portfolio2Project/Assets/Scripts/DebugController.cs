using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class DebugController : MonoBehaviour
{
    [SerializeField] Stats stats;

    public bool showConsole;
    public string input;
    [SerializeField] public int fontSize;

    public static DebugCommand GIVE_EXP;
    
    public List<object> commandList;


    private void Awake()
    {
        GIVE_EXP = new DebugCommand("give_exp~", "Gives 9999 EXP", "give_exp~", () => { stats.Exp += 9999; });
        
        commandList = new List<object>
        {
            GIVE_EXP,
        };
    }

    private void Update()
    {
        OnToggleDebug();
        OnReturn();
    }

    public void OnToggleDebug()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            showConsole = !showConsole;
        }
    }

    public void OnReturn()
    {
        if (input.EndsWith("~"))
        {

            HandleInput();
            input = "";
            showConsole = false;
        }
    }

    private void OnGUI()
    {
        if(!showConsole) { return; }

        float y = 0f;

        GUI.Box(new Rect(0f, y, Screen.width, 100), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);

        GUI.SetNextControlName("console"); //Gives a name to the text field
        
        GUI.skin.textField.fontSize = fontSize;
        
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width-20f, 100f), input);
        GUI.FocusControl("console"); //Lets you type in the text box when it is opened
    }

    
    public void HandleInput()
    {
        input = "give_exp~";
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                (commandList[i] as DebugCommand).InvokeCommand();
                if (commandList[i] as DebugCommand != null)
                {
                    Debug.Log("Code Hit");
                    (commandList[i] as DebugCommand).InvokeCommand();
                }
            }
        }
    }
}
