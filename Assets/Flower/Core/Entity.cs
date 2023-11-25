using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Flower
{
    public abstract class Entity : MonoBehaviour, IEntityInterface
    {
        internal delegate void EntityMessages(object[] messageData);
        internal Dictionary<int, Action<object[]>> Actions = new Dictionary<int, Action<object[]>>();
        private Dictionary<(string, int), Action<object[]>> _externalActions = new Dictionary<(string, int), Action<object[]>>();

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
                throw new Exception($"Not all actions were deleted before initialize. Last id = {_nextActionId}.");
            }

            _nextActionId = 0;
            Actions = new Dictionary<int, Action<object[]>>();
            _externalActions = new Dictionary<(string, int), Action<object[]>>();

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

        internal void Link(int actionId, EntityMessages message, int messageHash)
        {
            if (_externalActions.TryGetValue((message.Method.Name, messageHash), out Action<object[]> checkedAction))
            {
                Debug.LogError("fuck u");
                return;
            }
            else
            {
                Debug.Log("Method: " + message.Method.Name + " = " + messageHash);
            }

            var action = new Action<object[]>(message);
            Actions[actionId] += action;
            _externalActions.Add((message.Method.Name, messageHash), action);
        }

        internal void Unlink(int actionId, MethodInfo method, int messageHash)
        {
            if (_externalActions.TryGetValue((method.Name, messageHash), out Action<object[]> checkedAction))
            {
                Actions[actionId] -= checkedAction;
                _externalActions.Remove((method.Name, messageHash));
            }
        }
    }
}