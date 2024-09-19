using System.Globalization;

namespace ProductRepositoryAsync;

/// <summary>
/// Represents a product storage service and provides a set of methods for managing the list of products.
/// </summary>
public class ProductRepository(string productCollectionName, IDatabase database) : IProductRepository
{
    private readonly string productCollectionName = productCollectionName;
    private readonly IDatabase database = database;

    public async Task<int> AddProductAsync(Product product)
    {
        ValidateProduct(product);

        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            result = await this.database.CreateCollectionAsync(this.productCollectionName);

            if (result == OperationResult.ConnectionIssue)
            {
                throw new DatabaseConnectionException();
            }
            else if (result != OperationResult.Success)
            {
                throw new RepositoryException("Failed to create the product collection.");
            }
        }

        result = await this.database.GenerateIdAsync(this.productCollectionName, out var productId);
        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException("Failed to generate product ID.");
        }

        var productData = new Dictionary<string, string>
    {
        { "name", product.Name },
        { "category", product.Category },
        { "price", product.UnitPrice.ToString(CultureInfo.InvariantCulture) },
        { "in-stock", product.UnitsInStock.ToString(CultureInfo.InvariantCulture) },
        { "discontinued", product.Discontinued.ToString() },
    };

        result = await this.database.InsertCollectionElementAsync(this.productCollectionName, productId, productData);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException("Failed to add the product.");
        }

        return productId;
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        result = await this.database.GetCollectionElementAsync(this.productCollectionName, productId, out IDictionary<string, string> data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        return new Product
        {
            Id = productId,
            Name = data["name"],
            Category = data["category"],
            UnitPrice = decimal.Parse(data["price"], CultureInfo.InvariantCulture),
            UnitsInStock = int.Parse(data["in-stock"], CultureInfo.InvariantCulture),
            Discontinued = bool.Parse(data["discontinued"]),
        };
    }

    public async Task RemoveProductAsync(int productId)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        result = await this.database.DeleteCollectionElementAsync(this.productCollectionName, productId);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException("Failed to delete the product.");
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentException("Product cannot be null.", nameof(product));
        }

        if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Category))
        {
            throw new ArgumentException("Product name and category must not be empty.", nameof(product));
        }

        if (product.UnitPrice < 0 || product.UnitsInStock < 0)
        {
            throw new ArgumentException("Product price and stock must be non-negative.", nameof(product));
        }

        var result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, product.Id, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        var productData = new Dictionary<string, string>
    {
        { "name", product.Name },
        { "category", product.Category },
        { "price", product.UnitPrice.ToString(CultureInfo.InvariantCulture) },
        { "in-stock", product.UnitsInStock.ToString(CultureInfo.InvariantCulture) },
        { "discontinued", product.Discontinued.ToString() },
    };

        result = await this.database.UpdateCollectionElementAsync(this.productCollectionName, product.Id, productData);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException("Failed to update the product.");
        }
    }

    private static void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name cannot be empty or whitespace.", nameof(product));
        }

        if (string.IsNullOrWhiteSpace(product.Category))
        {
            throw new ArgumentException("Product category cannot be empty or whitespace.", nameof(product));
        }

        if (product.UnitPrice < 0)
        {
            throw new ArgumentException("Product unit price must be greater than or equal to zero.", nameof(product));
        }

        if (product.UnitsInStock < 0)
        {
            throw new ArgumentException("Product units in stock must be greater than or equal to zero.", nameof(product));
        }
    }
}
