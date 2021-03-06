﻿using System.Windows;
using System.Windows.Controls;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public abstract DataTemplate SelectTemplate(object item, DependencyObject container);

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}
