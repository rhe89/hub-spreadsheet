using System;

namespace Spreadsheet.Web.WebApp
{
    public class State
    {
        public State()
        {
            
        }
        public event Action OnChange;

        public bool Saving { get; private set; }
        
        public void SetSaving(bool saving)
        {
            Saving = saving;
            
            OnChange?.Invoke();
        }
    }
}