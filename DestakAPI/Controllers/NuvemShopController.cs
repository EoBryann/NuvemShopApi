
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

[ApiController]
[Route("[controller]")]
public class NuvemShopController : ControllerBase
{

    [HttpPost("ExemploNuvemShop")]
    public  IActionResult ExemploNuvemShop()
    {

        var apiNuvem = new ClientNuvemShop("Bryanporto02@GMAIL.COM", "DestakAPP", GetFullCredentials());

        var retorno = apiNuvem.GetData<dynamic>("/products");

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
