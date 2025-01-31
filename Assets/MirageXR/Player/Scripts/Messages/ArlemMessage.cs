﻿using UnityEngine;

namespace MirageXR
{
    public static class ArlemMessage
    {
        public static void ReadMessages(Action action)
        {
            foreach (var message in action.exit.messages)
            {
                if (!string.IsNullOrEmpty(message.target) && !string.IsNullOrEmpty(message.text))
                {
                    Read(message.target, message.text);
                }
            }
        }

        private static void Read(string target, string message)
        {
            if (string.IsNullOrEmpty(target))
            {
                Debug.Log("Message target not set.");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                Debug.Log("Message text not set.");
                return;
            }

            if (target == "all" || target == WorkplaceManager.GetUser())
            {
                Maggie.Speak(message);
            }
        }
    }
}