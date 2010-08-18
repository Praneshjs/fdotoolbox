#region LGPL Header
// Copyright (C) 2009, Jackie Ng
// http://code.google.com/p/fdotoolbox, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
//
// See license.txt for more/additional licensing information
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OSGeo.FDO.Schema;
using System.Diagnostics;
using ICSharpCode.Core;
using System.Drawing;
using FdoToolbox.Base.Forms;
using OSGeo.FDO.Expression;
using FdoToolbox.Core.Feature;
using OSGeo.FDO.Connections.Capabilities;

namespace FdoToolbox.Tasks.Controls.BulkCopy
{
    /// <summary>
    /// Helper class to perform manipulation of the tree nodes
    /// </summary>
    internal class ExpressionMappingsNodeDecorator
    {
        private TreeNode _node;

        private TreeView _treeView;

        internal readonly CopyTaskNodeDecorator Parent;

        private Dictionary<string, PropertyConversionNodeDecorator> _conv;

        private ContextMenuStrip _mappingsMenu;
        private ContextMenuStrip _mapExprContextMenu;

        internal ExpressionMappingsNodeDecorator(CopyTaskNodeDecorator parent, TreeNode exprsNode)
        {
            _conv = new Dictionary<string, PropertyConversionNodeDecorator>();
            Parent = parent;
            _node = exprsNode;
            _treeView = _node.TreeView;
            InitContextMenus();
            _node.ContextMenuStrip = _mappingsMenu;
        }

        private void InitContextMenus()
        {
            _mappingsMenu = new ContextMenuStrip();
            _mapExprContextMenu = new ContextMenuStrip();

            //Mappings
            _mappingsMenu.Items.Add("Add Computed Expression", null, OnAddExpression);

            //Selected expression
            _mapExprContextMenu.Items.Add("Edit Expression", null, OnEditExpression);
            _mapExprContextMenu.Items.Add("Rename alias", null, OnRenameAlias);
            _mapExprContextMenu.Items.Add(new ToolStripSeparator());
            _mapExprContextMenu.Items.Add("Remove Alias", ResourceService.GetBitmap("cross"), OnRemoveAlias);
            _mapExprContextMenu.Items.Add("Remove Mapping", ResourceService.GetBitmap("cross"), OnRemoveExpressionMapping);
            _mapExprContextMenu.Items.Add(new ToolStripSeparator());
            _mapExprContextMenu.Items.Add("Map Expression to property of same alias name (create if necessary)", null, OnMapAutoCreate);
            _mapExprContextMenu.Items.Add(new ToolStripSeparator());

            SortedList<string, ToolStripMenuItem> items = new SortedList<string, ToolStripMenuItem>();

            if (Parent.TargetClass != null)
            {
                foreach (PropertyDefinition p in Parent.TargetClass.Properties)
                {
                    if (p.PropertyType == PropertyType.PropertyType_DataProperty || p.PropertyType == PropertyType.PropertyType_GeometricProperty)
                    {
                        DataPropertyDefinition d = p as DataPropertyDefinition;
                        GeometricPropertyDefinition g = p as GeometricPropertyDefinition;
                        string name = p.Name;
                        string text = "Map to property: " + name;
                        Image icon = null;
                        if (d != null)
                        {
                            if (d.IsAutoGenerated || d.ReadOnly)
                                continue;

                            icon = ResourceService.GetBitmap("table");
                        }
                        else if (g != null)
                        {
                            if (g.ReadOnly)
                                continue;

                            icon = ResourceService.GetBitmap("shape_handles");
                        }
                        ToolStripMenuItem itm1 = new ToolStripMenuItem(text, icon, OnMapExpression);
                        itm1.Tag = name;
                        items.Add(name, itm1);
                    }
                }
            }

            foreach (ToolStripMenuItem item in items.Values)
            {
                _mapExprContextMenu.Items.Add(item);
            }
        }

        private void OnAddExpression(object sender, EventArgs e)
        {
            string expr = ExpressionEditor.NewExpression(Parent.GetSourceConnection(), Parent.SourceClass, ExpressionMode.Normal);
            if (expr != null)
            {
                string alias = string.Empty;
                do 
                {
                    alias = MessageService.ShowInputBox("Expression Alias", "Alias", "MyExpression");
                    if (alias == null) //cancelled
                        return;
                } 
                while(_node.Nodes[alias] != null);
                this.AddExpression(alias, expr);
            }
        }

