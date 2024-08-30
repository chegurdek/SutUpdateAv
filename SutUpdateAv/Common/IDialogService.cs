using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutUpdateAv.Common
{
    public interface IDialogService
    {
        Task<ButtonResult> ShowBox(MessageBoxStandardParams MBSparams, Window owner = null);

        ButtonResult ShowBoxNoAsync(MessageBoxStandardParams MBSparams, Window owner = null);

        ButtonResult ShowMessageBoxNoAsync(string message, Window owner = null);

        ButtonResult ShowMessageBoxNoAsync(MessageBoxStandardParams mbsParams, Window owner = null);

        Task<ButtonResult> ShowErrorBox(string message, Window owner = null);

    }
}
