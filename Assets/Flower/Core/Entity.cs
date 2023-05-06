using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public abstract class Entity : MonoBehaviour
    {
        internal delegate void EntityMessages(object[] messageData);
        internal Dictionary<int, Action<object[]>> Actions = new Dictionary<int, Action<object[]>>();

        private int _nextActionId;

        internal void Initialize()
        {
            _nextActionId = Actions.Count - 1;

            if (Actions.Count > 0)
            {
                ResetActions();
            }

            if (_nextActionId != -1)
            {
                throw new Exception($"Not all actions were deleted before initialize. Last id = {_nextActionId}");
            }

            _nextActionId = 0;
            Actions = new Dictionary<int, Action<object[]>>();

            InitialzeActions();
        }

        protected abstract void ResetActions();
        protected abstract void InitialzeActions();

        protected void AddAction(ref Action<object[]> action)
        {
            if (Actions.ContainsValue(action))
            {
                throw new Exception("Was try to add contanied action.");
            }

            int index = _nextActionId;

            Actions.Add(index, action);
            action += (object[] messageData) => Actions[Actions.Count - 1]?.Invoke(messageData);

            _nextActionId++;
        }

        protected void DeleteAction(ref Action<object[]> action)
        {
            Actions.Remove(_nextActionId);
            action = null;

            _nextActionId--;
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