using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LastStand : MonoBehaviour
{
    public TriggerSpawner LastTrigger;//make it so the ui appears
    public GameObject Door;//lock the door
    public GameObject LastStandCanvas;
    public TextMeshProUGUI LastStandObjective;

    private string word = "Objective:Survive!";
    private float Timer;

    int i = 0;

    private void Start()
    {
        //LastTrigger=
        // Door
        LastStandCanvas.SetActive(false);
        LastStandObjective.text = "";
    }

    void Update()
    {

        if (LastTrigger == null) return ;
        if (LastTrigger.checkTrigg == true)
        {
            Door.GetComponent<Collider>().isTrigger = false; //remove the trigger so its diabled

            //Disable the last door
            Timer += Time.deltaTime;
            if (LastStandCanvas == null) return;
            LastStandCanvas.SetActive(true);//appear

            if (Timer < 0.2) return;
            // if (i < word.Length) return;
            if (i == word.Length)
            {
                Destroy(LastStandCanvas, 3.0f);
                return;
            }
            else
            {              
                LastStandObjective.text += word.Substring(i, 1);
                i++;
                Timer = 0;
            }
        }
    }
}
