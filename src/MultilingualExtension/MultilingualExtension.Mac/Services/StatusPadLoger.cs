using System;
using MultilingualExtension.Shared.Interfaces;

namespace MultilingualExtension.Services
{
    public class StatusPadLoger : IStatusPadLoger
    {
        public StatusPadLoger()
        {
        }

        public void WriteText(string logtext)
        {
            MultilingualExtension.StatusPad.WriteText(logtext);
        }
    }
}

