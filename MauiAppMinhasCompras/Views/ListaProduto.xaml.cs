using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();
    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = Lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            Lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {

        try
        {

            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "Ok");
        }

    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            string P = e.NewTextValue;

            Lista.Clear();

            List<Produto> tmp = await App.Db.Search(P);

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = Lista.Sum(i => i.Total);

            string msg = $"O valor total da compra é {soma:C}";

            DisplayAlert("Total da Compra", msg, "Ok");
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "Ok");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Atenção", $"Confirma a exclusão do produto {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                Lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {

            Produto p = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "Ok");
        }
    }
}