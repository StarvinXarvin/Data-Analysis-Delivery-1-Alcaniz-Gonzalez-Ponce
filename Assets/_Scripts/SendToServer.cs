using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void LogSessionEnded(DateTime time, uint arg2)
    {
        Debug.Log("Session ended");
        Debug.Log(time);
    }

    private void LogSessionStarted(DateTime time, uint arg2)
    {
        Debug.Log("Session started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LogData(string lname, string lcountry, int lage, float lgender, DateTime ldateTime)
    {
        Debug.Log(lname);
        Debug.Log(lcountry);
        Debug.Log(lage);
        Debug.Log(lgender);
        Debug.Log(ldateTime);

        CallbackEvents.OnAddPlayerCallback.Invoke(9);
    }
}

//Sin modificar el script de Simulator, tenemos que conseguir con este script gatherear toda la data pertinente para poder enviarla a un hipotetico server

//NECESITAMOS SABER: 

// - General:          N�mero de usuarios al d�a, n�mero de usuarios al mes
// - Sesiones:         Duraci�n, n�mero
// - Monetizaci�n:     ARPU, ARPPU
// - Retenci�n:        Stickyness, D1, D3, D7
// - De cada usuario:  Edad, G�nero, Pa�s