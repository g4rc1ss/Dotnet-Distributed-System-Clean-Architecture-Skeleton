using System;
using Application.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Microsoft.AspNetCore.DataProtection;
using Moq;
using TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingTrueData;

namespace TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast
{
    public class MoqDataProtection
    {
        public Mock<IDataProtectionProvider> Mock { get; set; }

        public MoqDataProtection()
        {
            Mock = new Mock<IDataProtectionProvider>();
            Initialize();
        }

        private void Initialize()
        {
            Mock.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(new MoqDataProtector().Mock.Object);
        }
    }

    public class MoqDataProtector
    {
        public Mock<IDataProtector> Mock { get; set; }

        public MoqDataProtector()
        {
            Mock = new Mock<IDataProtector>();
            Initialize();
        }

        private void Initialize()
        {
            Mock.Setup(x => x.Protect(It.IsAny<byte[]>())).Returns((byte[] value) => value);

            Mock.Setup(x => x.Unprotect(It.IsAny<byte[]>())).Returns((byte[] value) => value);
        }
    }
}

