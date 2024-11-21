using System;
namespace MultilingualClient.Services
{
	public class StatusPadLoger : IStatusPadLoger
    {
        public Label Logger;
		public StatusPadLoger(Label logger)
		{
            Logger = logger;

        }
        
        public void WriteText(string logtext)
        {
            Logger.Text = logtext + Environment.NewLine + Logger.Text;
        }
    }
}

