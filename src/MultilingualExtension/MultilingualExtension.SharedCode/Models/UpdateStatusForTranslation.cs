using System;
namespace MultilingualExtension.Shared.Models
{
    public class UpdateStatusForTranslation
    {
        public string NodeName;
        public string NewStatus;

        public UpdateStatusForTranslation(string newStatus)
        {
           
            NewStatus = newStatus;


        }
    }
}
