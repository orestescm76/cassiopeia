using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace aplicacion_musica
{
    public partial class visualizarAlbum : Form
    {
        private Album albumAVisualizar;
        private byte numDisco;
        private DiscoCompacto CDaVisualizar;
        private ListViewItemComparer lvwColumnSorter;
        public visualizarAlbum(ref Album a)
        {
            InitializeComponent();
            numDisco = 0;
            albumAVisualizar = a;
            CDaVisualizar = null;
            infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + a.artista + Environment.NewLine +
                Programa.textosLocal.GetString("titulo") + ": " + a.nombre + Environment.NewLine +
                Programa.textosLocal.GetString("año") + ": " + a.year + Environment.NewLine +
                Programa.textosLocal.GetString("duracion") + ": " + a.duracion.ToString() + Environment.NewLine +
                Programa.textosLocal.GetString("genero") + ": " + a.genero.traducido + Environment.NewLine +
                "Localización: " + a.DirectorioSonido + Environment.NewLine;
            if (a.caratula != "")
            {
                Image caratula = Image.FromFile(a.caratula);
                vistaCaratula.Image = caratula;
                vistaCaratula.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            Controls.Add(barraAbajo);
            labelEstadoDisco.Hide();
            ponerTextos();
            cargarVista();
        }
        public visualizarAlbum(ref DiscoCompacto cd)
        {
            InitializeComponent();
            CDaVisualizar = cd;
            albumAVisualizar = cd.Album;
            numDisco = 1;
            infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + cd.Album.artista + Environment.NewLine +
                Programa.textosLocal.GetString("titulo") + ": " + cd.Album.nombre + Environment.NewLine +
                Programa.textosLocal.GetString("año") + ": " + cd.Album.year + Environment.NewLine +
                Programa.textosLocal.GetString("duracion") + ": " + cd.Album.duracion.ToString() + Environment.NewLine +
                Programa.textosLocal.GetString("genero") + ": " + cd.Album.genero.traducido + Environment.NewLine +
                Programa.textosLocal.GetString("formato") + ": " + Programa.textosLocal.GetString(cd.FormatoCD.ToString()) + Environment.NewLine +
                Programa.textosLocal.GetString("añoPublicacion") + ": " + cd.YearRelease + Environment.NewLine +
                Programa.textosLocal.GetString("paisPublicacion") + ":" + cd.PaisPublicacion + Environment.NewLine +
                Programa.textosLocal.GetString("estado_exterior") + ": " + Programa.textosLocal.GetString(cd.EstadoExterior.ToString()) + Environment.NewLine;
            labelEstadoDisco.Text = Programa.textosLocal.GetString("estado_medio") + " " + numDisco + ": " + Programa.textosLocal.GetString(cd.Discos[0].EstadoDisco.ToString()) + Environment.NewLine;
            if (cd.Album.caratula != "")
            {
                Image caratula = Image.FromFile(cd.Album.caratula);
                vistaCaratula.Image = caratula;
                vistaCaratula.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 9);
            Controls.Add(barraAbajo);
            ponerTextos();
            cargarVista();
        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("visualizando") + " " + albumAVisualizar.artista + " - " + albumAVisualizar.nombre;
            vistaCanciones.Columns[0].Text = "#";
            vistaCanciones.Columns[1].Text = Programa.textosLocal.GetString("titulo");
            vistaCanciones.Columns[2].Text = Programa.textosLocal.GetString("duracion");
            okDoomerButton.Text = Programa.textosLocal.GetString("hecho");
            editarButton.Text = Programa.textosLocal.GetString("editar");
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": 00:00:00";
            if (CDaVisualizar != null)
                buttonAnotaciones.Text = Programa.textosLocal.GetString("editar_anotaciones");
            else
                buttonAnotaciones.Text = "Reproducir";
            setBonusToolStripMenuItem.Text = Programa.textosLocal.GetString("setBonus");
            setLargaToolStripMenuItem.Text = Programa.textosLocal.GetString("setLarga");
        }
        private void cargarVista()
        {
            ListViewItem[] items = new ListViewItem[albumAVisualizar.canciones.Count];
            int i = 0, j = 0, d = 0;
            TimeSpan durBonus = new TimeSpan();
            if (CDaVisualizar != null && CDaVisualizar.Discos.Length > 1)
            {
                ListViewGroup d1 = new ListViewGroup("Disco 1");
                ListViewGroup d2 = new ListViewGroup("Disco 2");
                vistaCanciones.Groups.Add(d1);
                vistaCanciones.Groups.Add(d2);
                vistaCanciones.ShowGroups = true;
                foreach (Cancion c in albumAVisualizar.canciones)
                {
                    String[] datos = new string[3];
                    datos[0] = (j + 1).ToString();
                    c.ToStringArray().CopyTo(datos, 1);
                    items[i] = new ListViewItem(datos);
                    j++;
                    items[i].Group = vistaCanciones.Groups[d];
                    if (j >= CDaVisualizar.Discos[d].NumCanciones)
                    {
                        d++;
                        j = 0;
                    }
                    if (c is CancionLarga)
                    {
                        items[i].BackColor = Color.LightSalmon;
                    }
                    if (c.Bonus)
                    {
                        items[i].BackColor = Color.SkyBlue;
                        durBonus += c.duracion;
                    }
                    i++;
                }
                if (durBonus.TotalMilliseconds != 0)
                    infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + albumAVisualizar.artista + Environment.NewLine +
                        Programa.textosLocal.GetString("titulo") + ": " + albumAVisualizar.nombre + Environment.NewLine +
                        Programa.textosLocal.GetString("año") + ": " + albumAVisualizar.year + Environment.NewLine +
                        Programa.textosLocal.GetString("duracion") + ": " + albumAVisualizar.duracion.ToString() + " (" + durBonus.ToString() + ")" + Environment.NewLine +
                        Programa.textosLocal.GetString("genero") + ": " + albumAVisualizar.genero.traducido + Environment.NewLine +
                        Programa.textosLocal.GetString("estado_exterior") + ": " + Programa.textosLocal.GetString(CDaVisualizar.EstadoExterior.ToString()) + Environment.NewLine +
                        Programa.textosLocal.GetString("estado_medio") + ": " + Programa.textosLocal.GetString(CDaVisualizar.Discos[0].EstadoDisco.ToString()) + Environment.NewLine +
                        Programa.textosLocal.GetString("formato") + ": " + Programa.textosLocal.GetString(CDaVisualizar.FormatoCD.ToString()) + Environment.NewLine;
                vistaCanciones.Items.AddRange(items);
            }
            else if (CDaVisualizar != null)
            {
                foreach (Cancion c in albumAVisualizar.canciones)
                {

                    String[] datos = new string[3];
                    datos[0] = (i + 1).ToString();
                    c.ToStringArray().CopyTo(datos, 1);
                    items[i] = new ListViewItem(datos);

                    if (c is CancionLarga)
                    {
                        items[i].BackColor = Color.LightSalmon;
                    }
                    if (c.Bonus)
                    {
                        items[i].BackColor = Color.SkyBlue;
                        durBonus += c.duracion;
                    }
                    i++;
                }
                if (durBonus.TotalMilliseconds != 0)
                    infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + albumAVisualizar.artista + Environment.NewLine +
                        Programa.textosLocal.GetString("titulo") + ": " + albumAVisualizar.nombre + Environment.NewLine +
                        Programa.textosLocal.GetString("año") + ": " + albumAVisualizar.year + Environment.NewLine +
                        Programa.textosLocal.GetString("duracion") + ": " + albumAVisualizar.duracion.ToString() + " (" + durBonus.ToString() + ")" + Environment.NewLine +
                        Programa.textosLocal.GetString("genero") + ": " + albumAVisualizar.genero.traducido + Environment.NewLine + 
                        Programa.textosLocal.GetString("estado_exterior") + ": " + Programa.textosLocal.GetString(CDaVisualizar.EstadoExterior.ToString()) + Environment.NewLine +
                        Programa.textosLocal.GetString("estado_medio") + ": " + Programa.textosLocal.GetString(CDaVisualizar.Discos[0].EstadoDisco.ToString()) + Environment.NewLine +
                        Programa.textosLocal.GetString("formato") + ": " + Programa.textosLocal.GetString(CDaVisualizar.FormatoCD.ToString()) + Environment.NewLine;
                vistaCanciones.Items.AddRange(items);
                
            }
            else
            {
                foreach (Cancion c in albumAVisualizar.canciones)
                {

                    String[] datos = new string[3];
                    datos[0] = (i + 1).ToString();
                    c.ToStringArray().CopyTo(datos, 1);
                    items[i] = new ListViewItem(datos);

                    if (c is CancionLarga)
                    {
                        items[i].BackColor = Color.LightSalmon;
                    }
                    if (c.Bonus)
                    {
                        items[i].BackColor = Color.SkyBlue;
                        durBonus += c.duracion;
                    }
                    i++;
                }
                if (durBonus.TotalMilliseconds != 0)
                    infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + albumAVisualizar.artista + Environment.NewLine +
                        Programa.textosLocal.GetString("titulo") + ": " + albumAVisualizar.nombre + Environment.NewLine +
                        Programa.textosLocal.GetString("año") + ": " + albumAVisualizar.year + Environment.NewLine +
                        Programa.textosLocal.GetString("duracion") + ": " + albumAVisualizar.duracion.ToString() + " (" + durBonus.ToString() + ")" + Environment.NewLine +
                        Programa.textosLocal.GetString("genero") + ": " + albumAVisualizar.genero.traducido;
                vistaCanciones.Items.AddRange(items);
            }
        }
        private void ordenarColumnas(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.ColumnaAOrdenar) // Determine if clicked column is already the column that is being sorted.
            {
                if (lvwColumnSorter.Orden == SortOrder.Ascending)
                    lvwColumnSorter.Orden = SortOrder.Descending;
                else lvwColumnSorter.Orden = SortOrder.Ascending;

            }
            else if (e.Column != 2 && e.Column != 3)//si la columna es  la del año o la de la duracion, que lo ponga de mayor a menor.
            {
                lvwColumnSorter.ColumnaAOrdenar = e.Column;
                lvwColumnSorter.Orden = SortOrder.Ascending;

            }
            else
            {
                lvwColumnSorter.ColumnaAOrdenar = e.Column; // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.Orden = SortOrder.Descending;
            }
            vistaCanciones.Sort();
            vistaCanciones.Refresh();
        }

        private void okDoomerButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void editarButton_Click(object sender, EventArgs e)
        {
            if(CDaVisualizar is null)
            {
                editarAlbum editor = new editarAlbum(ref albumAVisualizar);
                editor.Show();
            }
            else
            {
                CrearCD editor = new CrearCD(ref CDaVisualizar, numDisco, true);
                editor.ShowDialog();
            }
            Close();
        }
        private void vistaCanciones_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            TimeSpan seleccion = new TimeSpan();
            foreach (ListViewItem cancion in vistaCanciones.SelectedItems)
            {
                if(CDaVisualizar !=  null &&CDaVisualizar.Discos.Length > 1)
                {
                    Cancion can = albumAVisualizar.getCancion(cancion.SubItems[1].Text);
                    seleccion += can.duracion;
                }
                else
                {
                    int c = Convert.ToInt32(cancion.SubItems[0].Text); c--;
                    Cancion can = albumAVisualizar.getCancion(c);
                    seleccion += can.duracion;
                }
            }
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": " + seleccion.ToString();
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int n = Convert.ToInt32(vistaCanciones.SelectedItems[0].SubItems[0].Text);
            Cancion c = albumAVisualizar.getCancion(n-1);
            if(c is CancionLarga cl)
            {
                string infoDetallada = "";
                for (int i = 0; i < cl.Partes.Count; i++)
                {
                    infoDetallada += cl.GetNumeroRomano(i + 1) + ". ";
                    infoDetallada += cl.Partes[i].titulo + " - " + cl.Partes[i].duracion;
                    infoDetallada += Environment.NewLine;
                }
                MessageBox.Show(infoDetallada);
            }
        }

        private void buttonAnotaciones_Click(object sender, EventArgs e)
        {
            if(CDaVisualizar != null)
            {
                Anotaciones anoForm = new Anotaciones(ref CDaVisualizar);
                anoForm.ShowDialog();
            }
            else
            {
                ListaReproduccion ls = new ListaReproduccion();
                foreach (Cancion cancion in albumAVisualizar.canciones)
                {
                    ls.AgregarCancion(cancion);
                }
                Reproductor.Instancia.ReproducirLista(ls);
            }
        }

        private void labelEstadoDisco_Click(object sender, EventArgs e)
        {
            if (CDaVisualizar.Discos.Length == 1)
                return;
            else
            {
                switch (numDisco)
                {
                    case 1:
                        numDisco = 2;
                        labelEstadoDisco.Text = Programa.textosLocal.GetString("estado_medio") + " " + numDisco + ": " + Programa.textosLocal.GetString(CDaVisualizar.Discos[numDisco-1].EstadoDisco.ToString()) + Environment.NewLine;
                        break;
                    case 2:
                        numDisco = 1;
                        labelEstadoDisco.Text = Programa.textosLocal.GetString("estado_medio") + " " + numDisco + ": " + Programa.textosLocal.GetString(CDaVisualizar.Discos[numDisco - 1].EstadoDisco.ToString()) + Environment.NewLine;
                        break;
                    default:
                        break;
                }
            }
        }

        private void visualizarAlbum_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in vistaCanciones.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void setBonusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in vistaCanciones.SelectedItems)
            {
                Cancion c = albumAVisualizar.canciones[Convert.ToInt32(item.SubItems[0].Text)-1];
                albumAVisualizar.duracion -= c.duracion;
                c.Bonus = true;
            }
            visualizarAlbum vNew = new visualizarAlbum(ref albumAVisualizar);
            Dispose();
            Close();
            vNew.Show();
        }

        private void vistaCanciones_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                clickDerechoConfig.Show(vistaCanciones, e.Location);
            }
        }
    }
}
