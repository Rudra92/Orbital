using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject SettingsPanel;
    public Slider slider;
    public Text playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerNumber.text = "2";
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        slider.onValueChanged.AddListener(delegate { setPlayerNumber(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMainMenu()
    {
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void setPlayerNumber()
    {
        playerNumber.text = slider.value.ToString();
    }
}
