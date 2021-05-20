using System;
using System.Collections;
using System.Windows.Forms;

namespace myseq
{
    // Compares two ListView items based on a selected column.
    public class ListViewComparer : IComparer
    {
        private int ColumnNumber;
        private SortOrder SortOrder;

        public ListViewComparer(int column_number, SortOrder sort_order)
        {
            ColumnNumber = column_number;
            SortOrder = sort_order;
        }

        // Compare two ListViewItems.
        public int Compare(object x, object y)
        {
            // Get the objects as ListViewItems.
            ListViewItem item_x = x as ListViewItem;
            ListViewItem item_y = y as ListViewItem;

            // Get the corresponding sub-item values.
            var string_x = (item_x.SubItems.Count <= ColumnNumber) ? "" : item_x.SubItems[ColumnNumber].Text;

            var string_y = item_y.SubItems.Count <= ColumnNumber ? "" : item_y.SubItems[ColumnNumber].Text;

            // Compare them.
            int result = CompareItems(string_x, string_y);

            // Return the correct result depending on whether
            // we're sorting ascending or descending.
            return SortOrder == SortOrder.Ascending ? result : -result;
        }

        private static int CompareItems(string string_x, string string_y)
        {
            int result;
            if (double.TryParse(string_x, out var double_x) &&
                double.TryParse(string_y, out var double_y))
            {
                // Treat as a number.
                result = double_x.CompareTo(double_y);
            }
            else
            {
                DateTime date_x, date_y;
                if (DateTime.TryParse(string_x, out date_x) &&
                    DateTime.TryParse(string_y, out date_y))
                {
                    // Treat as a date.
                    result = date_x.CompareTo(date_y);
                }
                else
                {
                    // Treat as a string.
                    result = string_x.CompareTo(string_y);
                }
            }

            return result;
        }
    }
}