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
    public class JoueurController : ControllerBase
    {
        private readonly GameApiService _gameApiService;
        private readonly JoueurRepository _joueurRepository;

        public JoueurController(GameApiService gameApiService, JoueurRepository joueurRepository)
        {
            _gameApiService = gameApiService;
            _joueurRepository = joueurRepository;
        }

        /// <summary>
        /// Récupère tous les joueurs
        /// GET /api/joueur
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Joueur>>> GetAllJoueurs()
        {
            try
            {
                var joueurs = await _joueurRepository.GetAllAsync();
                return Ok(joueurs);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère un joueur par son ID avec ses personnages
        /// GET /api/joueur/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetJoueur(int id)
        {
            try
            {
                var (joueur, personnages) = await _gameApiService.GetJoueurCompletAsync(id);
                
                if (joueur == null)
                {
                    return NotFound(new { message = "Joueur introuvable" });
                }

                return Ok(new 
                { 
                    joueur = joueur,
                    personnages = personnages
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Crée un nouveau joueur avec un personnage initial
        /// POST /api/joueur
        /// Body: { "pseudoJoueur": "PlayerOne", "nomPersonnage": "Hero", "typePersonnageId": 1 }
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Joueur>> CreerJoueur([FromBody] CreerJoueurRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PseudoJoueur) || string.IsNullOrEmpty(request.NomPersonnage))
                {
                    return BadRequest(new { message = "Pseudo et nom de personnage requis" });
                }

                var joueur = await _gameApiService.CreerNouveauJoueurAsync(
                    request.PseudoJoueur, 
                    request.NomPersonnage, 
                    request.TypePersonnageId
                );

                if (joueur == null)
                {
                    return StatusCode(500, new { message = "Erreur lors de la création" });
                }

                return CreatedAtAction(nameof(GetJoueur), new { id = joueur.IdJoueur }, joueur);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Met à jour un joueur
        /// PUT /api/joueur/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateJoueur(int id, [FromBody] Joueur joueur)
        {
            try
            {
                if (id != joueur.IdJoueur)
                {
                    return BadRequest(new { message = "ID incohérent" });
                }

                var success = await _joueurRepository.UpdateAsync(joueur);

                if (!success)
                {
                    return NotFound(new { message = "Joueur introuvable" });
                }

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        /// <summary>
        /// Supprime un joueur
        /// DELETE /api/joueur/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJoueur(int id)
        {
            try
            {
                var success = await _joueurRepository.DeleteAsync(id);

                if (!success)
                {
                    return NotFound(new { message = "Joueur introuvable" });
                }

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO pour la création d'un joueur
    /// </summary>
    public class CreerJoueurRequest
    {
        public string PseudoJoueur { get; set; }
        public string NomPersonnage { get; set; }
        public int TypePersonnageId { get; set; } = 1;
    }
} 