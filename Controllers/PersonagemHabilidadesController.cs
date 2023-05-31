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
using System.Linq;


namespace RpgMvc.Controllers

{    public class PersonagemHabilidadesController : Controller
    {
        public string uriBase = "http://ddvieira.somee.com/RpgApi/Disputas/";

        [HttpGet("PersonagemHabilidades/{id}")]
        public async Task<ActionResult> IndexAsync(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    PersonagemViewModel p = await Task.Run(() => JsonConvert.DeserializeObject<PersonagemViewModel>(serialized));
                    return View(p);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet("Delete/{habilidadeId}/{personagemId}")]
        public async Task<ActionResult> DeleteAsync(int habilidadeId, int personagemId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "DeletePersonagemHabilidade";
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                PersonagemHabilidadesViewModel ph = new PersonagemHabilidadesViewModel();
                ph.HabilidadeId = habilidadeId;
                ph.PersonagemId = personagemId;

                
            }
        }
    }
}