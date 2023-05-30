using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using System;
using RpgMvc.Models;
using RpgMvc.Models.Enuns;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


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

        [HttpPost]
        public async Task<ActionResult>  IndexAsync (DisputasViewModel disputa)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Arma";

                var content = new StringContent(JsonConvert.SerializeObject(disputa));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    disputa = await Task.Run(() => JsonConvert.DeserializeObject<DisputasViewModel>(serialized));
                    TempData["Mensagem"] = disputa.Narracao;
                    return RedirectToAction("Index", "Personagens");
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

        [HttpGet]
        public async Task<ActionResult> IndexHabilidadesAsync()
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
                }
                else   
                    throw new System.Exception(serialized);

                    string uriBuscaHabilidades = "http://ddvieira.somee.com/RpgApi/PersonagemHabilidades/GetHabilidades";
                    response = await httpClient.GetAsync(uriBuscaHabilidades);
                    serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<HabilidadeViewModel>listaHabilidades = await Task.Run(() =>
                            JsonConvert.DeserializeObject<List<HabilidadeViewModel>>(serialized));
                        ViewBag.ListaHabilidades = listaHabilidades;
                    }
                    else
                        throw new System.Exception(serialized);

                    return View("IndexHabilidades");
            }        
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> IndexHabilidadesAsync(DisputasViewModel disputa)
        {
            try
            {
                
            HttpClient httpClient = new HttpClient();
            string uriComplementar = "Habilidade";
            var content = new StringContent(JsonConvert.SerializeObject(disputa));
            content.Headers.ContentType = new MediaTypeHeaderValue ("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
            string serialized = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                disputa = await Task.Run(() =>
                JsonConvert.DeserializeObject<DisputasViewModel>(serialized));
                TempData["Mensagem"] = disputa.Narracao;
                return RedirectToAction("Index", "Personagens");
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