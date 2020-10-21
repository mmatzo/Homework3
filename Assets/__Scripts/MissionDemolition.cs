using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S; //a private Singleton

    [Header("Set in Inspector")]
    public Text uitLevel; //The UIText_Level Text
    public Text uitShots; //The UIText_Shots Text
    public Text uitShotsHighScore;
    public Text uitButton; //The Text on UIButton_View
    public Vector3 castlePos; //The place to put castles
    public GameObject[] castles; //An array of the castles

    [Header("Set Dynamically")]
    public int level; //The current level
    public int levelMax; //The number of levels
    public int shotsTaken;
    public GameObject castle; //The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam mode
    private string currentLevel;
    private int lowestEver;
    void Start()
    {
        S = this; //Define the Singleton
        currentLevel = "0";
        lowestEver = -1;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        lowestEver = -1;
        if (PlayerPrefs.HasKey(currentLevel))
        {
            lowestEver = PlayerPrefs.GetInt(currentLevel, 0);
        }
        if (lowestEver <= 0)
        {
            print("i got here 54" + lowestEver);
            uitShotsHighScore.text = "No Current Highscore!";
        }
        else
        {
            print("i got here 54");
            uitShotsHighScore.text = "Highscore: " + lowestEver;
        }

    
        //Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        //Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //Zoom out
            SwitchView("Show Both");
            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        if (shotsTaken < lowestEver)
        {
            PlayerPrefs.SetInt(currentLevel, shotsTaken);
        }
        else if (lowestEver == -1)
        {
            PlayerPrefs.SetInt(currentLevel, shotsTaken);
        }
        level++;
        currentLevel = level.ToString();
        
        if (level == levelMax)
        {
            level = 0;
            currentLevel = level.ToString();
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    //Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
