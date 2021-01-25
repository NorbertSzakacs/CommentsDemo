using System.Collections.Generic;
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
        public void AddComment_NewProduct_ProductCreatedWithComment()
        {
            // Arrange
            string productName = "SampleProductName";
            string comment = "SampleComment";

            // Act
            bool result = this.sut.AddComment(productName, comment);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            List<ProductDTO> productContent = this.sut.GetProducts().ToList();

            ProductDTO found = productContent.FirstOrDefault(p => p.ProductName.Equals(productName));
            Assert.That(found, Is.Not.Null);

            bool commentFound = found.Comments.Any(p => p.Content.Equals(comment));
            Assert.That(commentFound, Is.True);
            Assert.That(found.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddComment_ExisingProduct_CommentAddedInFirstPlace()
        {
            // Arrange
            string productName = "SampleProductName";
            string comment = "SampleComment";
            string newlyAddedComment = "AnotherComment";
            this.sut.AddComment(productName, comment);

            // Act
            bool result = this.sut.AddComment(productName, newlyAddedComment);

            // Assert
            Assert.That(result, Is.EqualTo(false));

            List<ProductDTO> productContent = this.sut.GetProducts().ToList();

            ProductDTO foundProduct = productContent.FirstOrDefault(p => p.ProductName.Equals(productName));
            Assert.That(foundProduct, Is.Not.Null);
                        
            Assert.That(foundProduct.Comments.Count, Is.EqualTo(2));
            Assert.That(foundProduct.Comments.ElementAt(0).Content, Is.EqualTo(newlyAddedComment));
            Assert.That(foundProduct.Comments.ElementAt(1).Content, Is.EqualTo(comment));
        }

        private DataAccess sut;
    }
}