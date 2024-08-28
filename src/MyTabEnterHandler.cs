using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace AICoderVS
{
    [Export(typeof(ICommandHandler))]
    [ContentType("text")]
    [Name("MyTabEnterHandler")]
    internal class MyTabEnterHandler : ICommandHandler<TabKeyCommandArgs>, ICommandHandler<ReturnKeyCommandArgs>
    {
        private readonly AICodeSuggestionProvider _aiCodeSuggestionProvider;

        [ImportingConstructor]
        public MyTabEnterHandler(AICodeSuggestionProvider aiCodeSuggestionProvider)
        {
            _aiCodeSuggestionProvider = aiCodeSuggestionProvider;
        }

        public string DisplayName => "My Tab and Enter Handler";

        public bool ExecuteCommand(TabKeyCommandArgs args, CommandExecutionContext executionContext)
        {
            MyLog.Log("Tab key handled by command");
            _aiCodeSuggestionProvider.AcceptAISuggestion();
            return true;
        }

        public bool ExecuteCommand(ReturnKeyCommandArgs args, CommandExecutionContext executionContext)
        {
            MyLog.Log("Enter key handled by command");
            return false;
        }

        public CommandState GetCommandState(TabKeyCommandArgs args)
        {
            return CommandState.Available;
        }

        public CommandState GetCommandState(ReturnKeyCommandArgs args)
        {
            return CommandState.Available;
        }
    }
}