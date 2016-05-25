using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Mobile.Common;

namespace Todo.Mobile.Model
{
    public class TodoItem : ModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set { SetProperty(ref _completed, value); }
        }
    }
}
