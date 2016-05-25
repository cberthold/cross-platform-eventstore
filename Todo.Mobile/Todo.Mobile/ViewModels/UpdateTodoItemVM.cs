using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.Mobile.Common;
using Xamarin.Forms;
using INavigator = Todo.Mobile.Common.INavigator;
using UpdateTodoItem = Todo.BoundedContext.Commands.UpdateTodoItem;
using CommandDispatcher = Infrastructure.Command.CommandDispatcher;
using Todo.BoundedContext.Domain;

namespace Todo.Mobile.ViewModels
{
    public class UpdateTodoItemVM : ViewModelBase
    {

        public ICommand CancelCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private UpdateTodoItem item;
        public UpdateTodoItem Item {
        get { return item; }
        set
            {
                item = value;
                OnPropertyChanged();
            }
        }

        readonly INavigator navigator;
        readonly CommandDispatcher dispatcher;
        

        public void SetTodoItem(TodoItemEntity todoItem)
        {
            Item = new UpdateTodoItem()
            {
                Completed = todoItem.Completed,
                ExpectedVersion = 0,
                ItemId = todoItem.ItemId,
                TenantId = TodoListVM.TENANT_ID,
                Title = todoItem.Title,
            };
        }

        public UpdateTodoItemVM(INavigator navigator, CommandDispatcher dispatcher)
        {
            this.navigator = navigator;
            this.dispatcher = dispatcher;
            
            CancelCommand = new Command((nothing) =>
            {
                navigator.PopAsync();
            });

            DeleteCommand = new Command((nothing) =>
            {
                var cmd = new BoundedContext.Commands.RemoveTodoItem()
                {
                    TenantId = TodoListVM.TENANT_ID,
                    ItemId = Item.ItemId
                };

                try
                {
                    dispatcher.Send(cmd);
                    NavigationMessage.Send(EventTypes.TodoListUpdated);
                    navigator.PopAsync();
                }
                catch { }
            });
            
            SaveCommand = new Command((nothing) =>
            {
                try
                {
                    dispatcher.Send(Item);
                    NavigationMessage.Send(EventTypes.TodoListUpdated);
                    navigator.PopAsync();
                }
                catch(Exception ex) {
                    var ex1 = ex;
                }
            });
        }

        
    }
}
