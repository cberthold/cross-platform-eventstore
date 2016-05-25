using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Todo.Mobile.Common.Behaviors
{
    public static class ListViewItemTappedCommandBehavior
    {
        public static readonly BindableProperty CommandProperty =
        BindableProperty.CreateAttached(
            "Command",
            typeof(ICommand),
            typeof(ListViewItemTappedCommandBehavior),
            null,
            propertyChanged: OnCommandChanged);

        static void OnCommandChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as ListView;
            if (entry == null)
                return;

            entry.ItemTapped += (sender, e) =>
            {
                var command = (newValue as ICommand);
                if (command == null)
                    return;

                if (command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }

            };
        }
    }
}
