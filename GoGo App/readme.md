# GoGo

GoGo is a mobile application developed using [Xamarin.Android](https://docs.microsoft.com/en-us/xamarin/android/) which aims to solve a need to improve knowledge around Costa Rica’s public transportation by working as a central repository of knowledge about bus routes in Costa Rica. This is achieved by allowing a special kind of users register, update o delete bus routes and their bus stops.

![image-20200722003720645](C:\Users\JeremyZelayaRodrigue\AppData\Roaming\Typora\typora-user-images\image-20200722003720645.png)

## Context

This is a college course’s project on Agile Software Development, where the project consisted on developing a application which had gone though the traditional phases of software development. This meant the scope was reduced from it’s original inception both due to a lack of time, money and tecnilical resources. Originally the front end was planned to be developed in React Native with a Node.js backend using MongoDB. This was changed due to time constrains, opting for Xamarin.Android which was previously taught in the institution. Here are a few concepts or technologies which where used to develop this application:

- Rest API using ASP.NET Core.
- Android Fragments.
- Google Maps API and Google Directions API

If you where to find this project interesting do feel free to initiate a fork and work on you own implementation of this idea.

# Requisites

You will need a mySQL database to run this aplication, you may configure a connection string in the `Startup.cs` or though a environment variable

```csharp
public void ConfigureServices(IServiceCollection services)

    {

      string awsConnection = Environment.GetEnvironmentVariable("AWSConnectionString");

      *// Register the DB*

      *// For local development please use:*

      *// services.AddTransient<AppDb>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));*

      services.AddTransient<AppDb>(_ => new AppDb(awsConnection));

      services.AddControllers();

    }
```

You will also need to run this app though `ÌIS`, instead of `ÌIS-Express` since it would not allow you to issue request externally.

# Special Thanks

Here are a few Stack OverFlow threads which allow us to finish the project which we felt should be shared:

- https://stackoverflow.com/questions/62866574/the-name-suportmapfragment-does-not-exist-in-the-current-context/62868229?noredirect=1#comment111178118_62868229
- https://stackoverflow.com/questions/62737714/android-views-inflateexception-binary-xml-file-line-2-binary-xml-file-line
- https://stackoverflow.com/questions/62887536/accesing-local-server-using-resharp
- https://forums.xamarin.com/discussion/183431/android-views-inflateexception-binary-xml-file-line-2-binary-xml-file-line-2#latest