using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject[] players;
    private int currPlayerIndex;
    private int activePlayers;

    private GameObject currPlayer;

    private PlayerBehaviour pb;
    private bool turnRunning;

    public Canvas WinningCanvas;
    private bool end;

    void Start()
    {
        end = false;
        players = GameObject.FindGameObjectsWithTag("Player");
        currPlayerIndex = 0;
        currPlayer = players[currPlayerIndex];
        turnRunning = false;
        activePlayers = players.Length;
        foreach (GameObject p in players)
        {
            pb = p.GetComponent<PlayerBehaviour>();
            print(pb.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            return;
        }

        print(currPlayerIndex);
        int count = 0;
        foreach(GameObject p in players)
        {
            if(!p.activeSelf)
            {
                count++;
            }
        }

        // pass on to the next player while the next player is inactive
        while (!currPlayer.activeSelf)
        {
            currPlayerIndex = (currPlayerIndex + 1) % players.Length;
            currPlayer = players[currPlayerIndex];
            print(currPlayerIndex);

        }

        if (count == players.Length - 1)
        {
            print("end");
            end = true;
            WinningCanvas.GetComponentInChildren<Text>().text = "Player " + (currPlayerIndex + 1) + " wins !";
            WinningCanvas.gameObject.SetActive(true);
        }

        if (!turnRunning) 
        {
            pb = currPlayer.GetComponent<PlayerBehaviour>();
            pb.StartTurn();
            turnRunning = true;
            SpriteRenderer[] sprites = currPlayer.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sp in sprites)
            {
                sp.enabled = true;
            }
        } else
        {
            pb = currPlayer.GetComponent<PlayerBehaviour>();
            if(!pb.IsTurn())
            {
                SpriteRenderer[] sprites = currPlayer.gameObject.GetComponentsInChildren<SpriteRenderer>();
                sprites[1].enabled = false;
                currPlayerIndex = (currPlayerIndex + 1) % players.Length;
                currPlayer = players[currPlayerIndex];
                turnRunning = false;
                
            }
        }

        
    }

    void StartTurn(GameObject player)
    {
        pb = player.GetComponent<PlayerBehaviour>();
        pb.StartTurn();
    }

    public void DisableTurning()
    {
        turnRunning = false;
    }

}
