﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public class BindingUtility
    {
        public static bool GetUpdateSourceOnChange(DependencyObject d)
        {
            return (bool)d.GetValue(UpdateSourceOnChangeProperty);
        }

        public static void SetUpdateSourceOnChange(DependencyObject d, bool value)
        {
            d.SetValue(UpdateSourceOnChangeProperty, value);
        }

        public static readonly DependencyProperty UpdateSourceOnChangeProperty = DependencyProperty.RegisterAttached(
            "UpdateSourceOnChange", typeof(bool), typeof(BindingUtility),new PropertyMetadata(false, OnPropertyChanged)
        );

        private static void OnPropertyChanged(DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;

            if (passwordBox != null)
            {
                if ((bool)e.NewValue)
                {
                    passwordBox.KeyUp += OnKeyUp;
                }
                else
                {
                    passwordBox.KeyUp -= OnKeyUp;
                }
            }

            var textBox = d as TextBox;
            if (textBox != null)
            {

                if ((bool)e.NewValue)
                {
                    textBox.TextChanged += OnTextChanged;
                }
                else
                {
                    textBox.TextChanged -= OnTextChanged;
                }
            }
        }

        private static void OnKeyUp(object sender, KeyEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            var bindingExpression = passwordBox.GetBindingExpression(PasswordBox.PasswordProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }

        }

        static void OnTextChanged(object s, TextChangedEventArgs e)
        {
            var textBox = s as TextBox;
            if (textBox == null)
                return;

            var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
    }
}
