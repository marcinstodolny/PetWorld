namespace PetWorld.Domain.Enums;

public enum ProductCategory
{
    DogFood = 1,
    CatFood = 2,
    Aquatics = 3,
    CatAccessories = 4,
    DogToys = 5,
    SmallPets = 6,
    DogAccessories = 7
}

public static class ProductCategoryLabels
{
    public static string ToPolish(this ProductCategory category) => category switch
    {
        ProductCategory.DogFood => "Karma dla psów",
        ProductCategory.CatFood => "Karma dla kotów",
        ProductCategory.Aquatics => "Akwarystyka",
        ProductCategory.CatAccessories => "Akcesoria dla kotów",
        ProductCategory.DogToys => "Zabawki dla psów",
        ProductCategory.SmallPets => "Gryzonie",
        ProductCategory.DogAccessories => "Akcesoria dla psów",
        _ => category.ToString()
    };
}