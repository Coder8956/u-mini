using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Common
{
    public partial class GameUI
    {
        public static Color MaskColor = new Color(0, 0, 0, 0.5f);

        public static void SetMaskColor(GameObject maskGo)
        {
            maskGo.GetComponent<Image>().color = MaskColor;
        }
    }
}