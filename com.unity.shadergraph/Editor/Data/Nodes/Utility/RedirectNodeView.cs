using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor.Graphing;
using UnityEditor.ShaderGraph.Drawing;
using UnityEditor.Experimental.GraphView;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace UnityEditor.ShaderGraph
{
    class RedirectNodeView : RedirectNode, IShaderNodeView
    {
        IEdgeConnectorListener m_ConnectorListener;
        VisualElement m_TitleContainer;
        GraphView m_GraphView;

        public RedirectNodeView() : base()
        {
        }

        public override void InitializeFromEdge(Edge edge, GraphView graphView)
        {
            // Created from de-serialization
            if(edge == null)
                return;

            orientation = edge.output.orientation;
            SplitEdge(edge, graphView);
        }

        // Tie the nodeView to its data
        public void ConnectToData(AbstractMaterialNode inNode, IEdgeConnectorListener connectorListener, GraphView graphView)
        {
            if (inNode == null)
                return;

            // Set references
            var nodeData = inNode as RedirectNodeData;
            nodeData.nodeView = this;
            node = inNode;
            title = node.name;
            m_GraphView = graphView;
            m_ConnectorListener = connectorListener;

            viewDataKey = node.guid.ToString();

            // Set the VisualElement's position
            SetPosition(new Rect(node.drawState.position.x, node.drawState.position.y, 0, 0));
            AddSlots(node.GetSlots<MaterialSlot>());

            InitializeFromEdge(nodeData.m_Edge, m_GraphView);
        }

        //public void UpdateSlots(IEnumerable<MaterialSlot> slots)
        // public void UpdateSlots(Edge edge)
        // {
        //     // Need also to force the slotreference to update on all children.
        //     // Set the color of the ports
        //     foreach (var port in inputContainer.Query<Port>().ToList())
        //     {
        //         port.visualClass = edge.output.GetSlot().concreteValueType.ToClassName();
        //     }
        //
        //     foreach (var port in outputContainer.Query<Port>().ToList())
        //     {
        //         port.visualClass = edge.output.GetSlot().concreteValueType.ToClassName();
        //         foreach (Edge connection in port.connections)
        //         {
        //             if (connection.input.GetSlot().owner is RedirectNodeData redirectNode)
        //             {
        //                 // If incoming node is redirect node we force the child to inherit the slot reference
        //                 // if (connection.output.GetSlot().owner is RedirectNodeData redirLeftNode)
        //                 // {
        //                 //     redirectNode.slotReferenceInput = redirLeftNode.slotReferenceInput;
        //                 // }
        //
        //                 redirectNode.nodeView.UpdateSlots(connection);
        //             }
        //         }
        //     }
        // }

        // Get the left most slot referende
        // If it ends with being a redirect node, then it means it is a dead end.
        // if it is another node than a redirect node we use that and return that slot ref
        // public SlotReference GetLeftMostSlotReference()
        // {
        //     foreach (var port in inputContainer.Children().OfType<ShaderPort>())
        //     {
        //         var slot = port.slot;
        //         var graph = slot.owner.owner;
        //         var edges = graph.GetEdges(slot.slotReference).ToList();
        //         if (edges.Any())
        //         {
        //             var outputSlotRef = edges[0].outputSlot;
        //             var nodeFromGuid = graph.GetNodeFromGuid(outputSlotRef.nodeGuid);
        //             // If this is a redirect node we continue to look for the top one
        //             // else we return the actual slot reference
        //             if (nodeFromGuid is RedirectNodeData redirNode)
        //             {
        //                 return redirNode.nodeView.GetLeftMostSlotReference();
        //             }
        //
        //             return outputSlotRef;
        //         }
        //     }
        //
        //     return new SlotReference();
        // }

        // public SlotReference? GetLeftMostSlotReference()
        // {
        //     foreach (var port in inputContainer.Children().OfType<ShaderPort>())
        //     {
        //         var slot = port.slot;
        //         var graph = slot.owner.owner;
        //         var edges = graph.GetEdges(slot.slotReference).ToList();
        //         if (edges.Any())
        //         {
        //             var outputSlotRef = edges[0].outputSlot;
        //             var nodeFromGuid = graph.GetNodeFromGuid(outputSlotRef.nodeGuid);
        //             // If this is a redirect node we continue to look for the top one
        //             if (nodeFromGuid is RedirectNodeData redirNode)
        //             {
        //                 return redirNode.nodeView.GetLeftMostSlotReference();
        //             }
        //
        //             return outputSlotRef;
        //         }
        //     }
        //
        //     return null;
        // }

        // void GetAllRedirectNodes(RedirectNodeData node, ref List<RedirectNodeData> allNodes)
        // {
        //     if(node == null)
        //         return;
        //     allNodes.Add(node);
        //     foreach (var port in node.nodeView.outputContainer.Children().OfType<ShaderPort>())
        //     {
        //         var slot = port.slot;
        //         var graph = slot.owner.owner;
        //         var edges = graph.GetEdges(slot.slotReference).ToList();
        //
        //         foreach (IEdge edge in edges)
        //         {
        //             // get the input details
        //             var inputSlotRef = edge.inputSlot;
        //             var inputNode = graph.GetNodeFromGuid(inputSlotRef.nodeGuid);
        //             if (inputNode is RedirectNodeData redirectNode)
        //             {
        //                 GetAllRedirectNodes(redirectNode, ref allNodes);
        //             }
        //         }
        //     }
        // }

        // public void UpdatePortTypes()
        // {
        //     // Get all the RedirectNodes and update them all in one go.
        //     List<RedirectNodeData> connectedRedirectNodes = new List<RedirectNodeData>();
        //     // Always add first node / this node
        //     GetAllRedirectNodes(node as RedirectNodeData, ref connectedRedirectNodes);
        //
        //     // Get the concrete value type of the incoming edge output
        //     string concreteValueType = "";
        //     foreach (var port in inputContainer.Children().OfType<ShaderPort>())
        //     {
        //         var slot = port.slot;
        //         var graph = slot.owner.owner;
        //         var edges = graph.GetEdges(slot.slotReference).ToList();
        //
        //         // get the output details
        //         var outputSlotRef = edges[0].outputSlot;
        //         var outputNode = graph.GetNodeFromGuid(outputSlotRef.nodeGuid);
        //         concreteValueType = outputNode.FindOutputSlot<MaterialSlot>(outputSlotRef.slotId).concreteValueType.ToClassName();
        //     }
        //
        //     foreach (RedirectNodeData redirectNode in connectedRedirectNodes)
        //     {
        //         foreach (var port in redirectNode.nodeView.inputContainer.Children().Concat(redirectNode.nodeView.outputContainer.Children()).OfType<ShaderPort>())
        //         {
        //             port.visualClass = concreteValueType;
        //         }
        //     }
        // }

        public void AddSlots(IEnumerable<MaterialSlot> slots)
        {
            foreach (var slot in slots)
            {
                if (slot.hidden)
                    continue;

                var port = ShaderPort.Create(slot, m_ConnectorListener);

                if (slot.isOutputSlot)
                    outputContainer.Add(port);
                else
                    inputContainer.Add(port);
            }
        }

        public override void SplitEdge(Edge edge, GraphView graphView)
        {
            var nodeData = userData as AbstractMaterialNode;
            var matGraphView = graphView as MaterialGraphView;

            if (edge != null)
            {
                var edge_outSlot = edge.output.GetSlot();
                var edge_inSlot = edge.input.GetSlot();

                var edge_outSlotRef = edge_outSlot.owner.GetSlotReference(edge_outSlot.id);
                var edge_inSlotRef = edge_inSlot.owner.GetSlotReference(edge_inSlot.id);

                // Hard-coded for single input-output. Changes would be needed for multi-input redirects
                var node_inSlotRef = nodeData.GetSlotReference(0);
                var node_outSlotRef = nodeData.GetSlotReference(1);

                matGraphView.graph.Connect(edge_outSlotRef, node_inSlotRef);
                matGraphView.graph.Connect(node_outSlotRef, edge_inSlotRef);
            }

            // Set the color of the ports
            foreach (var port in inputContainer.Query<Port>().ToList())
            {
                port.visualClass = edge.output.GetSlot().concreteValueType.ToClassName();
            }

            foreach (var port in outputContainer.Query<Port>().ToList())
            {
                port.visualClass = edge.output.GetSlot().concreteValueType.ToClassName();
            }
        }



        #region IShaderNodeView interface
        public Node gvNode => this;
        public AbstractMaterialNode node { get; private set; }
        public VisualElement colorElement { get { return this; } }

        public void Dispose()
        {
            node = null;
            userData = null;
        }

        public void UpdatePortInputTypes()
        {
            foreach (var anchor in inputContainer.Children().Concat(outputContainer.Children()).OfType<ShaderPort>())
            {
                var slot = anchor.slot;
                anchor.portName = slot.displayName;
                anchor.visualClass = slot.concreteValueType.ToClassName();
            }
            //m_GraphView.MarkDirtyRepaint();

            // foreach (var portInputView in m_PortInputContainer.Children().OfType<PortInputView>())
            //     portInputView.UpdateSlotType();
            //
            // foreach (var control in m_ControlItems.Children())
            // {
            //     var listener = control as AbstractMaterialNodeModificationListener;
            //     if (listener != null)
            //         listener.OnNodeModified(ModificationScope.Graph);
            // }
        }

        public void OnModified(ModificationScope scope)
        {
            //UpdateTitle();
            //if (node.hasPreview)
            //    UpdatePreviewExpandedState(node.previewExpanded);

            //base.expanded = node.drawState.expanded;

            // Update slots to match node modification

            if (scope == ModificationScope.Topological)
            {
                Debug.Log("FLORP");
                //RecreateSettings();

                var slots = node.GetSlots<MaterialSlot>().ToList();

                var inputPorts = inputContainer.Children().OfType<ShaderPort>().ToList();
                foreach (var port in inputPorts)
                {
                    var currentSlot = port.slot;
                    var newSlot = slots.FirstOrDefault(s => s.id == currentSlot.id);
                    if (newSlot == null)
                    {
                        // Slot doesn't exist anymore, remove it
                        inputContainer.Remove(port);

                        // We also need to remove the inline input
                        // var portInputView = m_PortInputContainer.Children().OfType<PortInputView>().FirstOrDefault(v => Equals(v.slot, port.slot));
                        // if (portInputView != null)
                        //     portInputView.RemoveFromHierarchy();
                    }
                    else
                    {
                        port.slot = newSlot;
                        // var portInputView = m_PortInputContainer.Children().OfType<PortInputView>().FirstOrDefault(x => x.slot.id == currentSlot.id);
                        // if (newSlot.isConnected)
                        // {
                        //     portInputView?.RemoveFromHierarchy();
                        // }
                        // else
                        // {
                        //     portInputView?.UpdateSlot(newSlot);
                        // }

                        slots.Remove(newSlot);
                    }
                }

                var outputPorts = outputContainer.Children().OfType<ShaderPort>().ToList();
                foreach (var port in outputPorts)
                {
                    var currentSlot = port.slot;
                    var newSlot = slots.FirstOrDefault(s => s.id == currentSlot.id);
                    if (newSlot == null)
                    {
                        outputContainer.Remove(port);
                    }
                    else
                    {
                        port.slot = newSlot;
                        slots.Remove(newSlot);
                    }
                }

                AddSlots(slots);

                slots.Clear();
                slots.AddRange(node.GetSlots<MaterialSlot>());

                if (inputContainer.childCount > 0)
                    inputContainer.Sort((x, y) => slots.IndexOf(((ShaderPort)x).slot) - slots.IndexOf(((ShaderPort)y).slot));
                if (outputContainer.childCount > 0)
                    outputContainer.Sort((x, y) => slots.IndexOf(((ShaderPort)x).slot) - slots.IndexOf(((ShaderPort)y).slot));

                //UpdatePortInputs();
                //UpdatePortInputVisibilities();
                //m_GraphView.MarkDirtyRepaint();
            }

            //RefreshExpandedState(); //This should not be needed. GraphView needs to improve the extension api here

            // foreach (var listener in m_ControlItems.Children().OfType<AbstractMaterialNodeModificationListener>())
            // {
            //     if (listener != null)
            //         listener.OnNodeModified(scope);
            // }
        }

        public void SetColor(Color newColor)
        {

        }

        public void ResetColor()
        {

        }
        #endregion
    }
}
