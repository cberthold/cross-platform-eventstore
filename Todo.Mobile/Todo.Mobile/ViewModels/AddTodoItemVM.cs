using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.Mobile.Common;
using Xamarin.Forms;
using INavigator = Todo.Mobile.Common.INavigator;
using AddTodoItem = Todo.BoundedContext.Commands.AddTodoItem;
using CommandDispatcher = Infrastructure.Command.CommandDispatcher;
using static Todo.Mobile.NavigationMessage;
using Todo.Mobile.Model;

namespace Todo.Mobile.ViewModels
{
    public class AddTodoItemVM : ViewModelBase
    {

        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private TodoItem _item;
        public TodoItem Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
            

        readonly INavigator navigator;
        readonly CommandDispatcher dispatcher;
        

        public void SetNewTodoItem()
        {
            Item = new TodoItem()
            {
                Completed = false,
                Title = string.Empty,
            };
        }

        public AddTodoItemVM(INavigator navigator, CommandDispatcher dispatcher)
        {
            this.navigator = navigator;
            this.dispatcher = dispatcher;

            SetNewTodoItem();
            
            CancelCommand = new Command((nothing) =>
            {
                navigator.PopAsync();
            });
            
            SaveCommand = new Command((nothing) =>
            {
                try
                {
                    var cmd = new AddTodoItem()
                    {
                        Completed = Item.Completed,
                        ItemId = Guid.NewGuid(),
                        TenantId = TodoListVM.TENANT_ID,
                        Title = Item.Title,
                    };
                    dispatcher.Send(cmd);
                    Send(EventTypes.TodoListUpdated);
                    navigator.PopAsync();
                }
                catch(Exception ex) {
                    var ex1 = ex;
                }
            });
        }
    }
}
