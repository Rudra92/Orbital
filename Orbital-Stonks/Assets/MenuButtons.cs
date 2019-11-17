using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject MainPanel;
    public Slider slider;
    public Slider planetSlider;
    public Text playerNumber;
    public Text planetNumber;

    private int nbPlayers;
    private int nbPlanets;

    // Start is called before the first frame update
    void Start()
    {
        playerNumber.text = "2";
        planetNumber.text = "2";
        slider.onValueChanged.AddListener(delegate {
            nbPlayers = (int)slider.value;
            setPlayerNumber(); });
        planetSlider.onValueChanged.AddListener(delegate {
            nbPlanets = (int)planetSlider.value;
            setPlanetNumber();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlayerNumber()
    {
        playerNumber.text = slider.value.ToString();
    }

    public void setPlanetNumber()
    {
        planetNumber.text = planetSlider.value.ToString();
    }

    public int GetPlayersNum()
    {
        return nbPlayers;
    }

    public int GetPlanetNum()
    {
        return nbPlanets;
    }
 }
