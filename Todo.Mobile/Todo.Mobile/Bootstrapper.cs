using Autofac;
using Infrastructure.Bus;
using Infrastructure.Command;
using Infrastructure.Domain;
using Infrastructure.Domain.Factories;
using Infrastructure.Events;
using Infrastructure.Repository;
using Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Handlers;
using Todo.Mobile.Common;
using Todo.Mobile.Infrastructure;
using Todo.Mobile.Infrastructure.Common;
using Todo.Mobile.Infrastructure.EventStore;
using Todo.Mobile.ViewModels;
using Todo.Mobile.Views;
using Application = Xamarin.Forms.Application;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Todo.Mobile
{
    public class Bootstrapper : CoreAutofacBootstrapper
    {
        public Bootstrapper(Application app, params Assembly[] autoRegisterAssemblies) 
            : base(autoRegisterAssemblies)
        {
            App = app;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register<ISQLite>((ctx) =>
            {
                return DependencyService.Get<ISQLite>();
            });
            

            builder.RegisterType<AutofacHandlerResolver>()
                .AsSelf();

            builder.RegisterType<MobileHandlerResolver>()
                .AsImplementedInterfaces();

            builder.RegisterType<InProcessBus>()
                .AsSelf()
                .As<ICommandSender>()
                .As<IEventPublisher>()
                .SingleInstance();

            builder.RegisterType<SqliteEventStore>()
                .As<IEventStore>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<Session>()
                .As<ISession>()
                .InstancePerLifetimeScope();

            builder.Register((ctx) =>
            {
                var eventStore = ctx.Resolve<IEventStore>();
                var publisher = ctx.Resolve<IEventPublisher>();
                var aggregateFactory = DependencyService.Get<IAggregateFactory>();

                //return new CacheRepository(new Repository(eventStore, publisher), eventStore);
                return new Repository(eventStore, publisher, aggregateFactory);
            })
            .As<IRepository>()
            .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(assembliesToCheck)
            //   .AsClosedTypesOf(typeof(IValidator<>))
            //   .InstancePerLifetimeScope();

            builder.RegisterType<ValidationFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandDispatcher>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AutoRegisterAssemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AutoRegisterAssemblies)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(SQLiteReadRepository<>))
                .As(typeof(IReadRepository<>))
                .InstancePerLifetimeScope();
            
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            //viewFactory.Register<MainPageVM, MainPageView>();
            viewFactory.Register<TodoListVM, TodoListView>();
            viewFactory.Register<AddTodoItemVM, AddTodoItemView>();
            viewFactory.Register<UpdateTodoItemVM, UpdateTodoItemView>();
            var mainPage = viewFactory.Resolve<TodoListVM>();
            var navigationPage = new NavigationPage(mainPage);
            

            App.MainPage = navigationPage;
        }

        private readonly Application App;
    }
}
