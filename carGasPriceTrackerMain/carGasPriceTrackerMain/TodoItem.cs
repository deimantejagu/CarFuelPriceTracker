using System;
using System.Collections.Generic;
using System.Text;

namespace carGasPriceTrackerMain
{
    public class TodoItem
    {
        public String TodoText { get; set; }
        public bool Complete { get; set; }

        public TodoItem(String TodoText, bool Complete)
        {
            this.TodoText = TodoText;
            this.Complete = Complete;
        }
    }
}
