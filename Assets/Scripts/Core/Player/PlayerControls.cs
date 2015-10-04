using Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Player
{
    /// <summary>
    /// Компонент, отслеживающий нажатия мыши и интерпретирующий их как сигналы к действиям 
    /// персонажа.
    /// </summary>
    public class PlayerControls : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Контроллируемые движения.
        /// </summary>
        public Movement ControlledMovement;
        /// <summary>
        /// Основная камера, с которой происходит контроль за действиями
        /// </summary>
        public Camera MainCamera;
        /// <summary>
        /// Набор областей для игнорирования элементов интерфейса.
        /// </summary>
        public List<RectTransform> NotIgnoreUI = new List<RectTransform>();

        protected override void OnAwake()
        {
            CashComponent<Transform>();
        }

        protected override void OnStart() { }

        void OnMouseUp()
        {
            if (IsIgnorePointForUI(Input.mousePosition))
                return;
            if (Input.mousePosition.y < Screen.height / 2)
            {
                ControlledMovement.Jump();
                return;
            }
            ControlledMovement.ShiftTo(Input.mousePosition.x < Screen.width / 2 ?
                Side.Left : Side.Right);
        }
        /// <summary>
        /// Содержится ли точка в какой либо из игнорируемых областей для интерфейса.
        /// </summary>
        /// <param name="point"> Точка. </param>
        /// <returns> True, если содержится. </returns>
        public bool IsIgnorePointForUI(Vector3 point)
        {
            foreach (RectTransform rect in NotIgnoreUI)
            {
                if (!rect.gameObject.activeInHierarchy)
                    continue;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(point.x, point.y), 
                    MainCamera))
                    return true;
            }
            return false;
        }
    }
}
