using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update

    AsyncNet net;

    void Start()
    {
        net = AsyncNet.CreateInstance();
        net.StartClient("192.168.31.143", 8888);
    }

    void Update()
    {
        if (net != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                net.session.ReqPBLogin();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                net.CloseClient();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (net != null && net.session != null)
        {
            if (net.session.sessionSate == AsyncSessionState.CONNECTED)
            {
                net.CloseClient();
            }
        }
    }
}
