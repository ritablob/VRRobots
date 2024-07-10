using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class Debug_Cheats : MonoBehaviour
{
    public static Debug_Cheats instance;

    public TMP_InputField input;
    public TextMeshProUGUI text, autoCompleteText, autoCompleteGhostText;
    public GameObject inputField;
    public GameObject box, apple;
    public Robot_Interaction_State_Machine robot;

    private Coroutine currentFade;
    private CommandTree commands;
    private int autoCompleteIndex;

    private void Start()
    {
        commands = new CommandTree();
        commands.Initialize();
    }

    public void EnterDebugMenu(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        inputField.SetActive(!inputField.activeInHierarchy);

        if (inputField.activeInHierarchy)
        {
            input.ActivateInputField();
        }
        else
        {
            input.DeactivateInputField();
            input.text = "";
        }
    }

    public void EnterDebugCommand(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !inputField.activeInHierarchy) { return; }

        string[] args = input.text.Split(' ');
        EnterCommand(args);

        inputField.SetActive(!inputField.activeInHierarchy);
        input.text = "";
        input.DeactivateInputField();
    }

    public void AutoCompleteDebugText(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !inputField.activeInHierarchy) { return; }

        AutoComplete();
    }
    public void ChangeAutoCompleteText(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        autoCompleteIndex += Mathf.RoundToInt(ctx.ReadValue<float>());
    }
    public void ResetScene(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !inputField.activeInHierarchy) { SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single); }
    }

    private void Update()
    {
        if (inputField.activeInHierarchy)
        {
            // Update auto-complete suggestions
            UpdateAutoComplete();
        }
    }

    private void EnterCommand(string[] args) {
        if (args.Length != 0)
        {
            string cmd = args[0].ToLower();

            switch (cmd)
            {
                case "spawn":
                    if (args.Length > 4)
                    {
                        Vector3 spawnPos = ParsePosition(args[3], args[4], args[5]);

                        if (args[1] == "item") { 
                            if (args[2] == "box") { 
                                Instantiate(box, spawnPos, Quaternion.identity);
                                OutputLog($"Spawned Box at position: {spawnPos}", Color.green);
                                break;
                            }
                            if (args[2] == "apple") { 
                                Instantiate(apple, spawnPos, Quaternion.identity);
                                OutputLog($"Spawned Box at position: {spawnPos}", Color.green);
                                break;
                            }
                        }
                        else { OutputLog($"Unknown command: {input.text}", Color.red); }
                    }
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
                case "robot":
                    if (args.Length >= 2 && args[1] == "state") {
                        if (GetState(args[2]) == Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
                            OutputLog($"State: {args[2]} does not exist.", Color.red); 
                            break; 
                        }

                        Robot_Interaction_State_Machine.ERobotInteractionState state = GetState(args[2]);
                        FindObjectOfType<Robot_Interaction_State_Machine>().Context.DEBUG_SetState(GetState(args[2]));
                        OutputLog($"Robot State change to {state.ToString()}", Color.green);
                        break;
                    }
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
                case "task":
                    if (args.Length >= 3 && args[1] == "add") {
                        Director.instance.AddObjective(args[2], ObjectiveType.Checkbox);
                        OutputLog($"Added objective {args[2]}", Color.green);
                        break;
                    }
                    else if (args.Length >= 3 && args[1] == "complete") {
                        Director.instance.CompleteObjective(args[2]);
                        OutputLog($"Completed objective {args[2]}", Color.green);
                        break;
                    }
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
                default:
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
            }
        }
    }

    private void UpdateAutoComplete()
    {
        //Get current branch of the auto-complete tree
        string[] args = input.text.Split(' ');
        CommandNode currentNode = commands.commandNodes[0];

        //Use the current string to get the correct branch of the command tree
        for (int i = 0; i < args.Length; i++)
        {
            foreach (CommandNode node in currentNode.commandsFromCurrent)
            {
                if (node.nodeName == args[i]) { currentNode = node; }
            }
        }

        string inputText = args[args.Length - 1].ToLower();
        List<string> filteredStrings = new List<string>();
        List<string> _currentCommands = new List<string>();
        foreach (CommandNode cmd in currentNode.commandsFromCurrent) { _currentCommands.Add(cmd.nodeName); }

        foreach (string str in _currentCommands)
        {
            if (str.ToLower().StartsWith(inputText))
            {
                filteredStrings.Add(str);
            }
        }

        if (filteredStrings.Count == 0) { autoCompleteText.text = ""; autoCompleteGhostText.text = ""; return; }

        if (autoCompleteIndex < 0) { autoCompleteIndex = filteredStrings.Count - 1; }
        else if (autoCompleteIndex >= filteredStrings.Count) { autoCompleteIndex = 0; }

        if (filteredStrings.Count > autoCompleteIndex)
        {
            string ghostText = "";
            for (int i = 0; i < args.Length - 1; i++)
            {
                ghostText += args[i] + " ";
            }
            ghostText += filteredStrings[autoCompleteIndex];
            autoCompleteGhostText.text = ghostText;
            filteredStrings[autoCompleteIndex] = $"<#7FFF76>{filteredStrings[autoCompleteIndex]}</color>";
        }

        string autoCompleteMessage = string.Join("\n", filteredStrings);
        autoCompleteText.text = autoCompleteMessage;
    }
    private void AutoComplete()
    {
        string[] args = input.text.Split(' ');
        string currentWord = args[args.Length - 1];
        CommandNode currentNode = commands.commandNodes[0];

        //Use the current string to get the correct branch of the command tree
        for (int i = 0; i < args.Length; i++)
        {
            foreach (CommandNode node in currentNode.commandsFromCurrent)
            {
                if (node.nodeName == args[i]) { currentNode = node; }
            }
        }
        List<string> filteredStrings = new List<string>();
        List<string> _currentCommands = new List<string>();
        foreach (CommandNode cmd in currentNode.commandsFromCurrent) { _currentCommands.Add(cmd.nodeName); }

        foreach (string str in _currentCommands)
        {
            if (str.ToLower().StartsWith(currentWord.ToLower()))
            {
                filteredStrings.Add(str);
            }
        }

        if (filteredStrings.Count > 0)
        {
            string autoCompletedText = filteredStrings[autoCompleteIndex % filteredStrings.Count];

            if (autoCompletedText.Equals(currentWord, System.StringComparison.OrdinalIgnoreCase))
            {
                autoCompleteIndex++;
                autoCompletedText = filteredStrings[autoCompleteIndex % filteredStrings.Count];
            }

            args[args.Length - 1] = autoCompletedText;
            input.text = string.Join(" ", args);
        }

        input.MoveTextEnd(false);
    }
    private void OutputLog(string msg, Color color)
    {
        if (currentFade != null) { StopCoroutine(currentFade); }

        currentFade = StartCoroutine(ShowOutput(msg, color));
    }
    private Vector3 ParsePosition(string _x, string _y, string _z) {
        Vector3 pos = Vector3.zero;

        if (float.TryParse(_x, out float x)) { pos.x = x; }
        if (float.TryParse(_y, out float y)) { pos.y = y; }
        if (float.TryParse(_z, out float z)) { pos.z = z; }

        return pos;
    }
    private IEnumerator ShowOutput(string msg, Color color)
    {
        text.text = msg;
        text.color = color;
        yield return new WaitForSeconds(3);
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime / 3;
            float alpha = Mathf.Lerp(0, 1, timer);
            text.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
    private Robot_Interaction_State_Machine.ERobotInteractionState GetState(string stateName) {
        string[] enumNames = Enum.GetNames(typeof(Robot_Interaction_State_Machine.ERobotInteractionState));

        for (int i = 0; i < enumNames.Length; i++) {
            if (enumNames[i].ToLower() == stateName) {
                return (Robot_Interaction_State_Machine.ERobotInteractionState)Enum.Parse(typeof(Robot_Interaction_State_Machine.ERobotInteractionState), enumNames[i]);
            }
        }

        return Robot_Interaction_State_Machine.ERobotInteractionState.BAD;
    }
}

