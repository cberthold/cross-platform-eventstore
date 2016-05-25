using System;
using System.ComponentModel;

namespace Todo.Mobile.Common
{
	public interface IViewModel : INotifyPropertyChanged
	{
		string Title { get; set; }

		void SetState<T>(Action<T> action) where T : class, IViewModel;
		void OnAppearing ();
		void OnDisappearing ();
		void OnPushed();
		void OnPopped();
	}
}