        private void OnMapAutoCreate(object sender, EventArgs e)
        {
            TreeNode exprNode = _treeView.SelectedNode;
            string alias = exprNode.Name;
            try
            {
                this.MapExpression(alias, alias, true);
            }
            catch (MappingException ex)
            {
                MessageService.ShowError(ex);
            }
        }

        private void OnRemoveAlias(object sender, EventArgs e)
        {
            this.RemoveExpression(_treeView.SelectedNode.Name);
        }

        private void OnRenameAlias(object sender, EventArgs e)
        {
            TreeNode exprNode = _treeView.SelectedNode;
            string alias = exprNode.Name;
            string newAlias = string.Empty;
            do
            {
                newAlias = MessageService.ShowInputBox("New Alias", "Enter the new alias", alias);
                if(newAlias == null) //null = cancel
                    return;
            }
            while (_node.Nodes[newAlias] != null);
            exprNode.Name = newAlias;
            exprNode.Text = newAlias;
        }

        private void OnEditExpression(object sender, EventArgs e)
        {
            TreeNode exprNode = _treeView.SelectedNode;
            Debug.Assert(exprNode.Tag != null);

            ExpressionMappingInfo expr = exprNode.Tag as ExpressionMappingInfo;
            if (expr != null)
            {
                string newExpr = ExpressionEditor.EditExpression(Parent.GetSourceConnection(), Parent.SourceClass, expr.Expression, ExpressionMode.Normal);
                if (newExpr != null) //null = cancel
                {
                    exprNode.ToolTipText = "Expression: " + newExpr;
                    expr.Expression = newExpr;
                }
            }
        }

