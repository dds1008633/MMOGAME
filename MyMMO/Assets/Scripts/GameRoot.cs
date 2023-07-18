using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update

    AsyncNet client;

    void Start()
    {        
        client = new AsyncNet();
        client.StartClient("192.168.31.143", 8888);
    }

    void Update()
    {
        if (client != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                client.session.ReqPBLogin();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                client.CloseClient();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.session != null)
        {
            if (client.session.sessionSate == AsyncSessionState.CONNECTED)
            {
                client.CloseClient();
            }
        }
    }
}
