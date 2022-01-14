using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    MyInputAction inputActions;

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        continueBtn = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        quitBtn = transform.GetChild(1).GetChild(2).GetComponent<Button>();

        inputActions = new MyInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Menu.Enter.performed += Enter_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Menu.Enter.performed -= Enter_performed;
    }

    public void Enter_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        NewGame();
    }

    void NewGame()
    {
        Debug.Log("new game");
    }

    void ContinueGame()
    {

    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit game");
    }
}
