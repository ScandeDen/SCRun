using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gui.Editor
{
    /// <summary>
    /// Аттрибут ключа пула в менеджере пулов.
    /// </summary>
    public class PoolKeyAttribute : PropertyAttribute
    {
        public readonly string helpMessage;
        public PoolKeyAttribute(string helpMessage)
        {
            this.helpMessage = helpMessage;
        }
    }
}
