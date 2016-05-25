using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.BoundedContext.Domain;
using Todo.Mobile.Common;
using Xamarin.Forms;
using CommandDispatcher = Infrastructure.Command.CommandDispatcher;
using static Todo.Mobile.NavigationMessage;

namespace Todo.Mobile.ViewModels
{
    public class TodoListVM : ViewModelBase
    {

        public ICommand AddTodoItem { get; private set; }
        public ICommand SelectedItemTapped { get; private set; }

        private TodoListAggregate list;
        public TodoListAggregate List
        {
            get { return list; }
            set
            {
                list = value;
                OnPropertyChanged();
            }
        }

        private TodoItemEntity selectedItem;
        public TodoItemEntity SelectedTodoItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                OnPropertyChanged();

            }
        }


        public readonly static Guid TENANT_ID = Guid.Parse("0F51BC57-ADEC-4C00-B34D-0D2C1FF7736C");
        readonly IRepository repository;
        readonly INavigator navigator;
        readonly IViewFactory viewFactory;
        readonly CommandDispatcher dispatcher;

        public TodoListVM(IViewFactory viewFactory, IRepository repository, INavigator navigator, CommandDispatcher dispatcher)
        {
            this.viewFactory = viewFactory;
            this.repository = repository;
            this.navigator = navigator;
            this.dispatcher = dispatcher;

            AddTodoItem = new Command((nothing) =>
            {
                // go to the todo item
                navigator.PushAsync<AddTodoItemVM>((vm) =>
                {
                    // add a new one
                    vm.SetNewTodoItem();
                });
            });

            SelectedItemTapped = new Command(
                (nothing) =>
                {
                    navigator.PushAsync<UpdateTodoItemVM>((vm) =>
                    {
                        vm.SetTodoItem(selectedItem);
                    });
                },
                (nothing) =>
                {
                    return selectedItem != null;
                });

        }

        private void SubscribeFromMessages()
        {
            Subscribe(this, EventTypes.TodoListUpdated, (message) =>
            {
                UpdateTodoList();
            });

        }

        private void UnsubscribeFromMessages()
        {
            Unsubscribe(this, EventTypes.TodoListUpdated);
        }

        public override void OnDisappearing()
        {
            UnsubscribeFromMessages();
            base.OnDisappearing();
        }

        public override void OnAppearing()
        {
            UpdateTodoList();
            SubscribeFromMessages();
            base.OnAppearing();
        }

        private void UpdateTodoList()
        {
            List = GetList();
        }


        private TodoListAggregate GetList()
        {
            try
            {
                return repository.Get<TodoListAggregate>(TENANT_ID);
            }
            catch
            {
                return TodoListAggregate.Create(TENANT_ID);
            }
        }

    }
}
