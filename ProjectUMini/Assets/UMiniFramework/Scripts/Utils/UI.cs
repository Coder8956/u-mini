using UnityEngine;

namespace UMiniFramework.Scripts.Utils
{
    public partial class UMUtils
    {
        public class UI
        {
            /// <summary>
            /// 等比缩放填充目标矩形
            /// </summary>
            /// <param name="source">源矩形</param>
            /// <param name="target">目标矩形</param>
            /// <returns></returns>
            public static Vector2 UniformScaleFillToTarget(Vector2 source, Vector2 target)
            {
                Vector2 res = new Vector2();
                // 计算宽高比 x是宽，y是高。
                float sRatio = source.x / source.y;
                float tRatio = target.x / target.y;

                if (sRatio > tRatio)
                {
                    // source 的宽高比 大于 target 的宽高比
                    res.x = target.x;
                    res.y = res.x / sRatio;
                }
                else if (sRatio < tRatio)
                {
                    // source 的宽高比 小于 target 的宽高比
                    res.y = res.y;
                    res.x = res.y * sRatio;
                }
                else
                {
                    // source 的宽高比 等于 target 的宽高比
                    res.x = target.x;
                    res.y = target.y;
                }

                return res;
            }

            public static void FillParent(RectTransform rt)
            {
                rt.anchorMax = Vector2.one;
                rt.anchorMin = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
        }
    }
}