public class CommandTree
{
    public List<CommandNode> commandNodes = new List<CommandNode>(0);

    public CommandTree() { commandNodes = new List<CommandNode>(0); }

    public void Initialize()
    {
        //Initialize all godmode nodes
        CommandNode node = new CommandNode("BASE");
        commandNodes.Add(node);
        node = new CommandNode("spawn");
        commandNodes[0].commandsFromCurrent.Add(node);
        node = new CommandNode("item");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("apple");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("~");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("box");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("~");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[1].commandsFromCurrent.Add(node);
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[1].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent[1].commandsFromCurrent[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);


        node = new CommandNode("robot");
        commandNodes[0].commandsFromCurrent.Add(node);
        node = new CommandNode("state");
        commandNodes[0].commandsFromCurrent[1].commandsFromCurrent.Add(node);


        node = new CommandNode("task");
        commandNodes[0].commandsFromCurrent.Add(node);
        node = new CommandNode("add");
        commandNodes[0].commandsFromCurrent[2].commandsFromCurrent.Add(node);
        node = new CommandNode("organizeToys");
        commandNodes[0].commandsFromCurrent[2].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("complete");
        commandNodes[0].commandsFromCurrent[2].commandsFromCurrent.Add(node);
        node = new CommandNode("organizeToys");
        commandNodes[0].commandsFromCurrent[2].commandsFromCurrent[1].commandsFromCurrent.Add(node);
        node = new CommandNode("partialFill");
        commandNodes[0].commandsFromCurrent[2].commandsFromCurrent.Add(node);

        foreach (Enum val in Enum.GetValues(typeof(Robot_Interaction_State_Machine.ERobotInteractionState))) {
            node = new CommandNode(val.ToString().ToLower());
            commandNodes[0].commandsFromCurrent[1].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        }
    }
}

public class CommandNode
{
    public string nodeName;
    public List<CommandNode> commandsFromCurrent = new List<CommandNode>(0);

    public CommandNode(string _nodeName)
    {
        nodeName = _nodeName;
        commandsFromCurrent = new List<CommandNode>(0);
    }
}