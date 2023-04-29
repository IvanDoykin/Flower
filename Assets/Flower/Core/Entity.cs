using System;
using UnityEngine;

namespace Flower
{
    public abstract class Entity : MonoBehaviour
    {
        public delegate void EntityMessages();

        public event EntityMessages Messages;

        protected void InvokeMessages()
        {
            Messages.Invoke();
        }

        public void Reset()
        {
            Messages = null;
        }

        public void Link(EntityMessages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Try to link 'null' message.");
            }

            Messages += message;
        }

        public void Unlink(EntityMessages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Try to unlink 'null' message.");
            }

            Messages -= message;
        }
    }
}