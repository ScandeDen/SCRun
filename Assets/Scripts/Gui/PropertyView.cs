using UnityEngine;
using System.Collections;
using Core.Objects.Properties;
using UnityEngine.UI;

namespace Gui.Views
{
    /// <summary>
    /// Отображение для любого компонента-свойства.
    /// </summary>
    public class PropertyView : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Модель данных, свойство.
        /// </summary>
        public Property Model;

        protected override void OnAwake()
        {
            CashComponent<Text>();
            if (Model != null)
                Model.ValueChanged += ModelValueChanged;
        }
        /// <summary>
        /// Обрабатывает изменение переменной в свойстве.
        /// </summary>
        /// <param name="sender"> Свойство. </param>
        /// <param name="oldVal"> Старое значение. </param>
        void ModelValueChanged(Property sender, int oldVal)
        {
            var text = GetCashedComponent<Text>();
            if (text != null)
                text.text = sender.Value.ToString();
        }

        protected override void OnStart()
        {
            var text = GetCashedComponent<Text>();
            if (text != null)
                text.text = "";
        }
    }
}