Использовал ASP .NET Core MVC и Entity Framework.  
Настройка:
* Нужно установить `LocalDb` из установщика `SQL Express`.
* В комадной строке выполнить `SqlLocalDB i`, посмотреть как называется экземпляр SQL сервера, выполнить `SqlLocalDB start [название]`. Либо создать новый с помощью `SqlLocalDB create [название]`, а потом его запустить.
* В проекте в `appsettings.json` нужно поменять название экземпляра SQL, если создавали свой.
* В проекте в `PowerShell` нужно выполнить `cd Store` и `dotnet ef database update`.
* Можно запускать проект.
