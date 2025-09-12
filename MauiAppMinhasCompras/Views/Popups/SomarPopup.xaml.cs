using CommunityToolkit.Maui.Views;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views.Popups
{
    public partial class SomarPopup : Popup
    {
        private ObservableCollection<Produto> ListaCompleta { get; set; }

        public SomarPopup(ObservableCollection<Produto> lista)
        {
            InitializeComponent();
            ListaCompleta = lista;
            CarregarDados();
        }

        private void CarregarDados()
        {
            // Soma o valor total de toda a lista
            double valorTotal = ListaCompleta.Sum(p => p.Total);
            lblValorTotal.Text = $"R$ {valorTotal:F2}";

            // Preenche o picker com as categorias únicas
            var categorias = ListaCompleta.Select(p => p.Categoria).Distinct().ToList();
            pickerCategoria.ItemsSource = categorias;
        }

        private void OnPickerCategoriaChanged(object sender, EventArgs e)
        {
            if (pickerCategoria.SelectedItem != null)
            {
                string categoriaSelecionada = pickerCategoria.SelectedItem.ToString();

                // Filtra a lista pela categoria selecionada e soma os valores
                double valorCategoria = ListaCompleta.Where(p => p.Categoria == categoriaSelecionada).Sum(p => p.Total);

                lblValorCategoria.Text = $"R$ {valorCategoria:F2}";
            }
            else
            {
                lblValorCategoria.Text = string.Empty;
            }
        }

        private void OnFecharClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}