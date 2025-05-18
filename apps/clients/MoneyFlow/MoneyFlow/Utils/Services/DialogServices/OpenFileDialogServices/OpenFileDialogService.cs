﻿using Microsoft.Win32;
using System.IO;

namespace MoneyFlow.Utils.Services.DialogServices.OpenFileDialogServices
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        public string[] OpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Multiselect = true;
            dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileNames;
            }
            else
            {
                return null;
            }
        }
    }
}
