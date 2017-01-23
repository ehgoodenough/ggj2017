﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {
    // Declare prefabs (for assigning in inspector)
    private GameObject Hero;
    private GameObject Thug;

    // Declare the entities in our scene
    private Thug thug;
    private Hero hero;
    public HealthBar healthbar;
    public Text endMessage;
    public Text tutorialMessage;
    public Text comboMessage;
    public const float restartTime = 3000;
    
    public bool gameHasEnded = false;
    public bool gameHasStarted = false;
    
    public bool hasCompletedTutorial = false;

    private string state;

    void Awake() {
        this.Hero = Resources.Load("MonkHero") as GameObject;
        this.Thug = Resources.Load("OniThug") as GameObject;

        if (GameObject.Find("HealthBar")) {
            this.healthbar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }
        
        if(GameObject.Find("Title") == null) {
            this.gameHasStarted = true;
            
            this.createHero();
            this.createThug(0);
        }
        
        tutorialMessage = GameObject.Find("TutorialMessage").GetComponent<Text>();

        comboMessage = GameObject.Find("ComboMessage").GetComponent<Text>();


        endMessage.text = "";

    }

    public Hero getHero() {
        return hero;
    }

    public Thug getThug() {
        return thug;
    }

    public void gameEnd(bool win)
    {
        if(this.gameHasEnded != true) {
            this.gameHasEnded = true;
            if (win)
            {
                endMessage.text = "You Win!";
            }
            else
            {
                endMessage.text = "You Lose!";
            }
            StartCoroutine(ExecuteAfterTime(3));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        Application.LoadLevel(Application.loadedLevel);
    }

	public Thug createThug(float stunPower) {
		this.thug = Object.Instantiate(Thug, new Vector2(6f + this.hero.transform.position.x, -0.5f), Quaternion.identity).GetComponent<Thug>();
        this.thug.gameObject.name = "Thug" + Random.Range(100, 999);
		this.thug.modifySpeed (stunPower);
        return this.thug;
    }

    public Hero createHero() {
        this.hero = Instantiate(Hero, new Vector2(-2f, -0.5f), Quaternion.identity).GetComponent<Hero>();
        this.hero.gameObject.name = "Hero";
        return this.hero;
    }

    public void destroyThug() {
        Object.Destroy(this.thug.gameObject);
    }

    void Update()
    {
        if(this.gameHasStarted == false) {
            if(Input.GetButtonDown("Action")) {
                this.gameHasStarted = true;
                
                this.createHero();
                this.createThug(0);
            }
        } else {
            Vector3 cameraMovement = new Vector3(0f, 0f, 0f);
            cameraMovement.x = (thug.transform.position.x - 1f - Camera.main.transform.position.x) / 16;
            Camera.main.transform.Translate(cameraMovement);
        }
    }
}
