using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Gui
{
    /// <summary>
    /// Перечисление типов возможных переменных.
    /// </summary>
    public enum ValueType
    {
        String,
        Int,
        Float
    }

    /// <summary>
    /// Отображение переменных, сохраненных между сессиями в PlayerPrefs.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class PrefsValueView : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Ключ, имя переменной.
        /// </summary>
        public string ValueKey;
        /// <summary>
        /// Тип переменной.
        /// </summary>
        public ValueType Type;

        protected override void OnAwake()
        {
            CashComponent<Text>();
        }

        protected override void OnStart()
        {
            var text = GetCashedComponent<Text>();
            if (text != null)
            {
                switch (Type)
                {
                    case ValueType.String:
                        text.text = PlayerPrefs.GetString(ValueKey);
                        break;
                    case ValueType.Int:
                        text.text = PlayerPrefs.GetInt(ValueKey).ToString();
                        break;
                    case ValueType.Float:
                        text.text = PlayerPrefs.GetFloat(ValueKey).ToString();
                        break;
                }
            }
        }
    }
}
