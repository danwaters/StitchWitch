using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace StitchyPhone2
{
    public class ThreadTableSource : UITableViewSource
    {
        protected string[] TableItems;
        protected string CellIdentifier = "TableCell";

        public ThreadTableSource(string[] items)
        {
            TableItems = items;
        }

        /// <summary>
        /// Called by the TableView to determine how many sections(groups) there are.
        /// </summary>
        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        public override int RowsInSection(UITableView tableview, int section)
        {
            return TableItems.Length;
        }

        /// <summary>
        /// Called when a row is touched
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            /*
            new UIAlertView("Row Selected"
                , TableItems[indexPath.Row], null, "OK", null).Show();
            tableView.DeselectRow(indexPath, true);*/
        }

        /// <summary>
        /// Called by the TableView to get the actual UITableViewCell to render for the particular row
        /// </summary>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            var item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = item;
            cell.Accessory = UITableViewCellAccessory.Checkmark;

            return cell;
        }
    }
}