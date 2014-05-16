using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ItzWarty
{
   public delegate void OnTreeHierarchyModified();
   public class Node
   {
      protected Node m_parent = null;
      protected List<Node> m_children = new List<Node>();

      /// <summary>
      /// Event fires when: Child of this tree node is added/removed
      /// </summary>
      public event OnTreeHierarchyModified TreeHierarchyModified;
      protected virtual void OnTreeHierarchyModified()
      {
         if (TreeHierarchyModified != null)
            TreeHierarchyModified();
         if (this.Parent != null)
            this.Parent.OnTreeHierarchyModified();
      }

      public Node() { }
      public Node(Node parent)
      {
         this.m_parent = parent;
      }
      /// <summary>
      /// Gets the m_parent of this given node.
      /// </summary>
      /// <returns></returns>
      public Node GetParent() { return this.m_parent; }
      /// <summary>
      /// Sets this node's m_parent.  Also appends self to m_parent if not previously appeneded.
      /// </summary>
      /// <param name="parent"></param>
      public void SetParent(Node parent) { SetParent(parent, true); }
      private void SetParent(Node parent, bool issueRelink)
      {
         if (this.m_parent != null)
            this.m_parent.RemoveChild(this);

         this.m_parent = parent;
         if (issueRelink && parent != null) //if we need to issue a relink, tree structure isn't finalized
            this.m_parent.AddChild(this, false);
         else                                //Tree modifications are finalized, so we can invoke events now.
            OnTreeHierarchyModified();
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
      /// Adds a child to this node.  The child's .m_parent is updated as well.
      /// </summary>
      /// <param name="node"></param>
      public void AddChild(Node node) { AddChild(node, true); }
      private void AddChild(Node node, bool issueRelink)
      {
         lock (this)
         {
            this.m_children.Add(node);

            if (issueRelink)
               node.SetParent(this, false);
            else
               OnTreeHierarchyModified();
         }
      }

      /// <summary>
      /// Unappends a child from this node. The child's .m_parent is set to null.
      /// </summary>
      /// <param name="node"></param>
      public void RemoveChild(Node node) { RemoveChild(node, true); }
      private void RemoveChild(Node node, bool issueRelink)
      {
         lock (this)
         {
            this.m_children.Remove(node);

            if (issueRelink && node != null)
               node.SetParent(null, false);
            else
               OnTreeHierarchyModified();
         }
      }

      /// <summary>
      /// Removes all child nodes from this node
      /// </summary>
      public void ClearChildren()
      {
         while (this.m_children.Any())
            RemoveChild(this.m_children.First());
      }

      public virtual ReadOnlyCollection<Node> Children
      {
         get
         {
            return this.m_children.AsReadOnly();
         }
      }
   }
}
