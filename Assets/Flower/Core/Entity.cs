using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public abstract class Entity : MonoBehaviour
    {
        private static int _nextActionId = 0;
        internal delegate void EntityMessages(object[] messageData);
        internal Dictionary<int, Action<object[]>> Actions = new Dictionary<int, Action<object[]>>();

        public void Initialize()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i] = null;
            }
            Actions = new Dictionary<int, Action<object[]>>();

            InitialzeActions();
        }
        protected abstract void InitialzeActions();

        protected void RegisterAction(ref Action<object[]> action)
        {
            int index = _nextActionId;

            Actions.Add(index, action);
            action += (object[] messageData) => Actions[index]?.Invoke(messageData);

            _nextActionId++;
        }

        internal void Link(int actionId, EntityMessages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Try to link 'null' message.");
            }

            Debug.Log($"Link action #{actionId} with {message.Method.Name}");
            Actions[actionId] += new Action<object[]>(message);
        }

        internal void Unlink(int actionId, EntityMessages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Try to unlink 'null' message.");
            }

            Debug.Log($"Unlink action #{actionId} with {message.Method.Name}");
            Actions[actionId] -= new Action<object[]>(message);
        }
    }
}