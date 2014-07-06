using System;
using System.Collections.Generic;
namespace ProjectGreen.Render
{
    class ContextStack<T>
    {
        Func<T, T, bool> areEqual;
        Stack<T> stack;

        public ContextStack(Func<T, T, bool> areEqual)
        {
            this.areEqual = areEqual;
            this.stack = new Stack<T>();
        }

        public void Push(T item)
        {
            stack.Push(item);
        }

        public T Top
        {
            get { return stack.Peek(); }
        }

        public bool Empty
        { get { return stack.Count == 0; } }

        /// <summary>
        /// Pop item from top of stack
        /// </summary>
        public void Pop(T expectedOnTop)
        {
            T top = stack.Pop();

            if (!areEqual(top, expectedOnTop))
            { throw new InvalidOperationException("GL context stack corrupted"); }
        }
    }
}
