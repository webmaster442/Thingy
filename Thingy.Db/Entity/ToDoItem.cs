using AppLib.WPF.MVVM;
using LiteDB;
using System;

namespace Thingy.Db.Entity
{
    public class ToDoItem: BindableBase
    {
        private string _content;
        private bool _iscompleted;
        private DateTime? _completeddate;
        private DateTime? _duedate;

        [BsonId]
        public string Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        [BsonField]
        public bool IsCompleted
        {
            get { return _iscompleted; }
            set
            {
                if (SetValue(ref _iscompleted, value))
                {
                    CompletedDate = DateTime.Now;
                }
            }
        }

        [BsonField]
        public DateTime? CompletedDate
        {
            get { return _completeddate; }
            set { SetValue(ref _completeddate, value); }
        }

        [BsonField]
        public DateTime? DueDate
        {
            get { return _duedate; }
            set { SetValue(ref _duedate, value); }
        }
    }
}
