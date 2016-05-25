using Infrastructure.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Mobile.Infrastructure.Common
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetAsyncConnection(Database database);
        SQLiteConnection GetConnection(Database database);
        void ResetReadDatabase();
        void ResetWriteDatabase();
    }
}
