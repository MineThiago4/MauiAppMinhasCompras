using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        string _quantidade;
        string _preco;

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
        public double Total { get => Quantidade * Preco; }

    }
}
