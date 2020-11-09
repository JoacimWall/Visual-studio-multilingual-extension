using System;
namespace MultilingualExtension.Shared.Helpers
{
    public class Result<T>
    {
        public Result(T model)
        {
            Model = model;
            WasSuccessful = true;
        }
        
        public Result(bool wasSuccessful)
        {

            WasSuccessful = wasSuccessful;
        }
        public Result(string error)
        {
            ErrorMessage = error;
            WasSuccessful = false;
        }
        public T Model { get; set; }

       

        public bool WasSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}
