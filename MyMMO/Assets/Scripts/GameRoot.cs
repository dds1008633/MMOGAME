using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Tools;
public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update

    private AsyncNet netInstance;
    private float deltaTime = 0.0f;

    private void Awake()
    {        
        Application.targetFrameRate = 60;
    }

    void OnGUI()
    {
        int fps = (int)(1.0f / deltaTime);
        GUI.Label(new Rect(20, 20, 300, 60), "FPS: " + fps);
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        GameUpdater.CreateInstance();
        EventManager.CreateInstance();
        LogTool.Log("ÍøÂçÄ£¿éÆô¶¯...", ConsoleColor.Red);
        netInstance = AsyncNet.CreateInstance();
        netInstance.StartClient("192.168.31.143", 8888);      
    }

    void Update()
    {      
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (netInstance != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                netInstance.session.ReqPBLogin();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                netInstance.CloseClient();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (netInstance != null && netInstance.session != null)
        {
            if (netInstance.session.sessionSate == AsyncSessionState.CONNECTED)
            {
                netInstance.CloseClient();
            }
        }
    }
}
