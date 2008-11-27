using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.Core;
using System.Resources;
using System.Reflection;
using FdoToolbox.Base.Services;

using Res = ICSharpCode.Core.ResourceService;
using Msg = ICSharpCode.Core.MessageService;
using FdoToolbox.Core;
using FdoToolbox.Base.Controls;

namespace FdoToolbox.Base.Commands
{
    public class StartupCommand : AbstractCommand
    {
        public override void Run()
        {
            EventWatcher.Initialize();
            ServiceManager svcMgr = ServiceManager.Instance;
            
            Res.RegisterNeutralStrings(FdoToolbox.Base.Strings.ResourceManager);
            Res.RegisterNeutralImages(FdoToolbox.Base.Images.ResourceManager);

            Workbench.WorkbenchInitialized += delegate
            {
                Workbench wb = Workbench.Instance;
                List<IObjectExplorerDecorator> decorators = AddInTree.BuildItems<IObjectExplorerDecorator>("/ObjectExplorer/Decorators", this);
                if (decorators != null)
                {
                    foreach (IObjectExplorerDecorator dec in decorators)
                    {
                        dec.Decorate(wb.ObjectExplorer);
                    }
                }

                svcMgr.RestoreSession();
                Msg.MainForm = wb;
                wb.SetTitle(Res.GetString("UI_TITLE"));

                wb.FormClosing += delegate
                {
                    svcMgr.UnloadAllServices();
                };
            };
        }
    }
}