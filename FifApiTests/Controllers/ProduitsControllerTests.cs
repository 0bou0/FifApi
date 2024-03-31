﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using FifApi.Models;
using FifApi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class ProduitsControllerTests
    {
        [TestMethod]
        public async Task GetProduitsByFilter_ReturnsFilteredProduits()
        {
            // Arrange
            var pays = new Pays { IdPays = "fr", NomPays = "France" };

            using (var dbContext = CreateDbContext())
            {
                dbContext.Produits.AddRange(new[]
   {
        new Produit { Id = 1, Name = "Produit 1", Description = "Description du produit 1", Caracteristiques = "Caractéristiques du produit 1", AlbumId = 1, PaysId = pays.IdPays },
        new Produit { Id = 2, Name = "Produit 2", Description = "Description du produit 2", Caracteristiques = "Caractéristiques du produit 2", AlbumId = 2, PaysId = pays.IdPays },
        new Produit { Id = 3, Name = "Produit 3", Description = "Description du produit 3", Caracteristiques = "Caractéristiques du produit 3", AlbumId = 3, PaysId = pays.IdPays }
    });

                dbContext.Albums.AddRange(new[]
                {
            new Album { IdAlbum = 1 },
            new Album { IdAlbum = 2 },
            new Album { IdAlbum = 3 }
        });

                dbContext.AlbumPhotos.AddRange(new[]
                {
            new AlbumPhoto { IdAlbum = 1, IdPhoto = 1 },
            new AlbumPhoto { IdAlbum = 2, IdPhoto = 2 },
            new AlbumPhoto { IdAlbum = 3, IdPhoto = 3 }
        });

                dbContext.Photos.AddRange(new[]
                {
            new Photo { IdPhoto = 1, URL = "https://www.foot.fr/70450-pdt_360/maillot-equipe-de-france-domicile-2020.jpg" },
            new Photo { IdPhoto = 2, URL = "https://example.com/image2.jpg" },
            new Photo { IdPhoto = 3, URL = "https://example.com/image3.jpg" }
        });

                dbContext.Couleurs.AddRange(new[]
                {
            new Couleur { Id = 1, Nom = "Rouge", Hexa = "#FF0000" },
            new Couleur { Id = 2, Nom = "Bleu", Hexa = "#0000FF" },
            new Couleur { Id = 3, Nom = "Vert", Hexa = "#00FF00" },
            new Couleur { Id = 4, Nom = "Jaune", Hexa = "#FFFF00" },
            new Couleur { Id = 5, Nom = "Orange", Hexa = "#FF8800" },
            new Couleur { Id = 6, Nom = "Turquoise", Hexa = "#00FFFF" }
        });

                dbContext.CouleurProduits.AddRange(new[]
                {
            new CouleurProduit { IdCouleurProduit = 1, IdProduit = 1, IdCouleur = 1, Prix = 30, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 2, IdProduit = 1, IdCouleur = 2, Prix = 25, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 3, IdProduit = 1, IdCouleur = 3, Prix = 32, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 4, IdProduit = 1, IdCouleur = 4, Prix = 25, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 5, IdProduit = 1, IdCouleur = 5, Prix = 32, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 6, IdProduit = 1, IdCouleur = 6, Prix = 29, CodeBarre = "123456789012" },
            new CouleurProduit { IdCouleurProduit = 7, IdProduit = 3, IdCouleur = 4, Prix = 50, CodeBarre = "345678901234" },
            new CouleurProduit { IdCouleurProduit = 8, IdProduit = 3, IdCouleur = 5, Prix = 51, CodeBarre = "345678901234" },
            new CouleurProduit { IdCouleurProduit = 9, IdProduit = 3, IdCouleur = 6, Prix = 55, CodeBarre = "345678901234" }
        });

                dbContext.Tailles.AddRange(new[]
                {
            new Taille { IdTaille = "S", NomTaille = "Small", DescriptionTaille = "Petite taille" },
            new Taille { IdTaille = "M", NomTaille = "Medium", DescriptionTaille = "Taille moyenne" },
            new Taille { IdTaille = "L", NomTaille = "Large", DescriptionTaille = "Grande taille" }
        });

                dbContext.Stocks.AddRange(new[]
                {
            new Stock { IdStock = 1,   CouleurProduitId = 1, TailleId = "S", Quantite = 14 },
            new Stock { IdStock = 2,  CouleurProduitId = 1, TailleId = "S", Quantite = 10 },
            new Stock { IdStock = 3,  CouleurProduitId = 1, TailleId = "L", Quantite = 50 },
            new Stock { IdStock = 4, CouleurProduitId = 2, TailleId = "M", Quantite = 28 },
            new Stock { IdStock = 5, CouleurProduitId = 2, TailleId = "M", Quantite = 100 },
            new Stock { IdStock = 6, CouleurProduitId = 2, TailleId = "L", Quantite = 40 },
            new Stock { IdStock = 7, CouleurProduitId = 3, TailleId = "L", Quantite = 20 },
            new Stock { IdStock = 8, CouleurProduitId = 3, TailleId = "L", Quantite = 20 },
            new Stock { IdStock = 9, CouleurProduitId = 3, TailleId = "M", Quantite = 62 },
            new Stock { IdStock = 10, CouleurProduitId = 4, TailleId = "S", Quantite = 50 }
        });

                dbContext.SaveChanges();

                var controller = new ProduitsController(dbContext);

                // Act
                var actionResult = await controller.GetProduitsByFilter("Rouge", "France", "SomeCategory", "M");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));


                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                foreach (var produit in (IEnumerable<object>)result)
                {
                    var properties = produit.GetType().GetProperties();

                    // Vérifiez si chaque propriété attendue existe dans le produit retourné
                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                    // Vous pouvez également vérifier le contenu des propriétés si nécessaire
                }

            }
        }


        [TestMethod]
        public async Task PutProduit_ReturnsNoContentResult()
        {
            // Arrange
            var id = 1;
            var existingProduit = new Produit { Id = id, Name = "Produit existant", Description = "Description du produit existant", PaysId = "fr" }; // Assurez-vous de définir PaysId

            using (var dbContext = CreateDbContext())
            {
                dbContext.Produits.Add(existingProduit);
                await dbContext.SaveChangesAsync();

                // Assurez-vous que l'objet est détaché du contexte de la base de données
                dbContext.Entry(existingProduit).State = EntityState.Detached;

                var updatedProduit = new Produit { Id = id, Name = "Produit 1 mis à jour", Description = "Nouvelle description", PaysId = "fr" }; // Assurez-vous de définir PaysId

                var controller = new ProduitsController(dbContext);

                // Act
                var actionResult = await controller.PutProduit(id, updatedProduit);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            }
        }


        [TestMethod]
        public async Task PostProduit_ReturnsCreatedAtAction()
        {
            // Arrange
            var produitToAdd = new Produit { Id = 1, Name = "Nouveau produit", Description = "Description du nouveau produit", PaysId = "fr" }; // Assurez-vous de définir PaysId

            using (var dbContext = CreateDbContext())
            {
                var controller = new ProduitsController(dbContext);

                // Act
                var actionResult = await controller.PostProduit(produitToAdd);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual("GetProduit", createdAtActionResult.ActionName);
                Assert.AreEqual(201, createdAtActionResult.StatusCode);

                var produitReturned = createdAtActionResult.Value as Produit;
                Assert.IsNotNull(produitReturned);
                Assert.AreEqual(produitToAdd.Id, produitReturned.Id);
                // Assurez-vous de vérifier d'autres propriétés si nécessaire
            }
        }


        [TestMethod]
        public async Task DeleteProduit_ReturnsNoContentResult()
        {
            // Arrange
            var produitToDelete = new Produit { Id = 1, Name = "Produit à supprimer", Description = "Description du produit à supprimer", PaysId = "fr" }; // Assurez-vous de définir PaysId

            using (var dbContext = CreateDbContext())
            {
                dbContext.Produits.Add(produitToDelete);
                await dbContext.SaveChangesAsync();

                var controller = new ProduitsController(dbContext);

                // Act
                var actionResult = await controller.DeleteProduit(produitToDelete.Id);
                var noContentResult = actionResult as NoContentResult;

                // Assert
                Assert.IsNotNull(noContentResult);
                Assert.AreEqual(204, noContentResult.StatusCode);

                // Vérifie si le produit a été correctement supprimé de la base de données
                var deletedProduit = await dbContext.Produits.FindAsync(produitToDelete.Id);
                Assert.IsNull(deletedProduit);
            }
        }

        [TestMethod]
        public async Task DeleteProduit_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidId = -1;

            using (var dbContext = CreateDbContext())
            {
                var controller = new ProduitsController(dbContext);

                // Act
                var actionResult = await controller.DeleteProduit(invalidId);
                var notFoundResult = actionResult as NotFoundResult;

                // Assert
                Assert.IsNotNull(notFoundResult);
                Assert.AreEqual(404, notFoundResult.StatusCode);
            }
        }


        private FifaDBContext CreateDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FifaDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())); // Utiliser un nom de base de données unique à chaque fois

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }

    }
}