using Core.Objects.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects.Bonuses
{
    public class PointsBonus : AbstractBonus
    {
        [Header("Points")]
        public int GivenPoints;

        protected override void BonusApply(Collider target)
        {
            var counter = target.GetComponent<PointsCounter>();
            if (counter != null)
                counter.Value += GivenPoints;
        }
    }
}
