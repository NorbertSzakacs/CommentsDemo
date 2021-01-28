using System.Linq;

using CommentsDemo.Common;
using CommentsDemo.Core;

using NUnit.Framework;

namespace CommentsDemo.Core_uTest
{
    public class DataAccessTest
    {
        [SetUp]
        public void SetUp()
        {
            this.sut = new DataAccess();
        }

        [Test]
        [Explicit]
        // TODO: update with IAzureTableAccess moq
        public void GetProduct_ExisingProduct_ProductReturned()
        {
            // Arrange
            string productName = "SampleProductName";
            string comment = "SampleComment";
            this.sut.AddComment(productName, comment);

            // Act
            ProductDTO foundProduct = this.sut.GetProduct(productName);

            // Assert
            Assert.That(foundProduct, Is.Not.Null);
            Assert.That(foundProduct.Comments.Count, Is.EqualTo(1));
            Assert.That(foundProduct.Comments.ElementAt(0).Content, Is.EqualTo(comment));
        }

        [Test]
        [Explicit]
        // TODO: update with IAzureTableAccess moq
        public void GetProduct_MissingProduct_NullReturned()
        {
            // Arrange
            string productName = "SampleProductName";
            string comment = "SampleComment";
            this.sut.AddComment(productName, comment);

            // Act
            ProductDTO foundProduct = this.sut.GetProduct("NotExistingProduct");

            // Assert
            Assert.That(foundProduct, Is.Null);
        }

        [Test]
        [Explicit]
        // TODO: update with IAzureTableAccess moq
        public void AddComment_NewProduct_ProductCreatedWithComment()
        {
            // Arrange
            string productName = "SampleProductName";
            string comment = "SampleComment";

            // Act
            this.sut.AddComment(productName, comment);

            // Assert
            ProductDTO found = this.sut.GetProduct(productName);

            Assert.That(found, Is.Not.Null);

            bool commentFound = found.Comments.Any(p => p.Content.Equals(comment));
            Assert.That(commentFound, Is.True);
            Assert.That(found.Comments.Count, Is.EqualTo(1));
        }     

        private DataAccess sut;
    }
}