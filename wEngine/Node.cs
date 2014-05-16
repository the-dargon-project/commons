using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wEngine
{
    public class Node
    {
        private Node parent = null;
        private SortedSet<Node> children = new SortedSet<Node>();
        public Node() { }
        public Node(Node parent)
        {
            this.parent = parent;
        }
        /// <summary>
        /// Gets the parent of this given node.
        /// </summary>
        /// <returns></returns>
        public Node GetParent() { return this.parent; }
        /// <summary>
        /// Sets this node's parent.  Also appends self to parent if not previously appeneded.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Node parent) { SetParent(parent, true); }
        private void SetParent(Node parent, bool issueRelink)
        {
            if (this.parent != null)
                this.parent.RemoveChild(this);

            this.parent = parent;
            if (issueRelink && parent != null)
                this.parent.AddChild(this, false);
        }
        public Node Parent
        {
            get
            {
                return GetParent();
            }
            set
            {
                SetParent(value);
            }
        }

        /// <summary>
        /// Adds a child to this node.  The child's .parent is updated as well.
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(Node node){ AddChild(node, true); }
        private void AddChild(Node node, bool issueRelink)
        {
            this.children.Add(node);

            if (issueRelink)
                node.SetParent(this, false);
        }

        /// <summary>
        /// Unappends a child from this node. The child's .parent is set to null.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(Node node){ RemoveChild(node, true); }
        private void RemoveChild(Node node, bool issueRelink)
        {
            this.children.Remove(node);

            if (issueRelink && node != null)
                node.SetParent(null, false);
        }
    }
}
