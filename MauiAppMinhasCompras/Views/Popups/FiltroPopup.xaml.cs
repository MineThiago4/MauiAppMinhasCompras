using CommunityToolkit.Maui.Views;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views.Popups;

public partial class FiltroPopup : Popup
{
    private ObservableCollection<Produto> _listaProdutos;

    public FiltroPopup(ObservableCollection<Produto> listaProdutos)
    {
        InitializeComponent();
        _listaProdutos = listaProdutos;
        CarregarCategorias();
    }

    private void CarregarCategorias()
    {
        var categorias = _listaProdutos.Select(p => p.Categoria).Distinct().ToList();
        pickerCategoria.ItemsSource = categorias;
    }

    void OnFecharClicked(object sender, EventArgs e)
    {
        // Fecha o popup sem retornar valor
        Close();
    }

    void OnFiltroSelecionado(object sender, EventArgs e)
    {
        // Pega o texto do botão que foi clicado para saber qual filtro aplicar
        var botao = sender as Button;
        string filtroEscolhido = botao.Text;

        // Fecha o popup e retorna o valor do filtro escolhido
        Close(filtroEscolhido);
    }

   private void OnFiltrarCategoriaClicked(object sender, EventArgs e)
    {
        if (pickerCategoria.SelectedItem != null)
        {
            string categoria = pickerCategoria.SelectedItem.ToString();
            Close($"categoria:{categoria}");
        }
    }

    private void OnFiltroData(object sender, EventArgs e)
    {
        // Coleta as datas dos dois DatePickers
        DateTime dataInicio = dtp_data_comeco.Date;
        DateTime dataFim = dtp_data_fim.Date;

        Close($"{dataInicio:yyyy-MM-dd}|{dataFim:yyyy-MM-dd}");
    }
}