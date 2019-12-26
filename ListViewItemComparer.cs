using System.Windows.Forms;
using System.Collections;

namespace aplicacion_musica
{
    class ListViewItemComparer : IComparer
    {
        public int ColumnaAOrdenar { get; set; }
        public SortOrder Orden { get; set; }
        private CaseInsensitiveComparer ObjectCompare;
        public ListViewItemComparer()
        {
            ColumnaAOrdenar = 0;
            Orden = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }
        public int Compare(object x, object y)
        {
            int Resultado;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;
            Resultado = ObjectCompare.Compare(listviewX.SubItems[ColumnaAOrdenar].Text, listviewY.SubItems[ColumnaAOrdenar].Text);
            if(ColumnaAOrdenar == 3)
            {

            }
            if (Orden == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return Resultado;
            }
            else if (Orden == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-Resultado);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }
    }
}
