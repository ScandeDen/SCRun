using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Gui
{
    /// <summary>
    /// Счетчик времени для его вывода в текстовых полях.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TimeCounter : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Прошедшее время.
        /// </summary>
        protected float elapsedTime;

        protected override void OnAwake()
        {
            CashComponent<Text>();
        }

        protected override void OnStart()
        {
            var text = GetCashedComponent<Text>();
            if (text != null)
                text.text = "";
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            var text = GetCashedComponent<Text>();
            if (text != null)
                text.text = new DateTime(TimeSpan.FromSeconds(elapsedTime).Ticks).
                    ToString("mm:ss");
        }
    }
}
