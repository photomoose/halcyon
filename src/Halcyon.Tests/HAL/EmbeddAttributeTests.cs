using Halcyon.HAL;
using Halcyon.Tests.HAL.Models;
using Newtonsoft.Json;
using Xunit;

namespace Halcyon.Tests.HAL
{
    public class EmbeddAttributeTests
    {
        [Fact]
        public void Embedded_Enumerable_Constructed_From_Attribute()
        {
            var model = new PersonModelWithAttributes();
            model.Pets.Add(new Pet { Id = 1, Name = "Fido" });
            var converter = new HALAttributeConverter();
            var halResponse = new HALResponse(model);
            halResponse = converter.Convert(halResponse);
            var serializer = new JsonSerializer();
            var jObject = halResponse.ToJObject(serializer);

            var embedded = jObject["_embedded"]["pets"][0];
            var embeddedSelfLink = embedded["_links"]["self"];

            Assert.Equal("Fido", embedded["Name"]);
            Assert.Equal("1", embedded["Id"]);
            Assert.Equal("pets/1", embeddedSelfLink["href"].ToString());
        }

        [Fact]
        public void Embedded_Single_Property_Constructed_From_Attribute()
        {
            var model = new PersonModelWithAttributes();
            var converter = new HALAttributeConverter();
            var halResponse = new HALResponse(model);
            halResponse = converter.Convert(halResponse);
            var serializer = new JsonSerializer();

            var jObject = halResponse.ToJObject(serializer);

            var embedded = jObject["_embedded"]["favouritePet"][0];
            Assert.Equal("Benji", embedded["Name"]);
            Assert.Equal("0", embedded["Id"]);
        }
    }
}