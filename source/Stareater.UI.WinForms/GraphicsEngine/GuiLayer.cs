using Stareater.GraphicsEngine.GuiElements;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GraphicsEngine
{
	class GuiLayer
	{
		private readonly Dictionary<AGuiElement, HashSet<AGuiElement>> guiHierarchy = new Dictionary<AGuiElement, HashSet<AGuiElement>>();

		private readonly float z0;
		private readonly float layerThickness;

		public AGuiElement Root { get; private set; }

		public GuiLayer(float z0, float layerThickness)
		{
			this.z0 = z0;
			this.layerThickness = layerThickness;
			this.Root = new GuiPanel();

			this.guiHierarchy[this.Root] = new HashSet<AGuiElement>();
			this.Root.Position.FixedCenter(0, 0);
			this.Root.SetDepth(z0, layerThickness);
		}

		public void AddElement(AGuiElement element, AScene scene)
		{
			this.AddElement(element, this.Root, scene);
		}

		public void AddElement(AGuiElement element, AGuiElement parent, AScene scene)
		{
			var treeRoot = element.Below ?? parent;
			this.guiHierarchy[element] = new HashSet<AGuiElement>();
			this.guiHierarchy[treeRoot].Add(element);

			element.SetDepth(treeRoot.Z0 - treeRoot.ZRange, treeRoot.ZRange);
			element.Attach(scene, parent);
			this.updateGuiZ(element);
			element.Position.Recalculate();
		}

		public void RemoveElement(AGuiElement element)
		{
			if (this.guiHierarchy.ContainsKey(element))
				foreach (var child in this.guiHierarchy[element].ToList())
					this.RemoveElement(child);

			this.guiHierarchy[element.Below ?? element.Parent].Remove(element);
			this.guiHierarchy.Remove(element);
			element.Detach();
		}

		public bool Contains(AGuiElement element)
		{
			return this.guiHierarchy.ContainsKey(element);
		}

		public IEnumerable<AGuiElement> EnumeratePostfix()
		{
			foreach (var child in this.guiHierarchy[this.Root])
				foreach (var element in this.enumeratePostfix(child))
					yield return element;
		}

		public IEnumerable<AGuiElement> ElementChildren(AGuiElement element)
		{
			return this.guiHierarchy[element];
		}

		private IEnumerable<AGuiElement> enumeratePostfix(AGuiElement parent)
		{
			if (this.guiHierarchy.ContainsKey(parent))
				foreach (var child in this.guiHierarchy[parent])
					foreach (var element in this.enumeratePostfix(child))
						yield return element;

			yield return parent;
		}

		private void updateGuiZ(AGuiElement element)
		{
			var layers = 2;
			var subroot = element;
			var treeParent = subroot.Below ?? subroot.Parent;

			while (treeParent != this.Root)
			{
				subroot = treeParent;
				treeParent = subroot.Below ?? subroot.Parent;
				layers++;
			}

			var zRange = this.layerThickness / layers;
			if (subroot.ZRange <= zRange)
				return;

			/*
			 * Updates Z ranges of a subtree, immediate child of layer root.
			 * Other subtrees should not visually overlap so no update needed.
			 */
			subroot.SetDepth(this.z0, zRange);
			var subtrees = new Queue<AGuiElement>();
			subtrees.Enqueue(subroot);

			while (subtrees.Count > 0)
			{
				subroot = subtrees.Dequeue();

				if (this.guiHierarchy[subroot].Any())
					foreach (var item in this.guiHierarchy[subroot])
					{
						item.SetDepth(subroot.Z0 - subroot.ZRange, subroot.ZRange);
						subtrees.Enqueue(item);
					}
			}
		}
	}
}
