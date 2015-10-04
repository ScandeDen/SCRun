using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Defaults
{
    /// <summary>
    /// Компонент для быстрых перемещений по сценам.
    /// </summary>
    public class SceneLoader : Defaults.AbstractBehavior
    {
        protected override void OnAwake() { }

        protected override void OnStart() { }
        /// <summary>
        /// Перемещает к следующей сцене.
        /// </summary>
        /// <param name="name"> Имя сцены. </param>
        public void MoveToScene(string name)
        {
            Application.LoadLevel(name);
        }
    }
}