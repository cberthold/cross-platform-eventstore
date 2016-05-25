using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Todo.Mobile
{
    public class NavigationMessage
    {
        public object Parameter { get; set; }

        public static void Subscribe(object subscriber, EventTypes type, Action<NavigationMessage> action)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            MessagingCenter.Subscribe(subscriber, type.ToString(), action);
        }

        public static void Unsubscribe(object subscriber, EventTypes type)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            MessagingCenter.Unsubscribe<NavigationMessage>(subscriber, type.ToString());
        }

        public static void Send(EventTypes type, object parameter = null)
        {
            var msg = new NavigationMessage()
            {
                Parameter = parameter,
            };

            MessagingCenter.Send(msg, type.ToString());
        }
    }
}
