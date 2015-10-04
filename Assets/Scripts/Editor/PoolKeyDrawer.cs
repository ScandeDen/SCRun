using UnityEngine;
using System.Collections;
using UnityEditor;
using Core.Spawners;
using Core;

namespace Gui.Editor
{
    /// <summary>
    /// Отрисовщик ключей пулов в виде Popup'ов
    /// </summary>
    [CustomPropertyDrawer(typeof(PoolKeyAttribute))]
    public class PoolKeyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, 
                GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var amountRect = new Rect(position.x, position.y, position.width, 
                position.height);
            if (PoolsManager.Instance.GetPoolKeys().Count == 0)
            {
                EditorGUI.LabelField(amountRect, "No pools created");
                property.stringValue = "";
            }
            else
            {
                var contain = PoolsManager.Instance.GetPoolKeys();
                var indexInManager = PoolsManager.Instance.GetPoolIndex(property.stringValue);
                var result = EditorGUI.Popup(amountRect, 
                    indexInManager == -1 ? 0 : indexInManager,
                    contain.ToArray());
                property.stringValue = PoolsManager.Instance.GetPoolKey(result);
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}