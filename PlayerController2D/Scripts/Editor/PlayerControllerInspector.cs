using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;
using Aspekt.PlayerController;
using static Aspekt.IO.ControllerInputHandler;
using System.Reflection;

namespace Aspekt.IO.Edit
{
    [CustomEditor(typeof(PlayerController))]
    public class Aspekt : Editor
    {
        private PlayerController controller;

        string[] classStrings;
        private bool isAddingButton = false;
        private Keybinding newKeybinding;
        private int bindingIndex;

        private int getKeycodeIndex = -1;

        public override void OnInspectorGUI()
        {
            controller = (PlayerController)target;
            Event e = Event.current;

            if (getKeycodeIndex >= 0)
            {
                if (e.isKey || e.isMouse)
                {
                    bool isValidKey = false;
                    bool isInvalidKey = false;
                    KeyCode code = KeyCode.Escape;
                    switch (e.type)
                    {
                        case EventType.MouseDown:
                            isValidKey = e.button == 0 || e.button == 1;
                            if (e.button == 0) code = KeyCode.Mouse0;
                            if (e.button == 1) code = KeyCode.Mouse1;
                            break;
                        case EventType.KeyDown:
                            code = e.keyCode;
                            isInvalidKey = code == KeyCode.Escape;
                            isValidKey = !isInvalidKey;
                            break; 
                    }

                    if (isValidKey)
                    {
                        var binding = controller.KeyBindings[getKeycodeIndex];
                        binding.KeyboardBinding = code;
                        controller.KeyBindings[getKeycodeIndex] = binding;
                    }

                    if (isValidKey || isInvalidKey)
                    {
                        getKeycodeIndex = -1;
                        EditorUtility.SetDirty(controller);
                    }
                }
            }

            for (int i = 0; i < controller.KeyBindings.Count; i++)
            {
                var binding = controller.KeyBindings[i];

                EditorGUILayout.BeginHorizontal();

                string className = binding.HandlerTypeString.Split('.').Last();
                EditorGUILayout.LabelField(className);

                if (getKeycodeIndex == i)
                {

                    EditorGUILayout.LabelField("press a key");
                    //binding.KeyboardBinding = (KeyCode)EditorGUILayout.EnumPopup(binding.KeyboardBinding);
                }
                else
                {
                    if (GUILayout.Button(binding.KeyboardBinding.ToString()))
                    {
                        getKeycodeIndex = i;
                    }
                }
                binding.ControllerBinding = (BindableButtons)EditorGUILayout.EnumPopup(binding.ControllerBinding);
                controller.KeyBindings[i] = binding;
                EditorGUILayout.EndHorizontal();
            }

            if (isAddingButton)
            {
                EditorGUILayout.BeginHorizontal();
                bindingIndex = EditorGUILayout.Popup(bindingIndex, classStrings);
                newKeybinding.HandlerTypeString = classStrings[bindingIndex];

                newKeybinding.KeyboardBinding = (KeyCode)EditorGUILayout.EnumPopup(newKeybinding.KeyboardBinding);
                newKeybinding.ControllerBinding = (BindableButtons)EditorGUILayout.EnumPopup(newKeybinding.ControllerBinding);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add"))
                {
                    controller.KeyBindings.Add(newKeybinding);
                    isAddingButton = false;
                }
                else if (GUILayout.Button("Cancel"))
                {
                    isAddingButton = false;
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("Add Button"))
            {
                isAddingButton = true;
                newKeybinding = new Keybinding();
                bindingIndex = 0;

                var classes = Assembly.GetAssembly(typeof(PlayerControllerButtonHandler)).GetTypes()
                    .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(PlayerControllerButtonHandler))).ToArray();
                
                classStrings = classes.Select(c => c.ToString()).ToArray();
            }
        }
    }
}
