using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using System.Text.Json.Serialization;

namespace DestakAPI.Models
{
    public class Produto
    {
        public string NomepUrl { get; set; } = "";
        public string IdentificadorUrl { get; set; } = "";
        public string ImgUrl { get; set; } = "";
        public string Nome { get; set; } = "";
        public List<int> Categorias { get; set; } = new List<int>();
        public string NomeVariacao1 { get; set; } = "";
        public string ValorVariacao1 { get; set; } = "";
        public string NomeVariacao2 { get; set; } = "";
        public string ValorVariacao2 { get; set; } = "";
        public string NomeVariacao3 { get; set; } = "";
        public string ValorVariacao3 { get; set; } = "";
        public string Preco { get; set; } = "";
        public string PrecoPromocional { get; set; } = "";
        public string PesoKg { get; set; } = "";
        public string AlturaCm { get; set; } = "";
        public string LarguraCm { get; set; } = "";
        public string ComprimentoCm { get; set; } = "";
        public string Estoque { get; set; } = "";
        public string Sku { get; set; } = "";
        public string CodigoBarras { get; set; } = "";
        public string ExibirNaLoja { get; set; } = "Sim";
        public string FreteGratis { get; set; } = "Não";
        public string Descricao { get; set; } = "";
        public string Tags { get; set; } = "";
        public string TituloSEO { get; set; } = "";
        public string DescricaoSEO { get; set; } = "";
        public string Marca { get; set; } = "";
        public string ProdutoFisico { get; set; } = "Sim";
        public string Mpn { get; set; } = "";
        public string Sexo { get; set; } = "";
        public string FaixaEtaria { get; set; } = "";
        public string Custo { get; set; } = "";
    }


}