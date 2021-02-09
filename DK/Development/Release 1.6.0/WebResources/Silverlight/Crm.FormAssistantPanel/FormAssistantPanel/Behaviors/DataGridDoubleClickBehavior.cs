using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Crm.FormAssistantPanel.Behaviors
{
    public class DataGridDoubleClickBehavior : Behavior<DataGrid>
    {
        private readonly MouseClickManager _gridClickManager;
        public event EventHandler<MouseButtonEventArgs> DoubleClick;
        public DataGridDoubleClickBehavior()
        {
            _gridClickManager = new MouseClickManager(300);
            _gridClickManager.DoubleClick += new MouseButtonEventHandler(_gridClickManager_DoubleClick);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LoadingRow += OnLoadingRow;
            AssociatedObject.UnloadingRow += OnUnloadingRow;
        }
        void OnUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            //row is no longer visible so remove double click event otherwise
            //row events will miss fire
            e.Row.MouseLeftButtonUp -= _gridClickManager.HandleClick;
        }
        void OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            //row is visible in grid, wire up double click event
            e.Row.MouseLeftButtonUp += _gridClickManager.HandleClick;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LoadingRow -= OnLoadingRow;
            AssociatedObject.UnloadingRow -= OnUnloadingRow;
        }
        void _gridClickManager_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClick != null)
                DoubleClick(sender, e);
        }
    }
}