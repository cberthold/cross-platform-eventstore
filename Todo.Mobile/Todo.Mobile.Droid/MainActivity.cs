using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Todo.Mobile.Infrastructure.Common;
using Todo.BoundedContext.Commands;
using Todo.BoundedContext.Projections;

namespace Todo.Mobile.Droid
{
    [Activity(Label = "Todo.Mobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(
                typeof(App).Assembly,
                typeof(AddTodoItem).Assembly,
                typeof(TodoListDTOHandlers).Assembly)
                );
        }
    }
}

