using MachineCafe.Core;
using MachineCafe.Core.Data;
using MachineCafe.Core.Repository;
using MachineCafe.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MachineCafe.Test
{
    public class MachineCafeControllerTest
    {
        [Fact]
        public void DemanderBoisson()
        {
            var mockMachineCafeChoixRepository = GetMachineCafeChoixRepository();
            var controller = new MachineCafeController(mockMachineCafeChoixRepository);
            MachineCafeChoix choix = new MachineCafeChoix() { CodeBadge = "badgetest", BoissonType = BoissonTypeEnum.Cafe, SucreQuantite = 2, CustomMug = true };
            IActionResult result = controller.DemanderBoisson(choix) ;

            var resultObj = result as OkObjectResult;
            Assert.IsType<MachineCafeChoix>(resultObj.Value);
            Assert.Equal(resultObj.Value, choix);
        }
        [Fact]
        public void ChoixBoissonList()
        {
            var mockMachineCafeChoixRepository = GetMachineCafeChoixRepository();
            var controller = new MachineCafeController(mockMachineCafeChoixRepository);
            MachineCafeChoix choix = new MachineCafeChoix();
            string badgecode = "badgetest";
            IActionResult result = controller.ChoixBoissonList(badgecode);

            var resultObj = result as OkObjectResult;
            Assert.IsType<List<MachineCafeChoix>>(resultObj.Value);
            List<MachineCafeChoix> choixList = resultObj.Value as List<MachineCafeChoix>;
            Assert.NotEqual(choixList.Count, 0);
        }
        private IMachineCafeChoixRepository GetMachineCafeChoixRepository()
        {
            DbContextOptions<DataContext> options;
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer(@"Server=localhost;Database=MachineCafe;Trusted_Connection=False;MultipleActiveResultSets=true;User Id=sa;Password=adminsebei;");
            options = builder.Options;
            DataContext dataContext = new DataContext(options);
            
            return new MachineCafeChoixRepository(dataContext);
        }
    }





}



