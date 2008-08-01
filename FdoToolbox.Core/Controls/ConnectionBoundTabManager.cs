#region LGPL Header
// Copyright (C) 2008, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace FdoToolbox.Core.Controls
{
    public class ConnectionBoundTabManager : IConnectionBoundTabManager
    {
        private List<Type> _ControlTypes;
        private Dictionary<Type, List<IConnectionBoundCtl>> _ControlInstances;
        
        public ConnectionBoundTabManager()
        {
            _ControlInstances = new Dictionary<Type, List<IConnectionBoundCtl>>();
            _ControlTypes = new List<Type>();
            HostApplication.Instance.SpatialConnectionManager.ConnectionRenamed += new ConnectionRenamedEventHandler(ConnectionManager_ConnectionRenamed);
            HostApplication.Instance.SpatialConnectionManager.BeforeConnectionRemove += new ConnectionBeforeRemoveHandler(ConnectionManager_BeforeConnectionRemove);
        }

        public void RemoveTab(IConnectionBoundCtl ctl)
        {
            bool removed = _ControlInstances[ctl.GetType()].Remove(ctl);
        }

        void ConnectionManager_ConnectionRenamed(string oldName, string newName)
        {
            foreach (Type t in _ControlTypes)
            {
                List<IConnectionBoundCtl> controls = _ControlInstances[t].FindAll(
                    delegate(IConnectionBoundCtl ctl)
                    {
                        return ctl.BoundConnection.Name == oldName;
                    }
                );
                controls.ForEach(
                    delegate(IConnectionBoundCtl ctl)
                    {
                        string newKey = GenerateKey(ctl.GetType(), newName);
                        ctl.SetName(newName);
                        ctl.SetKey(newKey);
                    }
                );
            }
        }

        void ConnectionManager_BeforeConnectionRemove(string name, ref bool cancel)
        {
            List<IConnectionBoundCtl> controls = new List<IConnectionBoundCtl>();
            foreach (Type type in _ControlInstances.Keys)
            {
                List<IConnectionBoundCtl> found = _ControlInstances[type].FindAll(
                    delegate(IConnectionBoundCtl ctl)
                    {
                        return ctl.BoundConnection.Name == name;
                    }
                );
                controls.AddRange(found);
            }
            if(controls.Count > 0)
                cancel = !AppConsole.Confirm("Tabs still open", "There are tabs still open which rely on the connection you are about to close.\nIf you close the connection they will be closed too.\n\nClose connection?");
        }

        public IConnectionBoundCtl CreateTab(Type tabType, SpatialConnectionInfo connInfo)
        {
            IConnectionBoundCtl control = null;
            if (!_ControlTypes.Contains(tabType))
            {
                throw new ArgumentException("Tab type " + tabType + " was not registered");
            }
            string key = GenerateKey(tabType, connInfo.Name);
            control = _ControlInstances[tabType].Find(delegate(IConnectionBoundCtl ctl) { return ctl.GetKey() == key; });
            if (control == null)
            {
                ISpatialConnectionMgr connMgr = HostApplication.Instance.SpatialConnectionManager;
                
                //We're expecting a constructor with the following signature:
                // (ConnectionInfo, string)
                control = Activator.CreateInstance(tabType, connInfo, key) as IConnectionBoundCtl;
                if (control == null)
                    throw new Exception("Failed to create tab of type " + tabType);

                string name = control.BoundConnection.Name;
                control.SetName(name);
                control.SetKey(key);
                ConnectionEventHandler removeHandler = new ConnectionEventHandler(delegate(string connName)
                {
                    if (control.BoundConnection.Name == connName)
                        control.WrappedControl.Close();
                });

                connMgr.ConnectionRemoved += removeHandler;
                control.WrappedControl.Disposed += delegate
                {
                    connMgr.ConnectionRemoved -= removeHandler;
                    RemoveTab(control);
                };

                _ControlInstances[tabType].Add(control);
            }
            return control;
        }

        public void RegisterTabType(Type tabType)
        {
            if (Array.IndexOf<Type>(tabType.GetInterfaces(), typeof(IConnectionBoundCtl)) < 0)
                throw new ArgumentException("The given type is not of type IConnectionBoundCtl");

            _ControlTypes.Add(tabType);
            _ControlInstances[tabType] = new List<IConnectionBoundCtl>();
        }

        public string GenerateKey(Type t, string connName)
        {
            return (t.ToString() + connName).GetHashCode().ToString();
        }
    }
}
