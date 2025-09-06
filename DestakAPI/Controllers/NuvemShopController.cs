
using DestakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NuvemShopApi;
using RestSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class NuvemShopController : ControllerBase
{

    [HttpPost("ExemploNuvemShop")]
    public IActionResult ExemploNuvemShop()
    {

        var apiNuvem = new ClientNuvemShop("Bryanporto02@GMAIL.COM", "DestakAPP", GetFullCredentials());

        var retorno = apiNuvem.GetData<dynamic>("/products");
        var retorno1 = apiNuvem.GetData<dynamic>("/categories");

        //foreach (var r in retorno)
        //{
        //    Console.WriteLine($"Id: {r.id} - Name: {r.name.pt}");
        //    //Console.WriteLine($"Description: {r.description.pt}");
        //    Console.WriteLine("= = = = = = = = = = = = = = = = = ");
        //    if (r.variants is IEnumerable)
        //        foreach (var variant in r.variants)
        //            try
        //            {
        //                var urlPutVariant = $"/products/{r.id}/variants/{variant.id}";

        //                variant.stock = 42;

        //                var parameters = new List<Parameter>
        //                    {
        //                        new Parameter
        //                        {
        //                            Name = "application/json",
        //                            Type = ParameterType.RequestBody,
        //                            Value = variant
        //                        }
        //                    };
        //                var clientPut = new ClientNuvemShop("guigomesa@outlook.com", "Test NuvemShop",
        //                    GetFullCredentials());
        //                var retornPutVariant = clientPut.PutData<dynamic>(urlPutVariant, parameters.ToArray());
        //                Console.WriteLine($"Variacao sku {variant.sku} foi alterada para estoque {variant.stock}");
        //            }
        //            catch (ApiNuvemShopException ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //}

        //Console.WriteLine("Consulta feita");
        //Console.ReadLine();
        return Ok(retorno);
    }


    [HttpPost("InsereProdutosViaHtml")]
    public async Task<IActionResult> InsereProdutosViaHtml(IFormFile fonteDados, [FromQuery] List<int> categoriaIds)
    {
        var apiNuvem = new ClientNuvemShop("Bryanporto02@GMAIL.COM", "DestakAPP", GetFullCredentials());

        if (fonteDados == null || fonteDados.Length == 0)
            return BadRequest("Arquivo fonteDados não enviado.");

        string fonteDadosContent;
        using (var reader = new StreamReader(fonteDados.OpenReadStream(), Encoding.UTF8))
        {
            fonteDadosContent = await reader.ReadToEndAsync();
        }

        var regex = new Regex(
            @"<script\s+type=""application/ld\+json""\s+data-component=""structured-data\.item"">\s*(.*?)\s*</script>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        var matches = regex.Matches(fonteDadosContent);
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

        foreach (var produto in produtos)
        {
            var itemProduto = new Produto();
            itemProduto.NomepUrl = produto.GetProperty("name").GetString() ?? "";
            itemProduto.IdentificadorUrl = GerarIdentificadorUrl(itemProduto.NomepUrl);
            itemProduto.ImgUrl = produto.GetProperty("image").GetString() ?? "";
            itemProduto.Nome = produto.GetProperty("name").GetString() ?? "";
            itemProduto.Categorias = categoriaIds;
            itemProduto.NomeVariacao1 = "";
            itemProduto.ValorVariacao1 = "";
            itemProduto.NomeVariacao2 = "";
            itemProduto.ValorVariacao2 = "";
            itemProduto.NomeVariacao3 = "";
            itemProduto.ValorVariacao3 = "";
            itemProduto.PrecoPromocional = produto.TryGetProperty("offers", out var offers) && offers.TryGetProperty("price", out var price) ? price.GetString() ?? "" : "";
            itemProduto.Preco = "199.90";
            itemProduto.PesoKg = "0.140";
            itemProduto.AlturaCm = "20.00";
            itemProduto.LarguraCm = "9.00";
            itemProduto.ComprimentoCm = "15.00";
            itemProduto.Estoque = "42";
            itemProduto.Sku = "";
            itemProduto.CodigoBarras = "";
            itemProduto.ExibirNaLoja = "Sim";
            itemProduto.FreteGratis = "Não";
            itemProduto.Descricao = "<h1 style=\"color: #ea2f99;\">🛋️ <span style=\"color: #ffffff;\">Descri&ccedil;&atilde;o do Produto</span></h1>\r\n<p><span style=\"color: #551757;\"><strong>Prazo de Produ&ccedil;&atilde;o:</strong></span> 2 a 3 dias &uacute;teis.</p>\r\n<p><span style=\"color: #551757;\"><strong>Medidas:</strong></span> 20cm de altura x 15cm de largura</p>\r\n<p><span style=\"color: #551757;\"><strong>Funcionamento:</strong></span> Cabo USB para Energia el&eacute;trica <br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; e/ou Pilhas <strong><span style=\"color: #551757;\">(N&Acirc;O INCLUSAS)</span></strong> .<br /><br /><span style=\"color: #f1c40f;\"><strong data-start=\"108\" data-end=\"120\"><span style=\"color: #e03e2d;\">Aten&ccedil;&atilde;o</span><span style=\"color: #ffffff;\">:</span></strong><span style=\"color: #ffffff;\"> Ao utilizar pilhas,</span> <span style=\"color: #e03e2d;\"><strong data-start=\"141\" data-end=\"167\">n&atilde;o conecte o cabo USB</strong></span><span style=\"color: #ffffff;\">, para evitar sobrecarga e poss&iacute;veis danos ao equipamento.</span></span></p>\r\n<h2 style=\"color: #ea2f99;\">✨ <span style=\"color: #ffffff;\">Personaliza&ccedil;&atilde;o Exclusiva</span></h2>\r\n<p data-start=\"80\" data-end=\"479\"><span style=\"color: #551757;\"><strong data-start=\"117\" data-end=\"155\">D&ecirc; um toque &uacute;nico &agrave; sua lumin&aacute;ria!</strong></span> Personalize com at&eacute; 3 nomes sem custo extra e, se desejar, adicione uma frase especial logo abaixo &mdash; tornando o produto ainda mais marcante e cheio de significado. Para isso, &eacute; s&oacute; preencher o campo de personaliza&ccedil;&atilde;o localizado abaixo da imagem do item. Nossa equipe cuidar&aacute; de tudo com carinho! Uma maneira incr&iacute;vel de criar um presente exclusivo e inesquec&iacute;vel.</p>\r\n<h2 style=\"color: #ea2f99;\">💡 <span style=\"color: #ffffff;\">Por que escolher nossa lumin&aacute;ria?</span></h2>\r\n<p><span style=\"color: #551757;\"><strong>Mais do que um simples item decorativo</strong></span>, nossa lumin&aacute;ria representa afeto e bem-estar. Ideal para presentear futuras mam&atilde;es, celebrar a chegada de um beb&ecirc; ou compor espa&ccedil;os infantis com charme, ela preenche o ambiente com uma luz serena e um clima envolvente. Com um design pensado nos m&iacute;nimos detalhes, transmite ternura, leveza e bom gosto &mdash; valorizando cada instante de forma &uacute;nica.</p>\r\n<h2 style=\"color: #ea2f99;\">💤 <span style=\"color: #ffffff;\">Lumin&aacute;ria Gravada com o Nome</span></h2>\r\n<p data-start=\"148\" data-end=\"632\"><span style=\"color: #551757;\"><strong>Imagine o cantinho do beb&ecirc;</strong></span> iluminado por uma pe&ccedil;a cheia de encanto, feita para surpreender. Produzida artesanalmente pela <span style=\"color: #551757;\"><strong data-start=\"270\" data-end=\"295\">Destak Personalizados</strong></span>, nossa lumin&aacute;ria re&uacute;ne eleg&acirc;ncia, suavidade e delicadeza. Com acabamento refinado e produ&ccedil;&atilde;o cuidadosa, ela transforma o espa&ccedil;o com calor e beleza. Mais do que iluminar, ela conquista pela presen&ccedil;a marcante, tornando-se um detalhe especial na decora&ccedil;&atilde;o. Um presente funcional e afetivo, feito para tornar cada momento ainda mais m&aacute;gico.</p>\r\n<h2 style=\"color: #ea2f99;\">🎁 <span style=\"color: #ffffff;\">O presente perfeito</span></h2>\r\n<p><span style=\"color: #551757;\"><strong>Se a inten&ccedil;&atilde;o &eacute; surpreender</strong></span> com algo bonito, pr&aacute;tico e cheio de emo&ccedil;&atilde;o, nossa lumin&aacute;ria personalizada &eacute; a escolha perfeita. Seu visual encantador, a luz acolhedora e a possibilidade de incluir nomes ou frases especiais tornam cada pe&ccedil;a &uacute;nica. Ideal para celebrar anivers&aacute;rios, maternidade, ch&aacute;s ou momentos marcantes, esse presente emociona e cria mem&oacute;rias que permanecem no cora&ccedil;&atilde;o.</p>";
            itemProduto.Tags = "";
            itemProduto.TituloSEO = "";
            itemProduto.DescricaoSEO = "";
            itemProduto.Marca = produto.TryGetProperty("offers", out var offers2) && offers2.TryGetProperty("seller", out var seller) && seller.TryGetProperty("name", out var sellerName) ? sellerName.GetString() ?? "" : "";
            itemProduto.ProdutoFisico = "Sim";
            itemProduto.Mpn = "";
            itemProduto.Sexo = "";
            itemProduto.FaixaEtaria = "";
            itemProduto.Custo = "";

            var productPost = ConverterParaProductPostModel(itemProduto, categoriaIds, "https://www.youtube.com/watch?v=cVqnhhJr2As");


            var parameters = new List<Parameter>
                {
                    new Parameter
                    {
                        Name = null,
                        Type = ParameterType.RequestBody,
                        ContentType = "application/json",
                        Value = productPost // objeto a ser serializado automaticamente
                    }
                };
            var retorno = apiNuvem.PostData<dynamic>("/products", parameters.ToArray());

        }

        return Ok();
    }



    // Tornar este método privado para que não seja exposto como action HTTP
    private ProductPostModel ConverterParaProductPostModel(Produto produto, List<int> categoriaIds, string videoUrl = "")
    {
        return new ProductPostModel
        {
            attributes = new List<ProductAttribute>(), // Pode popular se tiver atributos específicos
            categories = categoriaIds,
            created_at = null, // preencher se desejar
            description = new NomeMultilanguage
            {
                en = produto.Descricao, // se tiver multilínguas, adaptar aqui
                es = produto.Descricao,
                pt = produto.Descricao
            },
            handle = new NomeMultilanguage
            {
                en = GerarIdentificadorUrl(produto.NomepUrl),
                es = GerarIdentificadorUrl(produto.NomepUrl),
                pt = GerarIdentificadorUrl(produto.NomepUrl)
            },
            id = null, // novo produto, id não definido
            images = new List<Image>
        {
            new Image { src = produto.ImgUrl },
            new Image { src = "https://dcdn-us.mitiendanube.com/stores/006/563/772/products/foto-cores-rgb-1de59c7d81392b4ed917570102583683-1024-1024.webp" },
            new Image { src = "https://dcdn-us.mitiendanube.com/stores/006/563/772/products/base-branca-b4-7-cores-7ee0e5ccb3f4de466d17570093157526-1024-1024.webp" },
            new Image { src = "https://dcdn-us.mitiendanube.com/stores/006/563/772/products/base-preta-b1-7-cores-3c976d7a986830521217570087425776-1024-1024.webp" }
            // Pode adicionar mais imagens aqui
        },
            name = new NomeMultilanguage
            {
                en = produto.Nome,
                es = produto.Nome,
                pt = produto.Nome
            },
            brand = string.IsNullOrEmpty(produto.Marca) ? null : new Brand { name = produto.Marca },
            video_url = videoUrl,
            seo_title = produto.TituloSEO ?? produto.Nome,
            seo_description = produto.DescricaoSEO ?? "",
            published = produto.ExibirNaLoja?.ToLower() == "sim",
            free_shipping = produto.FreteGratis?.ToLower() == "sim",
            variants = new List<Variant>
        {
            new Variant
            {
                price = produto.Preco,
                stock_management = true,
                promotional_price = produto.PrecoPromocional,
                stock = int.TryParse(produto.Estoque, out var estoque) ? estoque : 0,
                weight = produto.PesoKg,
                cost = produto.Custo,
                sku = produto.Sku,
                // Pode adicionar dimensões se disponível: height, width, depth
            }
        },
            tags = produto.Tags ?? ""
        };
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

    internal static CredentialsNuvemShop GetFullCredentials()
    {
        return new CredentialsNuvemShop
        {
            AccessToken = "84e2517d9b94b3e5cf40f1afaaab2d3dddd4a340",
            AppId = "6563772",
            AppSecret = "ab8f4cd11d1307245e9cfe33acc841f5bd8f96ad1bb26a24",
            StoreId = "21103"
        };
    }
}
