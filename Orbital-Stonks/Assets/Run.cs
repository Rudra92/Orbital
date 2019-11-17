using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject[] players;
    private int currPlayerIndex;
    private int activePlayers;

    private GameObject currPlayer;

    private PlayerBehaviour pb;
    private bool turnRunning;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        currPlayerIndex = 0;
        currPlayer = players[0];
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
        if(!turnRunning)
        {
            pb = currPlayer.GetComponent<PlayerBehaviour>();
            pb.StartTurn();
            turnRunning = true;
        } else
        {
            pb = currPlayer.GetComponent<PlayerBehaviour>();
            if(!pb.IsTurn())
            {
                turnRunning = false;
                // pass on to the next player while the next player is inactive
                do
                {
                    currPlayerIndex = (currPlayerIndex + 1) % players.Length;
                    currPlayer = players[currPlayerIndex];
                } while (!currPlayer.activeInHierarchy);
            }
        }

        
    }

    void StartTurn(GameObject player)
    {
        pb = player.GetComponent<PlayerBehaviour>();
        pb.StartTurn();

    }

}
