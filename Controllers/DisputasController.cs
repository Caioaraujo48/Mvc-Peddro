using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RpgMvc.Models;

namespace RpgMvc.Controllers
{
    public class DisputasController : Controller
    {
        public string uriBase = "http://ddvieira.somee.com/RpgApi/Disputas/";

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                //Criação da variável http e obtenção do token guardado na session
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Definição da rota da API que buscará a lista de personagens na API, retornando uma lista se o método tiver êxito ou uma mensagem caso dê erro. Isso tudo guardando ainda serializado.
                string uriBuscaPersonagens = "http://ddvieira.somee.com/RpgApi/Personagens/GetAll";
                HttpResponseMessage response = await httpClient.GetAsync(uriBuscaPersonagens);
                string serialized = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<PersonagemViewModel> listaPersonagens = await Task.Run(() => JsonConvert.DeserializeObject<List<PersonagemViewModel>>(serialized));

                    ViewBag.ListAtacantes = listaPersonagens;
                    ViewBag.ListOponentes = listaPersonagens;
                    return View();
                }
                else   
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
            TempData["MensagemErro"] = ex.Message;
            return RedirectToAction("Index");
            }
        }
    }
}