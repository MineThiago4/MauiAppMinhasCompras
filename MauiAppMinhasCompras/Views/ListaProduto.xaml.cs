using CommunityToolkit.Maui.Views;
using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Views.Popups;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage, INotifyPropertyChanged
{
    ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();

    private bool _isFilteredByCategory;
    public bool IsFilteredByCategory
    {
        get => _isFilteredByCategory;
        set
        {
            if (_isFilteredByCategory != value)
            {
                _isFilteredByCategory = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isFilteredByDate;
    public bool IsFilteredByDate
    {
        get => _isFilteredByDate;
        set
        {
            if (_isFilteredByDate != value)
            {
                _isFilteredByDate = value;
                OnPropertyChanged();
            }
        }
    }
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

            lst_produtos.IsRefreshing = true;

            Lista.Clear();

            List<Produto> tmp = await App.Db.Search(P);

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        /*try
        {
            double soma = Lista.Sum(i => i.Total);

            string msg = $"O valor total da compra é {soma:C}";

            DisplayAlert("Total da Compra", msg, "Ok");
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "Ok");
        }*/

        var popup = new SomarPopup(Lista); 
        await this.ShowPopupAsync(popup);
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

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
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
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }


    private async void Filtro_Clicked(object sender, EventArgs e)
    {
        var popup = new FiltroPopup(Lista);
        var resultado = await this.ShowPopupAsync(popup);

        try
        {
            if (resultado is string filtro)
            {
                // Reseta as propriedades de visibilidade
                IsFilteredByCategory = false;
                IsFilteredByDate = false;

                
                // Declara a variável aqui para ser usada por todos os filtros
                List<Produto> listaFiltrada = new List<Produto>();

                // Usamos um switch para lidar com todos os filtros de forma unificada
                switch (filtro)
                {
                    case string s when s.StartsWith("categoria:"):
                        string categoriaFiltro = s.Substring("categoria:".Length);
                        listaFiltrada = Lista.Where(p => p.Categoria != null && p.Categoria.ToLower().Contains(categoriaFiltro.ToLower())).ToList();
                        IsFilteredByCategory = true;
                        break;

                    case string s when s.Contains("|"):
                        try
                        {
                            string[] datas = s.Split('|');

                            DateTime dataInicio = DateTime.ParseExact(datas[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime dataFim = DateTime.ParseExact(datas[1], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                            dataFim = dataFim.Date.AddDays(1).AddSeconds(-1);

                            listaFiltrada = Lista.Where(p => p.DataCadastro.Date >= dataInicio.Date && p.DataCadastro.Date <= dataFim.Date).ToList();
                            IsFilteredByDate = true;
                        }
                        catch (FormatException fe)
                        {
                            await DisplayAlert("Erro", $"O formato da data retornada pelo filtro está incorreto. Erro: {fe}", "Ok");
                            listaFiltrada = Lista.ToList();
                        }
                        break;
                    case "Ordem Alfabética (A-Z)":
                        listaFiltrada = Lista.OrderBy(p => p.Descricao).ToList();
                        break;
                    case "Mais Recentes":
                        listaFiltrada = Lista.OrderByDescending(p => p.DataCadastro).ToList();
                        break;
                    case "Menos Recentes":
                        listaFiltrada = Lista.OrderBy(p => p.DataCadastro).ToList();
                        break;
                    case "Maior Valor":
                        listaFiltrada = Lista.OrderByDescending(p => p.Preco).ToList();
                        break;
                    case "Menor Valor":
                        listaFiltrada = Lista.OrderBy(p => p.Preco).ToList();
                        break;
                    case "Maior Valor Total":
                        listaFiltrada = Lista.OrderByDescending(p => p.Total).ToList();
                        break;
                    case "Menor Valor Total":
                        listaFiltrada = Lista.OrderBy(p => p.Total).ToList();
                        break;
                    case "Maior Quantidade":
                        listaFiltrada = Lista.OrderByDescending(p => p.Quantidade).ToList();
                        break;
                    case "Menor Quantidade":
                        listaFiltrada = Lista.OrderBy(p => p.Quantidade).ToList();
                        break;
                    default:
                        listaFiltrada = Lista.ToList();
                        break;
                }


                Lista.Clear();
                foreach (var item in listaFiltrada)
                {
                    Lista.Add(item);
                }
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok");
        }

    }
}