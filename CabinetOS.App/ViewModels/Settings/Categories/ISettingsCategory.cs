using System;
using System.Collections.Generic;
using System.Text;


    namespace CabinetOS.App.ViewModels.Settings.Categories
    {
        public interface ISettingsCategory
        {
            string DisplayName { get; }
            object View { get; }

            void Load();
            void Save();
            void ResetToDefaults();
        }
    }

