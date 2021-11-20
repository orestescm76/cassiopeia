using System.Windows.Forms;
namespace Cassiopeia.src.Classes
{
    internal class ListViewPhysicalAlbum:ListViewItem
    {
        public ListViewPhysicalAlbum(string[] data) : base(data) { }
        public string ID { get; set; }
    }
}
