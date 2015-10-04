using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defaults
{
    /// <summary>
    /// Вспомогательный класс для хранения и запуска корутин, используя перечислитель.
    /// </summary>
    public class SafeCoroutine
    {
        /// <summary>
        /// Активирована ли корутина.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return method != null;
            }
        }
        /// <summary>
        /// Компонент, на котором запускаются корутины.
        /// </summary>
        protected AbstractBehavior behavior;
        /// <summary>
        /// Текущий запущенный метод в виде перечислителя.
        /// </summary>
        protected IEnumerator method;

        /// <summary>
        /// Конструктор с явным заданием компонента, на котором будут запускатья корутины.
        /// </summary>
        /// <param name="behavior"> Компонент. </param>
        public SafeCoroutine(AbstractBehavior behavior)
        {
            this.behavior = behavior;
        }
        /// <summary>
        /// Запускает корутину с заданным перечислителем.
        /// </summary>
        /// <param name="method"> Перечислитель. </param>
        public void Start(IEnumerator method)
        {
            if (this.method != null)
                Stop();
            this.method = method;
            behavior.StartCoroutine(this.method);
        }
        /// <summary>
        /// Останавливает текущую корутину.
        /// </summary>
        public void Stop()
        {
            if (method == null)
                return;
            behavior.StopCoroutine(method);
            method = null;
        }
    }
}
