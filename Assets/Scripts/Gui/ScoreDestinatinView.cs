﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Core.Player;
using Core.Objects.Properties;

namespace Gui
{
    /// <summary>
    /// Компонент, контроллирующий отображение прогресса игрока по очкам.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ScoreDestinatinView : Defaults.AbstractBehavior
    {
        /// <summary>
        /// Задний фон. Компонент будет менять его цвет в зависимости от состояния прогрессса.
        /// </summary>
        public Graphic Background;
        /// <summary>
        /// Обозреваемый компонентом игрок.
        /// </summary>
        public Player ObservedPlayer;
        /// <summary>
        /// Цвет до того, как количество очков меньше лучшего результата игрока.
        /// </summary>
        protected Color beforeBestScore = new Color(50, 50, 50, 100);
        /// <summary>
        /// Цвет после того, как количество очков больше лучшего результата игрока.
        /// </summary>
        protected Color afterBestScore = new Color(150, 230, 0, 100);

        protected override void OnAwake() 
        {
            if (Background != null)
                Background.color = beforeBestScore;
            CashComponent<Image>();
            var progress = GetCashedComponent<Image>();
            if (Background != null && !PlayerPrefs.HasKey(Player.BEST_SCORE_PREFS))
            {
                Background.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            if (progress != null)
                progress.fillAmount = 0;
            if (ObservedPlayer != null && ObservedPlayer.CharacterPoints != null)
                ObservedPlayer.CharacterPoints.ValueChanged += CharacterPointsValueChanged;
        }
        /// <summary>
        /// Обрабатывает изменение очков у игрока.
        /// </summary>
        /// <param name="sender"> Свойство, которое накапливает очки. </param>
        /// <param name="oldVal"> Старое значение переменной в свойстве. </param>
        protected void CharacterPointsValueChanged(Property sender, int oldVal)
        {
            if (!PlayerPrefs.HasKey(Player.BEST_SCORE_PREFS) || 
                !PlayerPrefs.HasKey(Player.LAST_SCORE_PREFS))
                return;
            var progress = GetCashedComponent<Image>();
            if (progress != null)
            {
                if (sender.Value <= PlayerPrefs.GetInt(Player.BEST_SCORE_PREFS))
                {
                    progress.fillAmount = (float)sender.Value /
                        PlayerPrefs.GetInt(Player.BEST_SCORE_PREFS);
                    if (Background != null && Background.color == afterBestScore)
                    {
                        Background.color = beforeBestScore;
                    }
                }
                else
                {
                    progress.fillAmount = 
                        (float)PlayerPrefs.GetInt(Player.BEST_SCORE_PREFS) /
                        sender.Value;
                    if (Background != null && Background.color == beforeBestScore)
                    {
                        Background.color = afterBestScore;
                    }
                }
            }
        }

        protected override void OnStart() { }

    }
}
