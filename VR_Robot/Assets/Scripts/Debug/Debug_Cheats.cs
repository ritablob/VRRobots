using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Debug_Cheats : MonoBehaviour
{
    public static Debug_Cheats instance;

    public TMP_InputField input;
    public TextMeshProUGUI text, autoCompleteText, autoCompleteGhostText;
    public GameObject inputField;

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

        #region Execute Command
        string[] args = input.text.Split(' ');
        if (args.Length != 0)
        {
            string cmd = args[0].ToLower();

            switch (cmd)
            {
                case "godmode":
                    if (args.Length > 1)
                    {
                        if (bool.TryParse(args[1], out bool enable))
                        {
                            break;
                        }
                        else { OutputLog($"Unknown command: {input.text}", Color.red); }
                    }
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
                default:
                    OutputLog($"Unknown command: {input.text}", Color.red);
                    break;
            }
        }
        #endregion

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
        node = new CommandNode("godmode");
        commandNodes[0].commandsFromCurrent.Add(node);
        node = new CommandNode("true");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
        node = new CommandNode("false");
        commandNodes[0].commandsFromCurrent[0].commandsFromCurrent.Add(node);
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