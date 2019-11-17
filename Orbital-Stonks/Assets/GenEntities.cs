using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEntities : MonoBehaviour
{
    static int nbOfPlanets = 6;
    static int nbOfPlayers = 3;
    private GameObject[] planets = new GameObject[nbOfPlanets];
    private List<GameObject> players = new List<GameObject>();
    Transform t;
    
    public GameObject prefabPlanet;
    public GameObject prefabPlayer;

    // Start is called before the first frame update
    void Start()
    {

        CreatePlanetWithRandomParams();
        CreatePlayers();

    }
    void CreatePlayers()
    {
        if (nbOfPlayers < nbOfPlanets)
        {
            for (int i = 0; i < nbOfPlayers; i++)
            {
                players.Add(Instantiate(prefabPlayer as GameObject));
                players[i].transform.position = planets[i].transform.position + (planets[i].transform.localScale / 2);
            }
        }
    }
    void CreatePlanetWithRandomParams()
    {


        Vector2 diff = new Vector2(0, 0);

        for (int k = 0; k < nbOfPlanets; k++)
        {
            planets[k] = Instantiate(prefabPlanet) as GameObject;
        }

        planets[0].transform.position = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));


        for (int j = 1; j < nbOfPlanets; j++)
        {
            bool isCorrect = true;
            while (true)
            {
                isCorrect = true;
                planets[j].transform.position = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));
                float radius = Random.Range(2.0f, 4.0f);
                planets[j].transform.localScale = new Vector3(radius, radius, 1);
                for (int k = 0; k < j; k++)
                {
                    diff = planets[j].transform.position - planets[k].transform.position;
                    float ert = diff.magnitude;
                    float radiiSum = 2 * ((planets[j].transform.localScale.x / 2) + planets[k].transform.localScale.x / 2);
                    if (ert < radiiSum) { isCorrect = false; break; }
                }
                if (isCorrect) break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}





