﻿using UnityEngine;
using UnityEngine.UI;

namespace UMiniFramework.Runtime.Modules.UIModule
{
    public abstract class UMUIDialog : UMUIPanel
    {
        [SerializeField] private bool m_isMask = true;

        protected bool IsMask
        {
            get => m_isMask;
            set { m_mask.enabled = m_isMask = value; }
        }

        [SerializeField] private Sprite m_maskSprite = null;
        [SerializeField] private Color m_maskColor = new Color(0, 0, 0, 0.7f);
        private Image m_mask;

        protected Image Mask
        {
            get => m_mask;
        }

        public virtual void InitMask()
        {
            m_mask = GetComponent<Image>();
            if (m_mask == null)
            {
                m_mask = gameObject.AddComponent<Image>();
            }

            m_mask.sprite = m_maskSprite;
            m_mask.color = m_maskColor;
            m_mask.enabled = m_isMask;
        }
    }
}