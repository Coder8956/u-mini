using Game.Scripts.Const;
using Game.Scripts.UI.Login;
using Game.Scripts.UI.Postern;
using UMiniFramework.Scripts.UMEntrance;
using UnityEngine;

namespace Game.Scripts.Login
{
    public class GameLogin : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("LoginScene Open");
            if (GameGlobalConst.IS_DEBUG)
            {
                UMini.UI.Open<PosternPanel>();
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