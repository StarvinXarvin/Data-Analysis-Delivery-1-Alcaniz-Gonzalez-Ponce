using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class SendToServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        Simulator.OnNewPlayer += LogData;
        Simulator.OnNewSession += LogSessionStarted;
        Simulator.OnEndSession += LogSessionEnded;
        Simulator.OnBuyItem += LogBuyItem;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= LogData;
        Simulator.OnNewSession -= LogSessionStarted;
        Simulator.OnEndSession -= LogSessionEnded;
        Simulator.OnBuyItem -= LogBuyItem;
    }

    private void LogBuyItem(int arg1, DateTime time, uint arg3)
    {
        //Debug.Log("Item " + arg1 + " Bought: " + arg3);

        WWWForm form = new WWWForm();
        form.AddField("ItemID", arg1.ToString());
        form.AddField("SessionID", arg3.ToString());
        form.AddField("ItemBoughtDate", time.ToString());
        Upload(form);

        CallbackEvents.OnItemBuyCallback.Invoke(1);
    }

    private void LogSessionEnded(DateTime time, uint arg2)
    {
        //Debug.Log("Session ended at " + time);

        WWWForm form = new WWWForm();
        form.AddField("SessionID", arg2.ToString());
        form.AddField("SessionEndDate", time.ToString());
        Upload(form);

        CallbackEvents.OnEndSessionCallback.Invoke(1);
    }

    private void LogSessionStarted(DateTime time, uint arg2)
    {
        //Debug.Log("Session started at " + time);

        WWWForm form = new WWWForm();
        form.AddField("SessionID", arg2.ToString());
        form.AddField("SessionStartDate", time.ToString());
        Upload(form);

        CallbackEvents.OnNewSessionCallback.Invoke(1);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LogData(string lname, string lcountry, int lage, float lgender, DateTime ldateTime)
    {
        //Debug.Log("[Server] Player named " + lname + " Country: " + lcountry + " Age: " + lage + " Gender: " + lgender + " Date: " + ldateTime);

        WWWForm form = new WWWForm();
        form.AddField("PlayerName", lname);
        form.AddField("PlayerCountry", lcountry);
        form.AddField("PlayerAge", lage);
        form.AddField("PlayerGender", lgender.ToString());
        form.AddField("PlayerJoinDate", ldateTime.ToString());
        StartCoroutine(Upload(form));


       
    }

   
    IEnumerator Upload(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~xavierac8/new1.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form Upload Completed");
                Debug.Log(www.downloadHandler.text);
                CallbackEvents.OnAddPlayerCallback.Invoke(1);
            }
        }


    }

}



//Sin modificar el script de Simulator, tenemos que conseguir con este script gatherear toda la data pertinente para poder enviarla a un hipotetico server

//NECESITAMOS SABER: 

// - General:          Número de usuarios al día, número de usuarios al mes
// - Sesiones:         Duración, número
// - Monetización:     ARPU, ARPPU
// - Retención:        Stickyness, D1, D3, D7
// - De cada usuario:  Edad, Género, País