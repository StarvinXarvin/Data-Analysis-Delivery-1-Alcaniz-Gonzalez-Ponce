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
        Simulator.OnNewPlayer += LogAge;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LogAge(string lname, string lcountry, int lage, float lgender, DateTime ldateTime)
    {
        Debug.Log(lname);
        Debug.Log(lcountry);
        Debug.Log(lage);
        Debug.Log(lgender);
        //Debug.Log("Date Time: ", ldateTime);
    }
}

//Sin modificar el script de Simulator, tenemos que conseguir con este script gatherear toda la data pertinente para poder enviarla a un hipotetico server

//NECESITAMOS SABER: 

// - General:          Número de usuarios al día, número de usuarios al mes
// - Sesiones:         Duración, número
// - Monetización:     ARPU, ARPPU
// - Retención:        Stickyness, D1, D3, D7
// - De cada usuario:  Edad, Género, País