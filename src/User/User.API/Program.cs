using HostWebApi.Shared;
using User.API.Extensions;

var app = DefaultWebApplication.Create(args, builder =>
{
    builder.Services.InitUser(builder.Configuration);

});



await DefaultWebApplication.RunAsync(app);

