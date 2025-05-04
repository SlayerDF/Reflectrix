using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UniTrait.Editor
{
    [CustomEditor(typeof(UniTraitContainer), true)]
    public class UniTraitContainerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var rect = GUILayoutUtility.GetLastRect();
            rect.y += rect.height + 12f;
            rect.width = 140f;
            rect.height = 20f;

            if (GUI.Button(rect, "Traits debug actions"))
            {
                ShowTraitMenu(((UniTraitContainer)target).GetTraits(), rect);
            }

            GUILayout.Space(rect.height + 12f);
        }

        private static void ShowTraitMenu(IEnumerable<IUniTrait> traits, Rect position)
        {
            var menu = new GenericMenu();

            foreach (var trait in traits)
            {
                const BindingFlags methodFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var traitType = trait.GetType();
                var traitMethods = traitType.GetMethods(methodFlags);

                foreach (var method in traitMethods)
                {
                    var attr = method.GetCustomAttribute<UniTraitDebugActionAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }

                    menu.AddItem(new GUIContent($"{traitType.Name}/{attr.Label}"), false,
                        () => method.Invoke(trait, null));
                }
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddDisabledItem(new GUIContent("No actions"));
            }

            menu.DropDown(position);
        }
    }
}
