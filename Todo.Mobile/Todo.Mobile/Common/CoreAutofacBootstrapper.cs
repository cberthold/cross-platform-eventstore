using Autofac;
using Infrastructure.Common;
using System.Linq;
using System.Reflection;

namespace Todo.Mobile.Common
{
    public abstract class CoreAutofacBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            ConfigureContainerInternal(builder);

            var container = builder.Build();




            var viewFactory = container.Resolve<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        public CoreAutofacBootstrapper() { }
        public CoreAutofacBootstrapper(Assembly[] autoRegisterAssemblies)
        {
            AutoRegisterAssemblies = autoRegisterAssemblies;
        }

        private void ConfigureContainerInternal(ContainerBuilder builder)
        {   
            if (AutoRegisterAssemblies != null)
            {
                builder.RegisterAssemblyModules(AutoRegisterAssemblies);
                builder.RegisterAssemblyTypes(AutoRegisterAssemblies).Where(t => t.Namespace != null && t.Namespace.EndsWith(".ViewModels") && t.Name.EndsWith("VM")).AsSelf().InstancePerDependency();
                builder.RegisterAssemblyTypes(AutoRegisterAssemblies).Where(t => t.Namespace != null && t.Namespace.EndsWith(".Views") && t.Name.EndsWith("View")).AsSelf().InstancePerDependency();
            }

            // add service locator
            builder.RegisterType<AutofacServiceLocator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            ConfigureContainer(builder);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            // to be overridden by implementing class
        }

        protected virtual void RegisterViews(IViewFactory viewFactory)
        {
            if (AutoRegisterAssemblies == null)
                return;

            foreach (var assembly in AutoRegisterAssemblies)
            {
                var viewModels = assembly.DefinedTypes.Where(t => t.Namespace != null && t.Namespace.EndsWith(".ViewModels") && t.Name.EndsWith("VM"));
                var views = assembly.DefinedTypes.Where(t => t.Namespace != null && t.Namespace.EndsWith(".Views") && t.Name.EndsWith("View"));

                foreach (var vm in viewModels)
                {
                    var pairedViews = views.Where(v => v.Name.Substring(0, v.Name.Length - 4) == vm.Name.Substring(0, vm.Name.Length - 2));
                    if (pairedViews.Count() == 1)
                    {
                        var vmt = vm.AsType();
                        viewFactory.Register(vm.AsType(), pairedViews.First().AsType());
                    }
                }
            }


        }

        protected abstract void ConfigureApplication(IContainer container);

        protected Assembly[] AutoRegisterAssemblies { get; private set; }

    }
}

