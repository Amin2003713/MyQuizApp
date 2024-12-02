namespace MyQuizApp.Shared.Components.Common.Dialogs.Common;

public record ModalResult<TData>(TData Data, bool IsSuccess);
