using UnityEngine;

namespace UMiniFramework.Scripts.Kit.Pool
{
    public class GameObjectPool : MonoBehaviour
    {
        
        public void Init(GameObject original, uint initCount = 1)
        {
            if (initCount < 1)
            {
                UMDebug.Error("The number of initialized objects must be greater than one");
            }
            else
            {
                //TODO 继续处理对象池初始化
            }
        }
    }
}