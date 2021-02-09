using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Globalization;

namespace CRM.GetOrders
{
    public class NumericTextBox : TextBox
    {

        #region static dependency property

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture", typeof(CultureInfo), typeof(NumericTextBox), new PropertyMetadata(OnCultureChanged));


        private static void OnCultureChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBox _decBox = (NumericTextBox)sender;
            _decBox.Culture = (CultureInfo)e.NewValue;
            _decBox.pCulture = (CultureInfo)e.NewValue == null ? CultureInfo.CurrentCulture : (CultureInfo)e.NewValue;
        }

        #endregion static dependency property

        #region private fields

        private int pNumberOfDecimals = 2;
        private bool pAcceptDecimalKey = true;
        private CultureInfo pCulture;
        private bool pReplaceMode = true;

        #endregion private fields

        #region constructor

        public NumericTextBox()
            : base()
        {
            pCulture = CultureInfo.CurrentCulture;
            TextAlignment = TextAlignment.Right;
            TextWrapping = TextWrapping.NoWrap;
        }

        #endregion constructor

        #region public properties

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }

            set
            {
                SetValue(CultureProperty, value);
            }
        }

        public bool ReplaceMode
        {
            get { return pReplaceMode; }
            set { pReplaceMode = value; }
        }

        public int NumberOfDecimals
        {
            get { return pNumberOfDecimals; }
            set
            {
                pNumberOfDecimals = value;
                pAcceptDecimalKey = pNumberOfDecimals > 0;
            }
        }

        #endregion public properties

        #region private methods (check key and decimals)

        private bool pIsNumericKey(KeyEventArgs keyArgs)
        {
            bool _isNumeric = false;
            switch (keyArgs.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    _isNumeric = true;
                    break;
            }
            return _isNumeric;
        }

        private bool pIsSpecialKey(KeyEventArgs keyArgs)
        {
            bool _isSpecialKey = false;
            switch (keyArgs.Key)
            {
                case Key.Delete:
                case Key.Home:
                case Key.End:
                case Key.Back:
                case Key.Left:
                case Key.Right:
                case Key.Tab:
                    _isSpecialKey = true;
                    break;
            }
            return _isSpecialKey;
        }

        private bool pIsDecimalKey(KeyEventArgs keyArgs)
        {

            if (keyArgs.Key == Key.Decimal)
                return true;

            // 188 = ,(komma) och 190 = .(punkt)
            if (keyArgs.PlatformKeyCode == 188 && pCulture.NumberFormat.NumberDecimalSeparator == ",")
                return true;

            if (keyArgs.PlatformKeyCode == 190 && pCulture.NumberFormat.NumberDecimalSeparator == ".")
                return true;

            return false;
        }

        private bool pIsDecimalKeyUsed()
        {
            return Text.IndexOf(pCulture.NumberFormat.NumberDecimalSeparator) > -1;
        }

        private bool pIsDecimalInSelection()
        {
            return this.SelectedText.IndexOf(pCulture.NumberFormat.NumberDecimalSeparator) > -1;
        }

        private bool pIsInsertingDecimal()
        {
            int _indexOfDec = this.Text.IndexOf(pCulture.NumberFormat.NumberDecimalSeparator);
            return _indexOfDec < 0 ? false : this.SelectionStart > _indexOfDec;
        }

        private int pNumberOfDecimalsUsed()
        {
            int _count = 0;
            int _indexOfDec = this.Text.IndexOf(pCulture.NumberFormat.NumberDecimalSeparator);
            if (_indexOfDec > -1)
            {
                _count = this.Text.Length - (_indexOfDec + 1);
            }
            return _count;
        }

        private bool pDoAcceptKey(KeyEventArgs keyArgs)
        {

            bool _doAccept = false;

            // if decimal key
            if (pIsDecimalKey(keyArgs) == true)
            {
                _doAccept = pAcceptDecimalKey == true && (pIsDecimalKeyUsed() == false || pIsDecimalInSelection() == true);
            }
            else
            {
                // if special key
                _doAccept = pIsSpecialKey(keyArgs);
                if (_doAccept == false)
                {
                    // check if numeric key
                    _doAccept = pIsNumericKey(keyArgs);
                    if (_doAccept == true)
                    {
                        // check decimals
                        if (pIsInsertingDecimal())
                            _doAccept = pNumberOfDecimalsUsed() < pNumberOfDecimals;
                    }
                }
            }

            return _doAccept;
        }

        #endregion private methods (check key and decimals)

        #region override events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.None)
            {

                if (!pDoAcceptKey(e))
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;

            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (pReplaceMode)
            {
                this.SelectAll();
            }

            base.OnGotFocus(e);
        }

        #endregion override events



    }

    
}
