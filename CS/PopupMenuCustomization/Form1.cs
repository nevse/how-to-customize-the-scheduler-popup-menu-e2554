using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
#region #usings
using DevExpress.Utils.Menu;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Services;
using DevExpress.XtraScheduler.Commands;
// ...
#endregion #usings


namespace PopupMenuCustomization
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm {
        public Form1()
        {
            InitializeComponent();
            MySchedulerHelper.FillResources(schedulerStorage1, 3);
            schedulerControl1.Views.DayView.DayCount = 2;
            schedulerControl1.Start = DateTime.Now.Date;
            schedulerControl1.DateNavigationBar.Visible = false;
            schedulerControl1.ActiveView.NavigationButtonVisibility = NavigationButtonVisibility.Never;
        }

#region #popupmenushowing
private void schedulerControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
{
    if (e.Menu.Id == DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu)
    {
        // Hide the "Change View To" menu item.
        SchedulerPopupMenu itemChangeViewTo = e.Menu.GetPopupMenuById(SchedulerMenuItemId.SwitchViewMenu);
        itemChangeViewTo.Visible = false;

        // Remove unnecessary items.
        e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewAllDayEvent);

        // Disable the "New Recurring Appointment" menu item.
        e.Menu.DisableMenuItem(SchedulerMenuItemId.NewRecurringAppointment);

        // Find the "New Appointment" menu item and rename it.
        SchedulerMenuItem item = e.Menu.GetMenuItemById(SchedulerMenuItemId.NewAppointment);
                if (item != null) {
                    item.Caption = "&New Meeting";
                    item.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("NewItem.svg");
                }

        // Create a menu item for the Scheduler command.
        ISchedulerCommandFactoryService service = schedulerControl1.GetService<ISchedulerCommandFactoryService>();
        SchedulerCommand cmd = service.CreateCommand(SchedulerCommandId.SwitchToGroupByResource);
        SchedulerMenuItemCommandWinAdapter menuItemCommandAdapter =
            new SchedulerMenuItemCommandWinAdapter(cmd);
        DXMenuItem menuItem = (DXMenuItem)menuItemCommandAdapter.CreateMenuItem(DXMenuItemPriority.Normal);
        menuItem.BeginGroup = true;
        e.Menu.Items.Add(menuItem);

        // Insert a new item into the Scheduler popup menu and handle its click event.
        e.Menu.Items.Add(new SchedulerMenuItem("Click me!", MyClickHandler));
    }
}

public void MyClickHandler(object sender, EventArgs e)
{
    MessageBox.Show("My Menu Item Clicked!");
}
#endregion #popupmenushowing
    }
}