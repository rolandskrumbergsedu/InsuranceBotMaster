using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.Technical
{
    public interface IExceptionDialog<T> : IDialog<object>
    {
        void SetDialog(IDialog<T> dialog);
    }
}