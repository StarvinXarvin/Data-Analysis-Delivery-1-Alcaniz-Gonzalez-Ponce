using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using System.Globalization;

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
        form.AddField("ItemBoughtDate", time.ToString("yyyy-MM-dd HH:mm:ss"));
        StartCoroutine(Upload(form, CallbackEvents.OnItemBuyCallback, "https://citmalumnes.upc.es/~xavierac8/buy.php"));

    }

    private void LogSessionEnded(DateTime time, uint arg2)
    {
        //Debug.Log("Session ended at " + time);

        WWWForm form = new WWWForm();
        form.AddField("SessionID", arg2.ToString());
        form.AddField("SessionEndDate", time.ToString("yyyy-MM-dd HH:mm:ss"));
        StartCoroutine(Upload(form, CallbackEvents.OnEndSessionCallback, "https://citmalumnes.upc.es/~xavierac8/endsession.php"));

    }

    private void LogSessionStarted(DateTime time, uint arg2)
    {
        //Debug.Log("Session started at " + time);

        WWWForm form = new WWWForm();
        form.AddField("PlayerID", arg2.ToString());
        form.AddField("SessionStartDate", time.ToString("yyyy-MM-dd HH:mm:ss"));
        StartCoroutine(Upload(form, CallbackEvents.OnNewSessionCallback, "https://citmalumnes.upc.es/~xavierac8/startsession.php"));

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LogData(string lname, string lcountry, int lage, float lgender, DateTime ldateTime)
    {
        //Debug.Log("[Server] Player named " + lname + " Country: " + lcountry + " Age: " + lage + " Gender: " + lgender + " Date: " + ldateTime);

        WWWForm form = new WWWForm();
        form.AddField("PlayerName", StringNormalize(lname));
        form.AddField("PlayerCountry", lcountry);
        form.AddField("PlayerAge", lage);
        form.AddField("PlayerGender", lgender.ToString(CultureInfo.InvariantCulture));
        form.AddField("PlayerJoinDate", ldateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        StartCoroutine(Upload(form, CallbackEvents.OnAddPlayerCallback, "https://citmalumnes.upc.es/~xavierac8/player.php"));

        
        


    }

    public static string StringNormalize(string input)
    {
        string normalized = input.Trim();
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"[^a-zA-z0-9\s]", "");

        return normalized;
    }

   
    IEnumerator Upload(WWWForm form, Action<uint> callback, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form Upload Completed");
                Debug.Log(www.downloadHandler.text.ToString());
                uint id = uint.Parse(www.downloadHandler.text.ToString());
                callback.Invoke(id);
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