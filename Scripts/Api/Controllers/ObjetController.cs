using Microsoft.AspNetCore.Mvc;
using JeuVideo.Data;
using JeuVideo.Data.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JeuVideo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ObjetController : ControllerBase
    {
        private readonly GameApiService _gameApiService;
        private readonly ObjetRepository _objetRepository;

        public ObjetController(GameApiService gameApiService, ObjetRepository objetRepository)
        {
            _gameApiService = gameApiService;
            _objetRepository = objetRepository;
        }

        /// <summary>
        /// Récupère tous les objets
        /// GET /api/objet
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Objet>>> GetAllObjets()
        {
            try
            {
                var objets = await _objetRepository.GetAllAsync();
                return Ok(objets);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère un objet par son ID
        /// GET /api/objet/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Objet>> GetObjet(int id)
        {
            try
            {
                var objet = await _objetRepository.GetByIdAsync(id);

                if (objet == null)
                {
                    return NotFound(new { message = "Objet introuvable" });
                }

                return Ok(objet);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Utilise un objet sur un personnage (utilise la logique métier d'Objet.cs)
        /// POST /api/objet/{objetId}/utiliser/{personnageId}
        /// </summary>
        [HttpPost("{objetId}/utiliser/{personnageId}")]
        public async Task<ActionResult<object>> UtiliserObjet(int objetId, int personnageId)
        {
            try
            {
                var resultat = await _gameApiService.UtiliserObjetAsync(personnageId, objetId);

                if (resultat.Contains("introuvable"))
                {
                    return NotFound(new { message = resultat });
                }

                return Ok(new { resultat = resultat });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Recherche des objets par effet
        /// GET /api/objet/recherche/effet/{effet}
        /// </summary>
        [HttpGet("recherche/effet/{effet}")]
        public async Task<ActionResult<List<Objet>>> RechercherObjetParEffet(string effet)
        {
            try
            {
                var objets = await _objetRepository.GetByEffetAsync(effet);
                return Ok(objets);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Crée un nouvel objet
        /// POST /api/objet
        /// Body: { "nomObjet": "Potion", "descriptionNomObjet": "Restore 50 HP", "effet": "vie +50" }
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Objet>> CreerObjet([FromBody] Objet objet)
        {
            try
            {
                if (string.IsNullOrEmpty(objet.NomObjet))
                {
                    return BadRequest(new { message = "Nom de l'objet requis" });
                }

                var objetCree = await _objetRepository.CreateAsync(objet);

                if (objetCree == null)
                {
                    return StatusCode(500, new { message = "Erreur lors de la création" });
                }

                return CreatedAtAction(nameof(GetObjet), new { id = objetCree.IdObjet }, objetCree);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }
    }
} 