        private void OnMapExpression(object sender, EventArgs e)
        {
            ToolStripItem itm = sender as ToolStripItem;
            Debug.Assert(itm != null);
            Debug.Assert(itm.Tag != null);

            TreeNode propertyNode = _treeView.SelectedNode;
            TreeNode taskNode = propertyNode.Parent.Parent;
            try
            {
                this.MapExpression(propertyNode.Name, itm.Tag.ToString(), false);
            }
            catch (MappingException ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
        }

        private void OnRemoveExpressionMapping(object sender, EventArgs e)
        {
            ToolStripItem itm = sender as ToolStripItem;
            Debug.Assert(itm != null);

            TreeNode exprNode = _treeView.SelectedNode;
            this.RemoveAliasMapping(exprNode.Name);
        }

        public void AddExpression(string srcAlias, string expressionText)
        {
            TreeNode exprNode = _node.Nodes[srcAlias];
            if (exprNode == null)
            {
                exprNode = new TreeNode();
                exprNode.Text = srcAlias;
                exprNode.ToolTipText = "Expression: " + expressionText;
                exprNode.Name = srcAlias;
                ExpressionMappingInfo map = new ExpressionMappingInfo();
                map.Expression = expressionText;
                exprNode.Tag = map;
                exprNode.ContextMenuStrip = _mapExprContextMenu;
                _node.Nodes.Add(exprNode);
                _node.Expand();
                _conv[exprNode.Name] = new PropertyConversionNodeDecorator(exprNode);
                exprNode.ExpandAll();
            }
        }

        public void MapExpression(string srcAlias, string destProperty, bool createIfNotExists)
        {
            var dstCls = Parent.TargetClass;
            TreeNode exprNode = _node.Nodes[srcAlias];
            ExpressionMappingInfo map = (ExpressionMappingInfo)exprNode.Tag;
            if (destProperty != null)
            {
                if (dstCls != null)
                {
                    PropertyDefinition dst = dstCls.Properties[destProperty];
                    DataPropertyDefinition dp = dst as DataPropertyDefinition;

                    if (string.IsNullOrEmpty(map.Expression))
                        throw new MappingException("Cannot map alias. There is no expression defined");

                    Expression expr = null;
                    try
                    {
                        expr = Expression.Parse(map.Expression);
                    }
                    catch (OSGeo.FDO.Common.Exception ex)
                    {
                        throw new MappingException("Cannot map alias. Invalid expression: " + ex.Message);
                    }

                    if (typeof(Function).IsAssignableFrom(expr.GetType()))
                    {
                        Function func = expr as Function;
                        FdoConnection conn = Parent.GetSourceConnection();
                        FunctionDefinitionCollection funcDefs = (FunctionDefinitionCollection)conn.Capability.GetObjectCapability(CapabilityType.FdoCapabilityType_ExpressionFunctions);
                        FunctionDefinition funcDef = null;

                        //Shouldn't happen because Expression Editor ensures a valid function
                        if (!funcDefs.Contains(func.Name))
                            throw new MappingException("Cannot map alias. Expression contains unsupported function: " + func.Name);

                        //Try to get the return type
                        foreach (FunctionDefinition fd in funcDefs)
                        {
                            if (fd.Name == func.Name)
                            {
                                funcDef = fd;
                                break;
                            }
                        }

                        if (funcDef.ReturnPropertyType != dst.PropertyType)
                        {
                            throw new MappingException("Cannot map alias. Expression evaluates to an un-mappable property type");
                        }

                        if (funcDef.ReturnPropertyType == PropertyType.PropertyType_GeometricProperty)
                        {

                        }
                        else if (funcDef.ReturnPropertyType == PropertyType.PropertyType_DataProperty)
                        {
                            if (!ValueConverter.IsConvertible(funcDef.ReturnType, dp.DataType))
                                throw new MappingException("Cannot map alias to property " + dst.Name + ". Expression evaluates to a data type that cannot be mapped to " + dp.DataType);
                        }
                        else //Association, Object, Raster
                        {
                            throw new MappingException("Cannot map alias. Expression evaluates to an un-mappable property type");
                        }
                    }
                    else if (typeof(BinaryExpression).IsAssignableFrom(expr.GetType()))
                    {
                        if (dp == null)
                            throw new MappingException("Cannot map alias. Expression evaluates to an un-mappable property type");

                        //We're assuming that this evalutes to a boolean value
                        if (!ValueConverter.IsConvertible(DataType.DataType_Boolean, dp.DataType))
                            throw new MappingException("Cannot map alias to property " + dst.Name + ". Expression evaluates to a data type that cannot be mapped to " + dp.DataType);
                    }
                    else if (typeof(DataValue).IsAssignableFrom(expr.GetType()))
                    {
                        if (dp == null)
                            throw new MappingException("Cannot map alias. Expression evaluates to an un-mappable property type");

                        DataValue dv = (DataValue)expr;
                        if (!ValueConverter.IsConvertible(dv.DataType, dp.DataType))
                            throw new MappingException("Cannot map alias to property " + dst.Name + ". Expression evaluates to a data type that cannot be mapped to " + dp.DataType);
                    }
                    //else if (expr.GetType() == typeof(Identifier))
                    //{
                    //    //TODO: use the property type of the referenced property
                    //}
                    else //Cannot be evaluated
                    {
                        throw new MappingException("Cannot map alias. Expression evaluates to an un-mappable value");
                    }
                }
                else
                {
                    if (!createIfNotExists)
                        throw new MappingException("Cannot map alias. The specified target property " + destProperty + " does not exist and the \"create if necessary\" option was not specified");
                }
            }
            //All good
            exprNode.Text = srcAlias + " ( => " + destProperty + " )";
            map.TargetProperty = destProperty;
            if (destProperty != null)
                exprNode.Text = exprNode.Name + " => " + destProperty;
            else
                exprNode.Text = exprNode.Name;
        }

        public IEnumerable<string> GetAliases()
        {
            foreach (TreeNode expNode in _node.Nodes)
            {
                yield return expNode.Name;
            }
        }

        public ExpressionMappingInfo GetMapping(string alias)
        {
            if (_node.Nodes[alias].Tag != null)
            {
                return _node.Nodes[alias].Tag as ExpressionMappingInfo;
            }
            return null;
        }

        public void RemoveAliasMapping(string alias)
        {
            MapExpression(alias, null, false);
        }

        public void RemoveExpression(string alias)
        {
            _node.Nodes.RemoveByKey(alias);
        }

        internal PropertyConversionNodeDecorator GetConversionRule(string alias)
        {
            return _conv[alias];   
        }
    }
}
