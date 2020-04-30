using System;

namespace Taskbar.FlashKiller
{
    public class EventArgs<TData> : EventArgs
    {
        public EventArgs(TData data)
        {
            this.Data = data;
        }

        public TData Data
        {
            get;
        }
    }
}
