using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Defaults;

namespace Core.Objects
{
    /// <summary>
    /// Сторона движения.
    /// </summary>
    public enum Side
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Компонент движения. Объекты с атким компонентом движутся по выбранному направлению 
    /// и имеют некоторые дополнительные функции, которые можно отключить и включить отдельно. 
    /// </summary>
    public class Movement : AbstractObject
    {
        /// <summary>
        /// Событие изменения скорости движения.
        /// </summary>
        public event MovingSpeedChangedHandler SpeedChanged;
        /// <summary>
        /// Событие остановки движения.
        /// </summary>
        public event MovingHandler MovingStoped;
        /// <summary>
        /// Событие начала движения.
        /// </summary>
        public event MovingHandler MovingStarted;
        /// <summary>
        /// Событие отключения специфичных действий во время движения.
        /// </summary>
        public event MovingHandler ActionsDisabled;
        /// <summary>
        /// Событие включения специфичных действий во время видов движения.
        /// </summary>
        public event MovingHandler ActionsEnabled;

        [Tooltip("Speed in m/s"), SerializeField]
        protected Vector3 speedVector;
        /// <summary>
        /// Скорость движения, описывающая и направление в целом.
        /// </summary>
        public Vector3 SpeedVector
        {
            get
            {
                return speedVector;
            }
            set
            {
                var temp = speedVector;
                speedVector = value;
                speedVectorMagnitude = speedVector.magnitude;
                var transformIns = GetComponent<Transform>();
                transformIns.LookAt(transformIns.position + speedVector);
                if (SpeedChanged != null)
                    SpeedChanged.Invoke(temp);
            }
        }
        /// <summary>
        /// Дистанция особого действия "Сдвиг" относительно направления движения.
        /// </summary>
        public float ShiftDistance = 1f;
        /// <summary>
        /// Время, на которое выключаются особые действия после сдвига.
        /// </summary>
        public float ShiftDelay = 0.5f;
        /// <summary>
        /// Сила прыжка.
        /// </summary>
        public float JumpPower = 10f;
        
        [SerializeField]
        protected bool isOn = true;
        /// <summary>
        /// Включено ли движение.
        /// </summary>
        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                isOn = value;
                if (value)
                {
                    if (MovingStarted != null)
                        MovingStarted.Invoke();
                }
                else
                {
                    if (MovingStoped != null)
                        MovingStoped.Invoke();
                }
            }
        }
        
        [SerializeField]
        protected bool isCanDoAction;
        /// <summary>
        /// Включены ли особые действия во время движения.
        /// </summary>
        public bool IsCanDoAction
        {
            get
            {
                return isCanDoAction;
            }
            set
            {
                isCanDoAction = value;
                if (value)
                {
                    if (ActionsEnabled != null)
                        ActionsEnabled.Invoke();
                }
                else
                {
                    if (ActionsDisabled != null)
                        ActionsDisabled.Invoke();
                }
            }
        }
        
        [SerializeField]
        protected Side currentSide;
        /// <summary>
        /// Текущая сторона движения.
        /// </summary>
        public Side CurrentSide
        {
            get
            {
                return currentSide;
            }
        }
        /// <summary>
        /// Магнитуда вектора скорости, закэшированное значение.
        /// </summary>
        protected float speedVectorMagnitude;
        /// <summary>
        /// Корутина, которая приостанавливает использование особых действий во время 
        /// движения.
        /// </summary>
        protected SafeCoroutine actionDelayCoroutine;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            CashComponent<Rigidbody>();
            speedVectorMagnitude = speedVector.magnitude;
            actionDelayCoroutine = new SafeCoroutine(this);
        }

        public void FixedUpdate()
        {
            OnFixedUpdate(Time.fixedDeltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (!IsOn)
            {
                return;
            }
            var transformIns = GetCashedComponent<Transform>();
            var p = transformIns.position;
            var shiftedP = p + SpeedVector;
            transformIns.position = Vector3.MoveTowards(p, shiftedP,
                speedVectorMagnitude * fixedDeltaTime);
        }
        /// <summary>
        /// Возвращает энумератор для корутины приостановки использования особых действий 
        /// во время движения.
        /// </summary>
        /// <returns> Энумератор. </returns>
        protected IEnumerator ShiftDelayCoroutine()
        {
            var temp = IsCanDoAction;
            IsCanDoAction = false;
            yield return new WaitForSeconds(ShiftDelay);
            IsCanDoAction = temp;
        }
        /// <summary>
        /// Особое действие во время движения. Сдвигает объект дискретно на другую сторону 
        /// движения. Значение Side.Center не вызывает эффекта.
        /// </summary>
        /// <param name="direction"> Сторона движения относительно текущей(влево или вправо). </param>
        public void ShiftTo(Side direction)
        {
            if (!IsOn || !IsCanDoAction || direction == Side.Center)
            {
                return;
            }
            var transformIns = GetCashedComponent<Transform>();
            var shift = Vector3.zero;
            switch (direction)
            {
                case Side.Right:
                    if (currentSide == Side.Right)
                    {
                        return;
                    }
                    else
                    {
                        currentSide = (Side)(((int)currentSide) + 1);
                    }
                    shift = transformIns.right * ShiftDistance;
                    break;
                case Side.Left:
                    if (currentSide == Side.Left)
                    {
                        return;
                    }
                    else
                    {
                        currentSide = (Side)(((int)currentSide) - 1);
                    }
                    shift = transformIns.right * (-1) * ShiftDistance;
                    break;
            }
            transformIns.position = transformIns.position + shift;
            actionDelayCoroutine.Start(ShiftDelayCoroutine());
        }
        /// <summary>
        /// Особое действие во время движения. Если у объекта есть Rigitbody, то производится 
        /// прыжок с силой, заданной в соответсвующем параметре компонента.
        /// </summary>
        public void Jump()
        {
            if (!isOn || !IsCanDoAction)
                return;
            var rigit = GetCashedComponent<Rigidbody>();
            if (rigit != null)
                rigit.AddForce(new Vector3(0, JumpPower));
        }
    }

    public delegate void MovingHandler();

    public delegate void MovingSpeedChangedHandler(Vector3 oldSpeed);
}