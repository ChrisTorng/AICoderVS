using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AICoderVS
{
    [Export(typeof(AICodeSuggestionProvider))]
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class AICodeSuggestionProvider : IWpfTextViewCreationListener
    {
        private ToolTip _tooltipControl;
        private IWpfTextView _activeTextView;

        public void TextViewCreated(IWpfTextView textView)
        {
            _activeTextView = textView;
            // Initialize code
            InitializeAICodeSuggestion(textView);
        }

        private void InitializeAICodeSuggestion(IWpfTextView textView)
        {
            // Initialize tooltip control
            _tooltipControl = new ToolTip();

            // Modify subscription method
            textView.TextBuffer.Changed += (sender, e) => Task.Run(() => TextBuffer_ChangedAsync(sender, (TextContentChangedEventArgs)e));
        }

        private async Task TextBuffer_ChangedAsync(object sender, TextContentChangedEventArgs e)
        {
            // Show AI suggestion when text changes
            await ShowAISuggestionTooltipAsync();
        }

        private string GetCurrentLineText()
        {
            var textView = GetActiveTextView();
            if (textView == null) return string.Empty;

            var caretPosition = textView.Caret.Position.BufferPosition;
            var line = caretPosition.GetContainingLine();
            return line.GetText();
        }

        private async Task ShowAISuggestionTooltipAsync()
        {
            var currentLineText = GetCurrentLineText();
            var caretPosition = GetActiveTextView().Caret.Position.BufferPosition;

            // Invoke AI service to get suggestion
            var aiSuggestion = await GetAISuggestionAsync(currentLineText, caretPosition);

            // Use Dispatcher to update UI on the UI thread
            await _activeTextView.VisualElement.Dispatcher.InvokeAsync(() =>
            {
                if (!string.IsNullOrEmpty(aiSuggestion))
                {
                    _tooltipControl.Content = new TextBlock { Text = aiSuggestion };
                    _tooltipControl.IsOpen = true;
                }
                else
                {
                    _tooltipControl.IsOpen = false;
                }
            });
        }

        private async Task<string> GetAISuggestionAsync(string currentLineText, SnapshotPoint caretPosition)
        {
            // TODO: Implement actual communication logic with AI service
            await Task.Delay(100); // Simulate asynchronous operation
            return "AI Suggestion: " + currentLineText;
        }

        public void AcceptAISuggestion()
        {
            var textView = GetActiveTextView();
            if (textView == null) return;

            var caretPosition = textView.Caret.Position.BufferPosition;
            var line = caretPosition.GetContainingLine();

            // Get the AI suggestion from the tooltip
            var aiSuggestion = (_tooltipControl.Content as TextBlock)?.Text;
            if (string.IsNullOrEmpty(aiSuggestion)) return;

            // Replace the entire line with the AI suggestion
            using (var edit = textView.TextBuffer.CreateEdit())
            {
                edit.Replace(line.Start, line.Length, aiSuggestion);
                edit.Apply();
            }

            // Move the caret to the end of the new line
            textView.Caret.MoveTo(new SnapshotPoint(textView.TextBuffer.CurrentSnapshot, line.Start.Position + aiSuggestion.Length));

            _tooltipControl.IsOpen = false;
        }

        private IWpfTextView GetActiveTextView()
        {
            return _activeTextView;
        }
    }
}