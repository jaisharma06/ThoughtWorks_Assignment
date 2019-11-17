using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerTurn; //1 if player 1's turn 2 if player 2's turn.
    public Bullet bullet; //Bullet to destroy the tanks.
    public static GameManager instance;
    public List<Manager> playerManagers;

    public Text turnText;
    public Text tanksLeftText;

    private void OnEnable()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ShowTurnText();
    }

    private void OnDisable()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    public void SwitchPlayerTurn(int turn)
    {
        playerTurn = turn;
        EventManager.TurnSwitched(turn);
        ShowTurnText();
    }

    /// <summary>
    /// Shows the turn text.
    /// </summary>
    private void ShowTurnText()
    {
        turnText.enabled = true;
        turnText.text = "PLAYER " + playerTurn + " TURN";
        tanksLeftText.text = "TANKS LEFT " + playerManagers[playerTurn - 1].TanksLeft() + "/5";
        Invoke("Hide", 2);
    }

    /// <summary>
    /// Hides the turn text.
    /// </summary>
    private void HideTurnText()
    {
        turnText.enabled = false;
    }

    /// <summary>
    /// Restarts the game on completion.
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Switches the players turn when the missile is fired.
    /// Checks if game is over or not and shows the result.
    /// </summary>
    public void ToggleTurn()
    {
        if (playerManagers[0].TanksLeft() <= 0)
        {
            turnText.enabled = true;
            turnText.text = "PLAYER " + 2 + " WON";
            Invoke("RestartGame", 2);
            return;
        }
        else if (playerManagers[1].TanksLeft() <= 0)
        {
            turnText.enabled = true;
            turnText.text = "PLAYER " + 1 + " WON";
            Invoke("RestartGame", 2);
            return;
        }
        else if(playerManagers[0].playerTurnLeft <= 0 && playerManagers[1].playerTurnLeft <= 0)
        {
            turnText.enabled = true;
            turnText.text = "MATCH TIE";
            Invoke("RestartGame", 2);
            return;
        }

        playerTurn = (playerTurn == 1) ? 2 : 1;
        EventManager.TurnSwitched(playerTurn);
        ShowTurnText();
    }
}
