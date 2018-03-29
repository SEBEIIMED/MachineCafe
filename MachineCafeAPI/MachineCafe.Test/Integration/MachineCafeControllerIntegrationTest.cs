using System.Net;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using MachineCafe.Service;
using MachineCafe.Core;
using MachineCafe.Core.Data;

namespace MachineCafe.Test
{
    public class MachineCafeControllerIntegrationTest : StartupTestIntegration<Startup>
    {

        [Fact]
        public async Task DemanderBoissonReturnOk ()
        {
            MachineCafeChoix choix = new MachineCafeChoix() { BoissonType = BoissonTypeEnum.Cafe, CodeBadge = "badgetest", CustomMug = true, SucreQuantite = 2 };
            var content = new StringContent(JsonConvert.SerializeObject(choix), Encoding.UTF8, "application/json");

            // Act
            var response = await this.Client.PostAsync("/api/MachineCafe/DemanderBoisson", content);
            // Assert
            Assert.Equal(expected: HttpStatusCode.OK, actual: response.StatusCode);
        }

        [Fact]
        public async Task ChoixBoissonListReturnOk()
        {
            var content = new StringContent("badgetest", Encoding.UTF8, "application/json");
            var response = await this.Client.PostAsync("/api/MachineCafe/ChoixBoissonList", content);
            // Assert
            Assert.Equal(expected: HttpStatusCode.OK, actual: response.StatusCode);
        }
    }
}
