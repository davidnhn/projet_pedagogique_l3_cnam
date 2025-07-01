using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using JeuVideo.Data.Database;

namespace JeuVideo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration des contrôleurs
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configuration pour éviter les références circulaires
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // Injection de dépendances - Services
            services.AddScoped<GameApiService>();
            
            // Injection de dépendances - Repositories
            services.AddScoped<JoueurRepository>();
            services.AddScoped<PersonnageRepository>();
            services.AddScoped<ObjetRepository>();
            services.AddScoped<ZoneRepository>();
            services.AddScoped<QueteRepository>();
            services.AddScoped<BotRepository>();
            services.AddScoped<CombatRepository>();
            services.AddScoped<TypePersonnageRepository>();
            services.AddScoped<InventaireRepository>();
            services.AddScoped<SauvegardeRepository>();

            // Configuration CORS pour Godot
            services.AddCors(options =>
            {
                options.AddPolicy("GodotPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Configuration Swagger pour la documentation API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Jeu Video API", 
                    Version = "v1",
                    Description = "API pour le jeu vidéo avec gestion des joueurs, combats, objets, etc."
                });
                
                // Inclusion des commentaires XML
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // Configuration de la base de données
            services.AddSingleton<DatabaseConnection>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jeu Video API V1");
                    c.RoutePrefix = string.Empty; // Swagger à la racine
                });
            }

            app.UseRouting();
            
            // Activation de CORS
            app.UseCors("GodotPolicy");

            // TODO: Ajouter l'authentification plus tard
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                // Route de santé
                endpoints.MapGet("/health", async context =>
                {
                    await context.Response.WriteAsync("API is running!");
                });
            });
        }
    }
} 