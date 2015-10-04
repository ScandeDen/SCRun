using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects.Zones
{
    /// <summary>
    /// Зона, включающая возможность особых действий для компонента передвижеий.
    /// </summary>
    public class FootingDetection : AbstractReversedZone<FootingState>
    {

    }

    /// <summary>
    /// Состояние, в котором включены особые действия у компонента движения. 
    /// </summary>
    [Serializable]
    public class FootingState : AbstractState
    {
        /// <summary>
        /// Применяет эффекты состояния на объект.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public override void ApplyState(StatableObject target)
        {
            var movement = target.GetComponent<Movement>();
            movement.IsCanDoAction = true;
        }
        /// <summary>
        /// Снимает эффекты состояния с объекта.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public override void ResetState(StatableObject target)
        {
            var movement = target.GetComponent<Movement>();
            movement.IsCanDoAction = false;
        }
    }
}
