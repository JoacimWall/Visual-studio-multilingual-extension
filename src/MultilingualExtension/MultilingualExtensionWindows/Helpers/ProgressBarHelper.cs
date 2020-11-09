using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilingualExtensionWindows.Helpers
{
    public class ProgressBarHelper : MultilingualExtension.Shared.Interface.IProgressBar
    {
        public bool HideAll()
        {
            return true;
        }

        public bool Init(string InfoText)
        {
            return true;
        }

        public bool Pulse()
        {
            // throw new NotImplementedException();
            return true;
        }
    }
}
