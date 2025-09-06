using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using System.Text.Json.Serialization;

namespace DestakAPI.Models
{
    public class FonteDados
    {
        public IFormFile Txt { get; set; }

        // Exemplo de colunas fixas que definem o padrão da planilha de importação

        [OpenApiIgnore]
        public ImportColunas colunas { get; set; }
    }
    public class ImportColunas
    {
        [OpenApiIgnore]
        public string IdentificadorURL { get; set; }

        [OpenApiIgnore]
        public string Nome { get; set; }

        [OpenApiIgnore]
        public string Categorias { get; set; }

        [OpenApiIgnore]
        public string NomeVariacao1 { get; set; }

        [OpenApiIgnore]
        public string ValorVariacao1 { get; set; }

        [OpenApiIgnore]
        public string NomeVariacao2 { get; set; }

        [OpenApiIgnore]
        public string ValorVariacao2 { get; set; }

        [OpenApiIgnore]
        public string NomeVariacao3 { get; set; }

        [OpenApiIgnore]
        public string ValorVariacao3 { get; set; }

        [OpenApiIgnore]
        public string Preco { get; set; }

        [OpenApiIgnore]
        public string PrecoPromocional { get; set; }

        [OpenApiIgnore]
        public string PesoKg { get; set; }

        [OpenApiIgnore]
        public string AlturaCm { get; set; }

        [OpenApiIgnore]
        public string LarguraCm { get; set; }

        [OpenApiIgnore]
        public string ComprimentoCm { get; set; }

        [OpenApiIgnore]
        public string Estoque { get; set; }

        [OpenApiIgnore]
        public string SKU { get; set; }

        [OpenApiIgnore]
        public string CodigoBarras { get; set; }

        [OpenApiIgnore]
        public string ExibirNaLoja { get; set; }

        [OpenApiIgnore]
        public string FreteGratis { get; set; }

        [OpenApiIgnore]
        public string Descricao { get; set; }

        [OpenApiIgnore]
        public string Tags { get; set; }

        [OpenApiIgnore]
        public string TituloSEO { get; set; }

        [OpenApiIgnore]
        public string DescricaoSEO { get; set; }

        [OpenApiIgnore]
        public string Marca { get; set; }

        [OpenApiIgnore]
        public string ProdutoFisico { get; set; }

        [OpenApiIgnore]
        public string MPN { get; set; }

        [OpenApiIgnore]
        public string Sexo { get; set; }

        [OpenApiIgnore]
        public string FaixaEtaria { get; set; }

        [OpenApiIgnore]
        public string Custo { get; set; }
    }

}