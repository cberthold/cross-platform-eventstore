using Infrastructure.Command;
using Infrastructure.Domain;
using Infrastructure.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Todo.BoundedContext.Commands;
using Todo.BoundedContext.Domain;
using Todo.Mobile.Common;
using Todo.Mobile.ViewModels;
using Xamarin.Forms;

namespace Todo.Mobile
{
    public class App : Application
    {
        public App(params Assembly[] assemblies)
        {
            var list = new List<Assembly>()
            {
                Assembly.Load(new AssemblyName("Todo.Mobile")),
                //Assembly.Load(new AssemblyName("Todo.BoundedContext.Projection")),
                Assembly.Load(new AssemblyName("Todo.BoundedContext")),
            };

            if (assemblies != null)
                list.AddRange(assemblies);
            var bootstrapper = new Bootstrapper(this, list.ToArray());
            bootstrapper.Run();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    
}
