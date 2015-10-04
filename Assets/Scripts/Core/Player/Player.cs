using Core.Objects.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Player
{
    /// <summary>
    /// Компонент, обрабатывающий события игрока.
    /// </summary>
    public class Player : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Сцена, к которой совершается переход при уничтожении персонажа.
        /// </summary>
        public const string ON_DEAD_NEXT_SCENE = "GameResults";
        /// <summary>
        /// Переменная в PlayerPrefs, хранящая последние набранные очки.
        /// </summary>
        public const string LAST_SCORE_PREFS = "lastScore";
        /// <summary>
        /// Переменная в PlayerPrefs, хранящая лучший результат.
        /// </summary>
        public const string BEST_SCORE_PREFS = "bestScore";
        /// <summary>
        /// Компонент жизни отслеживаемого персонажа.
        /// </summary>
        public DamagableObject CharacterLife;
        /// <summary>
        /// Компонент очков отслеживаемого персонажа.
        /// </summary>
        public PointsCounter CharacterPoints;

        protected override void OnAwake() 
        {
            if (CharacterLife != null)
                CharacterLife.ValueChanged += CharacterLifeValueChanged;
            if (CharacterPoints != null)
                CharacterPoints.ValueChanged += CharacterPointsValueChanged;
        }

        protected override void OnStart() 
        {
            
        }
        /// <summary>
        /// Обрабатывает изменение очков отслеживаемого персонажа.
        /// </summary>
        /// <param name="sender"> Свойство-источник. </param>
        /// <param name="oldVal"> Старое значение. </param>
        void CharacterPointsValueChanged(Property sender, int oldVal)
        {
            PlayerPrefs.SetInt(LAST_SCORE_PREFS, sender.Value);
            PlayerPrefs.Save();
        }
        /// <summary>
        /// Обрабатывает изменение уровня жизни отслеживаемого персонажа.
        /// </summary>
        /// <param name="sender"> Свойство-источник. </param>
        /// <param name="oldVal"> Старое значение. </param>
        void CharacterLifeValueChanged(Property sender, int oldVal)
        {
            if (sender.Value <= 0)
            {
                if (CharacterPoints != null)
                {
                    if (PlayerPrefs.GetInt(LAST_SCORE_PREFS) >
                        PlayerPrefs.GetInt(BEST_SCORE_PREFS))
                    {
                        PlayerPrefs.SetInt(BEST_SCORE_PREFS, CharacterPoints.Value);
                        PlayerPrefs.Save();
                    }
                }
                Application.LoadLevel(ON_DEAD_NEXT_SCENE);
            }
        }

        void OnDestroy()
        {
            if (CharacterLife != null)
                CharacterLife.ValueChanged -= CharacterLifeValueChanged;
            if (CharacterPoints != null)
                CharacterPoints.ValueChanged -= CharacterPointsValueChanged;
        }
    }
}
