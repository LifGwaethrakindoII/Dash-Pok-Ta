using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour {


//Var CountDownTimer------------------------------
     public float timeLeft = 180.0f;
     private Text textCountDownTimer;
     public GameObject textCountDownTimerObj;

//Var Score
	 private Text textScore;
     private int currentScore= 0;
     public GameObject textScoreObj;


//Var Score
     private Text textScorePlayer2;
     private int currentScorePlayer2= 0;
     public GameObject textScoreObj2;


     
     

     void Start ()
     {

     	 textCountDownTimer = textCountDownTimerObj.GetComponent<Text>();
         textScore = textScoreObj.GetComponent<Text>(); 
         textScore.text="ScoreTeamRed: " + currentScore;
         textScorePlayer2 = textScoreObj2.GetComponent<Text>(); 
         textScorePlayer2.text="ScoreTeamBlue: " + currentScorePlayer2;

         textCountDownTimer.text="Time Left: " + timeLeft;

     }

    void Update()
     {
        timeLeft -= Time.deltaTime;
        textCountDownTimer.text = "TimeLeft: " + timeLeft;
            if(timeLeft <= 0)
            {
                SceneManager.LoadScene("ResumeGame"); 
            }
    }

     public void setScore(int score)
     {
        currentScore+=1;
        textScore.text="ScoreTeamRed: " + currentScore; 
     }

        public void setScorePlayer2(int score)
     {
        currentScorePlayer2+=1;
        textScorePlayer2.text="ScoreTeamBlue: " + currentScorePlayer2; 
     }




}



