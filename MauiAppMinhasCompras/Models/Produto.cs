using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        string _categoria;
        string _quantidade;
        string _preco;
        DateTime _datacadastro;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {

            get => _descricao;

            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, defina a descrição.");
                }
                _descricao = value;
            }
        }

        public string Categoria
        {
            get => _categoria;

            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, defina a categoria.");
                }
                _categoria = value;
            }
        }
        public double Quantidade
        {

            get => Convert.ToDouble(_quantidade);

            set
            {
                if (value <= 0)
                {
                    throw new Exception("A quantidade deve ser maior que zero.");
                }
                _quantidade = value.ToString();
            }
        }
        public double Preco
        {

            get => Convert.ToDouble(_preco);


            set
            {

                if (value < 0)
                {
                    throw new Exception("O preço não pode ser zero.");
                }
                _preco = value.ToString();

            }

        }

        public DateTime DataCadastro
        {
            get => _datacadastro;
            set
            {
                _datacadastro = value.Date;
            }
        }
        public double Total { get => Quantidade * Preco; }

    }
}
