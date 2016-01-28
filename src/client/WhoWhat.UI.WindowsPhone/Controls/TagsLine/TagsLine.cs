using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    [TemplatePart(Name = PART_WrapPanel, Type = typeof(RadWrapPanel))]
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_ScrollViewer, Type = typeof(ScrollViewer))]
    public class TagsLine : Control
    {
        public const string PART_WrapPanel = "PART_WrapPanel";
        public const string PART_TextBox = "PART_TextBox";
        public const string PART_ScrollViewer = "PART_ScrollViewer";

        private RadWrapPanel wrapPanel;
        private TextBox textBox;
        private ScrollViewer scrollViewer;

        public TagsLine()
        {
            DefaultStyleKey = typeof(TagsLine);
        }

        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(List<string>), typeof(TagsLine), new PropertyMetadata(default(List<string>), (
                o, args) =>
                {
                    var @this = (TagsLine) o;
                    List<string> tags = (List<string>) args.NewValue;
                    @this.UpdateTags(tags);
                }));

        private void UpdateTags(IEnumerable<string> tags)
        {
            if (wrapPanel == null)
            {
                return;
            }

            var buttons = wrapPanel.ChildrenOfType<Button>().ToList();
            foreach (Button button in buttons)
            {
                wrapPanel.Children.Remove(button);
            }

            foreach (string tag in tags)
            {
                AddValueButton(tag);
            }
        }

        public List<string> Tags
        {
            get { return (List<string>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        public static readonly DependencyProperty ValueButtonStyleProperty =
           DependencyProperty.Register(
           "ValueButtonStyle",
           typeof(Style),
           typeof(TagsLine), null
        );

        public Style ValueButtonStyle
        {
            get { return (Style)GetValue(ValueButtonStyleProperty); }
            set { SetValue(ValueButtonStyleProperty, value); }
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (textBox != null)
            {
                textBox.Focus();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            wrapPanel = (RadWrapPanel)GetTemplateChild(PART_WrapPanel);
            textBox = (TextBox)GetTemplateChild(PART_TextBox);
            scrollViewer = (ScrollViewer)GetTemplateChild(PART_ScrollViewer);

            UpdateTags(this.Tags);

            textBox.KeyDown += (s, e) =>
            {
                //On Enter or Tab
                if (((e.Key == Key.Enter) || (e.Key == Key.Tab)) && !string.IsNullOrWhiteSpace(this.textBox.Text))
                {
                    AddEntryIfValid();
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Back && string.IsNullOrEmpty(textBox.Text))
                {
                    //Remove button
                    Button button = (Button)wrapPanel.Children.LastOrDefault(x => x is Button);
                    if (button != null)
                    {
                        Tags.Remove((string)button.Content);
                        wrapPanel.Children.Remove(button);
                    }
                }
            };

            textBox.LostFocus += (s, e) => AddEntryIfValid();

            this.Tap += (s, e) => textBox.Focus();
        }

        private void AddEntryIfValid()
        {
            string entry = textBox.Text.Trim();

            if (!string.IsNullOrEmpty(entry))
            {
                AddValueButton(entry);
                textBox.Text = string.Empty;
                Tags.Add(entry);
                ScrollBottom();
            }
        }

        private void ScrollBottom()
        {
            scrollViewer.UpdateLayout();
            scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight);
        }


        /// <summary>
        /// Add a new selected value
        /// </summary>
        private void AddValueButton(string text)
        {
            //Create an item button
            Button button = new Button();
            button.Style = ValueButtonStyle;
            button.Content = text;

            RadContextMenu menu = new RadContextMenu();
            menu.IsZoomEnabled = false;

            RadContextMenu.SetContextMenu(button, menu);
            RadContextMenuItem miRemove = new RadContextMenuItem()
            {
                Content = "Remove"
            };

            menu.Items.Add(miRemove);

            RadContextMenuItem miEdit = new RadContextMenuItem()
            {
                Content = "Edit"
            };

            menu.Items.Add(miEdit);

            menu.ItemTapped += (s, e) =>
            {
                if (e.VisualContainer == miEdit)
                {
                    textBox.Text = (string)button.Content;
                    textBox.Focus();
                }

                wrapPanel.Children.Remove(button);
            };

            button.Tap += (s, e) =>
            {
                menu.IsOpen = true;
                e.Handled = true;
            };

            //Insert before the TextBox
            wrapPanel.Children.Insert(wrapPanel.Children.Count - 1, button);
        }

    }
}
