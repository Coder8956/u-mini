using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime
{
    /// <summary>
    /// 通用弹窗面板，支持三种状态：
    /// 1. TwoButton  — 标题 + 内容 + 左右两个按钮
    /// 2. OneButton  — 标题 + 内容 + 单个按钮
    /// 3. CountDown — 标题 + 内容 + 倒计时关闭 / 不可关闭
    /// </summary>
    [UMUIPanelCfg("UI/Debug/UMUICommonDebug")]
    public class UMUICommonDebug : UMUIPanelBase
    {
        protected override void OnInitialize()
        {
            
        }
    }
}