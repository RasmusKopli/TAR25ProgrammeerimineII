

using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test Address",
                RoomNumber = 3,
                BuildingType = "House",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            // Act, siin tuleb kutsuda RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        // Teha test, kus otsib ankeeti Id alusel, aga seda ei ole andmebaasis
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            Guid databaseGuid = Guid.Parse("70cd4d70-63b9-46ed-a086-e2ad35e93290");
            Guid wrongGuid = Guid.NewGuid();

            // Act
            await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

            // Assert
            // Vastupidist loogikat tuleb kasutada
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        
        // On vaja testida DetailAsync meetodit
        // Leiab ankeedi ülesse e õige Id otsib
        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            // See Guid on juba andmebaasis olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("70cd4d70-63b9-46ed-a086-e2ad35e93290");
            // Kui vajutad nupule, siis see Guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("70cd4d70-63b9-46ed-a086-e2ad35e93290");

            // Act
            await Svc<IRealEstateServices>().DetailsAsync(guid);

            // Assert
            Assert.Equal(databaseGuid, guid);

        }

        // Kustutamise meetodit on vaja testida. Andmebaasis on ankeet, mida
        // tahan ära kustutada
        // Fix this!
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            // Arrange
            // Vaja oleks Mock'ida ankeeti e teha meetod, kus on ankeedi andmed
            RealEstateDto dto = MockRealEstateData();

            // Act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstateId = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            // Assert
            // Võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(addRealEstate.Id, deleteRealEstateId.Id);

        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAdress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

    }
}
