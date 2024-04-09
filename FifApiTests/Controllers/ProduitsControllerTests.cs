using Microsoft.AspNetCore.Http;
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
using System.Text.Json;
using FifApi.Models.Products;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class ProduitsControllerTests
    {
        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Filtered_Produits_by_All()
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

                dbContext.TypeProduits.AddRange(new[]
                {
            new TypeProduit { Id = 1, Nom = "SomeCategory", Description = "Description du type 1" },
            new TypeProduit { Id = 2, Nom = "SomeCategoryForTest", Description = "Description du type 2" },
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
               
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));


                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                foreach (var produit in (IEnumerable<object>)result)
                {
                    var properties = produit.GetType().GetProperties();

                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                }

            }
        }


        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Filtered_Produits_by_Couleur()
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
                var actionResult = await controller.GetProduitsByFilter("Rouge", null, null, null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
            
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));


                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                foreach (var produit in (IEnumerable<object>)result)
                {
                    var properties = produit.GetType().GetProperties();

                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                }

            }
        }


        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Filtered_Produits_by_Nation()
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
                var actionResult = await controller.GetProduitsByFilter(null, "France", null, null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
               
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));


                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                foreach (var produit in (IEnumerable<object>)result)
                {
                    var properties = produit.GetType().GetProperties();

                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                }

            }
        }

        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Filtered_Produits_by_Categorie()
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

                dbContext.TypeProduits.AddRange(new[]
                {
            new TypeProduit { Id = 1, Nom = "SomeCategory", Description = "Description du type 1" },
            new TypeProduit { Id = 2, Nom = "SomeCategoryForTest", Description = "Description du type 2" },
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
                var actionResult = await controller.GetProduitsByFilter(null, null, "SomeCategoryForTest", null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));


                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                foreach (var produit in (IEnumerable<object>)result)
                {
                    var properties = produit.GetType().GetProperties();

                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                }

            }
        }


        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Filtered_Produits_by_Taille()
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

                dbContext.TypeProduits.AddRange(new[]
                {
            new TypeProduit { Id = 1, Nom = "SomeCategory", Description = "Description du type 1" },
            new TypeProduit { Id = 2, Nom = "SomeCategoryForTest", Description = "Description du type 2" },
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
                var actionResult = await controller.GetProduitsByFilter(null, null, null, "L");
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

                    Assert.IsTrue(properties.Any(p => p.Name == "idProduct"));
                    Assert.IsTrue(properties.Any(p => p.Name == "title"));
                    Assert.IsTrue(properties.Any(p => p.Name == "description"));
                    Assert.IsTrue(properties.Any(p => p.Name == "caracteristiques"));
                    Assert.IsTrue(properties.Any(p => p.Name == "image"));
                    Assert.IsTrue(properties.Any(p => p.Name == "couleurs"));
                    Assert.IsTrue(properties.Any(p => p.Name == "quantite"));

                }

            }
        }

        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Couleur_null()
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
                var actionResult = await controller.GetProduitsByFilter("Magenta", null, null, null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));

                var jsonBody = JsonSerializer.Serialize(result);

                var expectedJson = "[]";

                Assert.AreEqual(expectedJson, jsonBody);
            }
        }


        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Nation_null()
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

                // Act

                dbContext.SaveChanges();

                var controller = new ProduitsController(dbContext);

                
                var actionResult = await controller.GetProduitsByFilter(null, "Ukraine", null, null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));

                var jsonBody = JsonSerializer.Serialize(result);

                var expectedJson = "[]";

                Assert.AreEqual(expectedJson, jsonBody);
            }
        }

        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Categorie_null()
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

                // Act

                dbContext.SaveChanges();

                var controller = new ProduitsController(dbContext);

                
                var actionResult = await controller.GetProduitsByFilter(null, null, "unCategoriealazob", null);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));

                var jsonBody = JsonSerializer.Serialize(result);

                var expectedJson = "[]";

                Assert.AreEqual(expectedJson, jsonBody);
            }
        }

        [TestMethod]
        public async Task Get_Produits_By_Filter_Returns_Taille_null()
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


                // Act

                dbContext.SaveChanges();

                var controller = new ProduitsController(dbContext);

                var actionResult = await controller.GetProduitsByFilter(null, null, null, "XXXXXXXXL");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(ActionResult<object>));

                var jsonBody = JsonSerializer.Serialize(result);

                var expectedJson = "[]";

                Assert.AreEqual(expectedJson, jsonBody);
            }
        }



        [TestMethod]
        public async Task Post_Produit_Returns_CreatedAtAction()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();

                var actionResult = await controller.PostProduit(produitToAdd);
                var okResult = actionResult.Result as NoContentResult;

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NoContentResult));


                Assert.IsNotNull(okResult);

                Assert.AreEqual(204, okResult.StatusCode);


            }
        }


        [TestMethod]
        public async Task Post_Produit_Returns_No_Marque()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Couleurs = couleurs
            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();

                var actionResult = await controller.PostProduit(produitToAdd);
                var okResult = actionResult.Result as NoContentResult;

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }


        [TestMethod]
        public async Task Post_Produit_Returns_Marque_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Marque = 1,
                Couleurs = couleurs
            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);
                //Act
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();

                var actionResult = await controller.PostProduit(produitToAdd);
                var okResult = actionResult.Result as NoContentResult;

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }


        [TestMethod]
        public async Task Post_Produit_Returns_No_Name()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));


            }
        }





        [TestMethod]
        public async Task Post_Produit_Returns_taille_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs

            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };

            using (var dbContext = CreateDbContext()) 
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }

        [TestMethod]
        public async Task Post_Produit_Returns_typeProduit_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs

            };

            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };
            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Tailles.Add(taille);
                dbContext.Marques.Add(marque);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }

        [TestMethod]
        public async Task Post_Produit_Returns_Nation_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs

            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }


        [TestMethod]
        public async Task Post_Produit_Returns_No_Nation()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs

            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


            }
        }

        [TestMethod]
        public async Task Post_Produit_Returns_No_Couleur()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,

            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Couleurs.Add(couleur);
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));


            }
        }


        [TestMethod]
        public async Task Post_Produit_Returns_Couleur_Dont_Exist()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {
                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs

            };

            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Tailles.Add(taille);

                dbContext.SaveChanges();
                var actionResult = await controller.PostProduit(produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));


            }
        }

        [TestMethod]
        public async Task Put_Produit_Returns_CreatedAtAction()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
                var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                // Assert

                Assert.IsNotNull(actionResult);

                Assert.IsNotNull(actionResult);

                var statusCodeResult = actionResult as StatusCodeResult;

                Assert.IsNotNull(statusCodeResult);

                Assert.AreEqual(204, statusCodeResult.StatusCode);

            }
        }



        [TestMethod]
        public async Task Put_Produit_Returns_Marque_Dont_Exist()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
              
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));

             


            }
        }

        [TestMethod]
        public async Task Put_Produit_Returns_No_Marque()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
               
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));

              


            }
        }
        [TestMethod]
        public async Task Put_Produit_Returns_Taille_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Marque = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();

                var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));




            }
        }

        [TestMethod]
        public async Task Put_Produit_Returns_TypeProduit_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Marque = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Tailles.Add(taille);
                dbContext.Marques.Add(marque);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();

                var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));




            }
        }


        [TestMethod]
        public async Task Put_Produit_Returns_Produit_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Marque = 1,
                Couleurs = couleurs
            };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Tailles.Add(taille);
                dbContext.Marques.Add(marque);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.TypeProduits.Add(typeProduit);

                dbContext.SaveChanges();

                var actionResult = await controller.PutProduit(1, produitToAdd);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));




            }
        }

        [TestMethod]
        public async Task Put_Produit_Returns_Pays_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
              
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));

            


            }
        }


        [TestMethod]
        public async Task Put_Produit_Returns_No_Pays()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
               
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));



            }
        }


        [TestMethod]
        public async Task Put_Produit_Returns_Couleur_Dont_Exists()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
                Couleurs = couleurs
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
                
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

                
                


            }
        }

        [TestMethod]
        public async Task Put_Produit_Returns_No_Couleur()
        {
            var tailles = new List<Size> { new Size { Code = "XL", Quantite = 45 } };
            var couleurs = new List<Color> { new Color { Nom = "rouge", Prix = 53, Tailles = tailles } };
            // Arrange
            var produitToAdd = new Product
            {

                NomProduit = "Nouveau Produit",
                Image = "bonjour.jpg",
                DescriptionProduit = "Description du nouveau produit",
                Nation = "FRA",
                Marque = 1,
                Categorie = 1,
            };
            var produit = new Produit { Id = 1, Name = produitToAdd.NomProduit, Description = produitToAdd.DescriptionProduit, MarqueId = produitToAdd.Marque, PaysId = produitToAdd.Nation };
            var typeProduit = new TypeProduit { Description = "fff", Nom = "bonjour", Id = 1 };
            var marque = new Marque { NomMarque = "bonjour", IdMarque = 1 };
            var pays = new Pays { NomPays = "FRANCE", IdPays = "FRA" };
            var couleur = new Couleur { Hexa = "FF0000", Id = 1, Nom = "rouge" };
            var taille = new Taille { DescriptionTaille = "bonjour", IdTaille = "XL", NomTaille = "Très large" };

            using (var dbContext = CreateDbContext()) // Create your DbContext here
            {
                var controller = new ProduitsController(dbContext);

                // Act
                dbContext.Marques.Add(marque);
                dbContext.TypeProduits.Add(typeProduit);
                dbContext.Pays.Add(pays);
                dbContext.Couleurs.Add(couleur);
                dbContext.Tailles.Add(taille);
                dbContext.Produits.Add(produit);
                dbContext.SaveChanges();
          
                    var actionResult = await controller.PutProduit(produit.Id, produitToAdd);

                    // Assert
                    Assert.IsNotNull(actionResult);
                    Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));



            }
        }

        [TestMethod]
        public async Task Delete_Produit_Returns_NoContentResult()
        {
            // Arrange
            var produitToDelete = new Produit { Id = 1, Name = "Produit à supprimer", Description = "Description du produit à supprimer", PaysId = "fr" }; 
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

                var deletedProduit = await dbContext.Produits.FindAsync(produitToDelete.Id);
                Assert.IsNull(deletedProduit);
            }
        }

        [TestMethod]
        public async Task Delete_Produit_With_Invalid_Id_Returns_NotFoundResult()
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
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())); 

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }

    }
}
