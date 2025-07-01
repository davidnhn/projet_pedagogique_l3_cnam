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
    public class CombatController : ControllerBase
    {
        private readonly GameApiService _gameApiService;
        private readonly CombatRepository _combatRepository;

        public CombatController(GameApiService gameApiService, CombatRepository combatRepository)
        {
            _gameApiService = gameApiService;
            _combatRepository = combatRepository;
        }

        /// <summary>
        /// Initie un nouveau combat
        /// POST /api/combat/initier
        /// Body: { "personnageId": 1, "botId": 2, "zoneId": 1 }
        /// </summary>
        [HttpPost("initier")]
        public async Task<ActionResult<Combat>> InitierCombat([FromBody] InitierCombatRequest request)
        {
            try
            {
                if (request.PersonnageId <= 0 || request.BotId <= 0 || request.ZoneId <= 0)
                {
                    return BadRequest(new { message = "IDs invalides" });
                }

                var combat = await _gameApiService.InitierCombatAsync(
                    request.PersonnageId, 
                    request.BotId, 
                    request.ZoneId
                );

                if (combat == null)
                {
                    return StatusCode(500, new { message = "Impossible d'initier le combat" });
                }

                return CreatedAtAction(nameof(GetCombat), new { id = combat.IdCombat }, combat);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère les détails d'un combat
        /// GET /api/combat/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Combat>> GetCombat(int id)
        {
            try
            {
                var combat = await _combatRepository.GetByIdAsync(id);

                if (combat == null)
                {
                    return NotFound(new { message = "Combat introuvable" });
                }

                return Ok(combat);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Gère un tour de combat (utilise la logique métier de Combat.cs)
        /// POST /api/combat/{id}/tour
        /// </summary>
        [HttpPost("{id}/tour")]
        public async Task<ActionResult<object>> GererTourCombat(int id)
        {
            try
            {
                var (resultat, estTermine) = await _gameApiService.GererTourCombatAsync(id);

                if (resultat == "Combat introuvable")
                {
                    return NotFound(new { message = "Combat introuvable" });
                }

                return Ok(new 
                { 
                    resultat = resultat,
                    combatTermine = estTermine,
                    message = estTermine ? "Combat terminé" : "Tour suivant"
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Tente une fuite du combat (utilise la logique métier de Combat.cs)
        /// POST /api/combat/{id}/fuir
        /// </summary>
        [HttpPost("{id}/fuir")]
        public async Task<ActionResult<object>> FuirCombat(int id)
        {
            try
            {
                var resultat = await _gameApiService.TenterFuiteCombatAsync(id);

                if (resultat == "Combat introuvable")
                {
                    return NotFound(new { message = "Combat introuvable" });
                }

                return Ok(new { resultat = resultat });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère tous les combats d'une zone
        /// GET /api/combat/zone/{zoneId}
        /// </summary>
        [HttpGet("zone/{zoneId}")]
        public async Task<ActionResult<List<Combat>>> GetCombatsParZone(int zoneId)
        {
            try
            {
                var combats = await _combatRepository.GetByZoneIdAsync(zoneId);

                return Ok(combats);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère l'historique complet d'un combat
        /// GET /api/combat/{id}/historique
        /// </summary>
        [HttpGet("{id}/historique")]
        public async Task<ActionResult<object>> GetHistoriqueCombat(int id)
        {
            try
            {
                var combat = await _combatRepository.GetByIdAsync(id);

                if (combat == null)
                {
                    return NotFound(new { message = "Combat introuvable" });
                }

                // Utiliser la logique métier de Combat pour obtenir l'historique
                var historique = combat.ObtenirHistorique();

                return Ok(new { historique = historique });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO pour l'initiation d'un combat
    /// </summary>
    public class InitierCombatRequest
    {
        public int PersonnageId { get; set; }
        public int BotId { get; set; }
        public int ZoneId { get; set; }
    }
} 