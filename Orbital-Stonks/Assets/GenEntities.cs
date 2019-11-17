using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenEntities : MonoBehaviour
{
    static int nbOfPlanets = 6;
    private int nbOfPlayers;
    private GameObject[] planets = new GameObject[nbOfPlanets];
    private List<GameObject> players = new List<GameObject>();
    Transform t;
    
    public GameObject prefabPlanet;
    public GameObject prefabPlayer;
    private GameObject gameController;
    private Run controllerScript;

    public float width;
    public float height;
    private float scalar;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        scalar = 3;
        nbOfPlayers = (int)slider.value;

        CreatePlanetWithRandomParams();
        CreatePlayers();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        controllerScript = gameController.GetComponent<Run>();
        controllerScript.enabled = true;


    }
    void CreatePlayers()
    {
        float min = nbOfPlanets > nbOfPlayers ? nbOfPlayers : nbOfPlanets;
    
            for (int i = 0; i < min; i++)
            {
                players.Add(Instantiate(prefabPlayer as GameObject));
                players[i].transform.position = planets[i].transform.position + (planets[i].transform.localScale / 2);
            }
        }
    
    void CreatePlanetWithRandomParams()
    {


        Vector2 diff = new Vector2(0, 0);

        for (int k = 0; k < nbOfPlanets; k++)
        {
            planets[k] = Instantiate(prefabPlanet) as GameObject;
        }

        planets[0].transform.position = new Vector2(Random.Range(-width, width), Random.Range(-height, height));


        for (int j = 1; j < nbOfPlanets; j++)
        {
            bool isCorrect = true;
            while (true)
            {
                isCorrect = true;
                planets[j].transform.position = new Vector2(Random.Range(-width, width), Random.Range(-height, height));
                float radius = Random.Range(2.0f, 4.0f);
                planets[j].transform.localScale = new Vector3(radius, radius, 1);
                for (int k = 0; k < j; k++)
                {
                    diff = planets[j].transform.position - planets[k].transform.position;
                    float ert = diff.magnitude;
                    float radiiSum = scalar * ((planets[j].transform.localScale.x / 2) + planets[k].transform.localScale.x / 2);
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





