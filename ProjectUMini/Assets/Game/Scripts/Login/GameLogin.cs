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
            // UMEntity.Audio.Play();
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