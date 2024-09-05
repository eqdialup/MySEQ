using System;
using System.Collections;
using System.Windows.Forms;

namespace myseq
{
    // Compares two ListView items based on a selected column.
    // Compares two ListView items based on a selected column.
    public class ListViewComparer : IComparer
    {
        private readonly int ColumnNumber;
        private readonly SortOrder SortOrder;

        public ListViewComparer(int columnNumber, SortOrder sortOrder)
        {
            ColumnNumber = columnNumber;
            SortOrder = sortOrder;
        }

        // Compare two ListViewItems.
        public int Compare(object x, object y)
        {
            // Get the objects as ListViewItems.
            var item_x = x as ListViewItem;
            var item_y = y as ListViewItem;

            // Get the corresponding sub-item values.
            var string_x = item_x?.SubItems.Count > ColumnNumber ? item_x.SubItems[ColumnNumber].Text : "";
            var string_y = item_y?.SubItems.Count > ColumnNumber ? item_y.SubItems[ColumnNumber].Text : "";

            // Compare them.
            int result = CompareItems(string_x, string_y);

            // Return the correct result depending on the sort order.
            return SortOrder == SortOrder.Ascending ? result : -result;
        }

        private static int CompareItems(string string_x, string string_y)
        {
            // Compare as numbers
            if (double.TryParse(string_x, out var double_x) && double.TryParse(string_y, out var double_y))
                return double_x.CompareTo(double_y);

            // Compare as dates
            if (DateTime.TryParse(string_x, out var date_x) && DateTime.TryParse(string_y, out var date_y))
                return date_x.CompareTo(date_y);

            // Compare as strings
            return string.Compare(string_x, string_y);
        }
    }
}