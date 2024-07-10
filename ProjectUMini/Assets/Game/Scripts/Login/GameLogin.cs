using Game.Scripts.Const;
using Game.Scripts.UI.Debug;
using Game.Scripts.UI.Login;
using UMiniFramework.Scripts;
using UMiniFramework.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Login
{
    public class GameLogin : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            UMUtils.Debug.Log("LoginScene Open");
            if (GameGlobalConst.IS_DEBUG)
            {
                UMini.UI.Open<DebugPanel>();
            }
            UMini.UI.Open<LoginPanel>();
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKey(KeyCode.Space))
            // {
            //     UMEntity.Scene.Load("Launch");
            // }
        }
    }
}