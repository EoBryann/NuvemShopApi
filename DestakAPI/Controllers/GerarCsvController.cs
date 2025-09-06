using DestakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    [HttpPost("GerarPlanilha")]
    public async Task<IActionResult> GerarPlanilha([FromForm] FonteDados fonteDados, string categoria)
    {
        if (fonteDados.Txt == null || fonteDados.Txt.Length == 0)
            return BadRequest("Arquivo TXT não enviado.");

        string txtContent;
        using (var reader = new StreamReader(fonteDados.Txt.OpenReadStream(), Encoding.UTF8))
        {
            txtContent = await reader.ReadToEndAsync();
        }

        // Regex para extrair todos os <script type="application/ld+json" ...> ... </script> blocos que contenham o JSON
        var regex = new Regex(
            @"<script\s+type=""application/ld\+json""\s+data-component=""structured-data\.item"">\s*(.*?)\s*</script>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        var matches = regex.Matches(txtContent);

        var produtos = new List<JsonElement>();

        foreach (Match match in matches)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(match.Groups[1].Value);
                produtos.Add(jsonDoc.RootElement.Clone());
            }
            catch
            {
                // Ignorar bloco com JSON inválido
            }
        }

        var pattern = new ImportColunas();

        var sb = new StringBuilder();

        // Cabeçalho CSV
        sb.AppendLine(string.Join(";", new string[]
        {
                pattern.IdentificadorURL,
                pattern.Nome,
                pattern.Categorias,
                pattern.NomeVariacao1,
                pattern.ValorVariacao1,
                pattern.NomeVariacao2,
                pattern.ValorVariacao2,
                pattern.NomeVariacao3,
                pattern.ValorVariacao3,
                pattern.Preco,
                pattern.PrecoPromocional,
                pattern.PesoKg,
                pattern.AlturaCm,
                pattern.LarguraCm,
                pattern.ComprimentoCm,
                pattern.Estoque,
                pattern.SKU,
                pattern.CodigoBarras,
                pattern.ExibirNaLoja,
                pattern.FreteGratis,
                pattern.Descricao,
                pattern.Tags,
                pattern.TituloSEO,
                pattern.DescricaoSEO,
                pattern.Marca,
                pattern.ProdutoFisico,
                pattern.MPN,
                pattern.Sexo,
                pattern.FaixaEtaria,
                pattern.Custo
        }));

        foreach (var produto in produtos)
        {
            string nomepUrl = produto.GetProperty("name").GetString() ?? "";
            // Extração dos campos úteis do JSON, com fallback para string vazia se não existir
            //string identificadorUrl = produto.GetProperty("mainEntityOfPage").GetProperty("@id").GetString() ?? "";
            //string identificadorUrl = produto.GetProperty("name").GetString().Replace(" ", "-") ?? "";
            //string identificadorUrl = "";
            string identificadorUrl = GerarIdentificadorUrl(nomepUrl);
            string nome = produto.GetProperty("name").GetString() ?? "";
            string categorias = categoria; // Pode definir um valor fixo se desejar
            string nomeVariacao1 = "";
            string valorVariacao1 = "";
            string nomeVariacao2 = "";
            string valorVariacao2 = "";
            string nomeVariacao3 = "";
            string valorVariacao3 = "";             
            string preco = produto.TryGetProperty("offers", out var offers) && offers.TryGetProperty("price", out var price) ? price.GetString() ?? "" : "";
            string precoPromocional = "";
            string pesoKg = produto.TryGetProperty("weight", out var weight) && weight.TryGetProperty("value", out var weightValue) ? weightValue.GetString() ?? "" : "";
            string alturaCm = "";
            string larguraCm = "";
            string comprimentoCm = "";
            string estoque = "";
            string sku = "";
            string codigoBarras = "";
            string exibirNaLoja = "Sim"; // definido fixo
            string freteGratis = "Não";  // definido fixo
            string descricao = produto.GetProperty("description").GetString() ?? "";
            string tags = "";
            string tituloSEO = "";
            string descricaoSEO = "";
            string marca = produto.TryGetProperty("offers", out var offers2) && offers2.TryGetProperty("seller", out var seller) && seller.TryGetProperty("name", out var sellerName) ? sellerName.GetString() ?? "" : "";
            string produtoFisico = "Sim"; // definido fixo
            string mpn = "";
            string sexo = "";
            string faixaEtaria = "";
            string custo = "";

            var linha = new string[]
            {
                    identificadorUrl, nome, categorias,
                    nomeVariacao1, valorVariacao1,
                    nomeVariacao2, valorVariacao2,
                    nomeVariacao3, valorVariacao3,
                    preco, precoPromocional,
                    pesoKg, alturaCm, larguraCm, comprimentoCm,
                    estoque, sku, codigoBarras,
                    exibirNaLoja, freteGratis,
                    descricao, tags, tituloSEO, descricaoSEO,
                    marca, produtoFisico, mpn, sexo, faixaEtaria, custo
            };

            // Escapar campos que contêm ponto e vírgula ou aspas
            for (int i = 0; i < linha.Length; i++)
            {
                if (linha[i].Contains(";") || linha[i].Contains("\""))
                    linha[i] = $"\"{linha[i].Replace("\"", "\"\"")}\"";
            }

            sb.AppendLine(string.Join(";", linha));
        }

        var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
        var csvStream = new MemoryStream(csvBytes);

        return File(csvStream, "text/csv", "planilha_gerada.csv");
    }

    private string GerarIdentificadorUrl(string nomeProduto)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            return "";

        var normalized = nomeProduto.Normalize(System.Text.NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        var nomeSemAcento = sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();

        nomeSemAcento = System.Text.RegularExpressions.Regex.Replace(nomeSemAcento, @"[^a-z0-9\s-]", "");
        nomeSemAcento = System.Text.RegularExpressions.Regex.Replace(nomeSemAcento, @"[\s-]+", "-").Trim('-');

        return nomeSemAcento;
    }

}
