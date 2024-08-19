using HostWebApi.Shared;

using User.API.Extensions;

var app = DefaultWebApplication.Create(args, builder => builder.Services.InitUser());



await DefaultWebApplication.RunAsync(app);

