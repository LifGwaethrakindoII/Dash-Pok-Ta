     using UnityEngine;
     using UnityEngine.UI;
     
     public class Timer : MonoBehaviour
     {
         private int Minutes;
         private int Seconds;
     
         public GameObject timeText;
         private Text m_text;
         private float m_leftTime;

         private GameMechanics gm;
     
        void OnEnable()
         {
            gm = this.GetComponent<GameMechanics>();
            Minutes = gm.getMinutes();
            Seconds = gm.getSeconds();

            m_text = timeText.GetComponent<Text>();
            m_leftTime = GetInitialTime();

            m_text.text = PlayerPrefs.GetInt("Minutes").ToString() + ":" + Seconds.ToString("00");
         }

         public void initScript()
         {
            gm = this.GetComponent<GameMechanics>();
            Minutes = gm.getMinutes();
            Seconds = gm.getSeconds();

            m_text = timeText.GetComponent<Text>();
            m_leftTime = GetInitialTime();
         }
     
         public void tick()
         {
            //Debug.Log("Greetings asshole...");

             if(m_leftTime > 0f)
             {
                 //  Update countdown clock
                 m_leftTime -= Time.deltaTime;
                 Minutes = GetLeftMinutes();
                 Seconds = GetLeftSeconds();
     
                 //  Show current clock
                 if (m_leftTime > 0f)
                 {
                    m_text.text = Minutes + ":" + Seconds.ToString("00");
                    //Debug.Log(Minutes + ":" + Seconds.ToString("00"));
                 }
                 else
                 {
                     //  The countdown clock has finished
                    m_text.text = "Time : 0:00";
                 }
             }

             if(m_leftTime <= 0)
             {
                gm.timeUp();
             }
         }

         public void setInitialTimeValues()
         {
            Minutes = gm.getMinutes();
            Seconds = gm.getSeconds();

            m_leftTime = GetInitialTime();
         }
     
         private float GetInitialTime()
         {
             return Minutes * 60f + Seconds;
         }
     
         private int GetLeftMinutes()
         {
             return Mathf.FloorToInt(m_leftTime / 60f);
         }
     
         private int GetLeftSeconds()
         {
             return Mathf.FloorToInt(m_leftTime % 60f);
         }
     }