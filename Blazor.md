# Blazor

Blazor est un framework full-stack orienté front-end. Il utilise notament razor(.razor), qui regroupe du HTML/CSS et C# dans un même fichier de code.

## Sources

| Ressource | Lien |
|-----------|------|
| Tutos et vidéos de Microsoft | [Blazor Tutorial](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/intro) |
| Tutos de Microsoft | [Build Web Apps with Blazor](https://learn.microsoft.com/fr-fr/training/paths/build-web-apps-with-blazor/) |
| Vidéos de Microsoft | [Frontend Web Development with .NET for Beginners](https://learn.microsoft.com/fr-fr/shows/frontend-web-development-with-dotnet-for-beginners/what-is-blazor-frontend-web-development-with-dotnet-for-beginners) |

## Structure de projet 
### Côté client

| Dossier/Fichier | Description |
|-----------------|-------------|
| bin | Contient les fichiers binaires compilés |
| Components | Contient chaque composant de l'application |
| Layout | Contient la navbar et la sidebar |
| Pages | Contient des éléments pouvant être appelés dans n'importe quelle page |
| obj | Contient les fichiers objets compilés |
| Properties | Contient les propriétés du projet |
| wwwroot | Contient les fichiers statiques accessibles publiquement |
| appsettings.Development.json | Contient les paramètres de configuration pour l'environnement de développement |
| appsettings.json | Contient les paramètres de configuration globaux |
| BlazorApp.csproj | Fichier de projet pour l'application Blazor |
| BlazorApp.sln | Fichier de solution pour l'application Blazor |
| Program.cs | Point d'entrée principal de l'application |

### Côté serveur

| Dossier/Fichier | Description |
|-----------------|-------------|
| bin | Contient les fichiers binaires compilés |
| Controllers | Contient les contrôleurs API |
| Data | Contient les classes de contexte de la base de données et les migrations |
| Models | Contient les modèles de données |
| obj | Contient les fichiers objets compilés |
| Properties | Contient les propriétés du projet |
| wwwroot | Contient les fichiers statiques accessibles publiquement |
| appsettings.Development.json | Contient les paramètres de configuration pour l'environnement de développement |
| appsettings.json | Contient les paramètres de configuration globaux |
| Program.cs | Point d'entrée principal de l'application |
| Startup.cs | Contient la configuration des services et du pipeline de traitement des requêtes |

### Côté bibliothèque

| Dossier/Fichier | Description |
|-----------------|-------------|
| bin | Contient les fichiers binaires compilés |
| obj | Contient les fichiers objets compilés |
| Properties | Contient les propriétés du projet |
| wwwroot | Contient les fichiers statiques accessibles publiquement |
| appsettings.json | Contient les paramètres de configuration globaux |
| MyProjectName.csproj | Fichier de projet pour la bibliothèque de classes Razor |

## Code

Il utilise razor qui est un mix entre HTML/CSS et C#.

### HTML

Blazor permet d'utiliser toutes les balises de base du HTML. Il peut donc structurer et présenter le contenu des pages web en utilisant des éléments HTML standard comme `<p>` pour les paragraphes, `<h1>` pour les titres, et `<ul>` pour les listes non ordonnées, entre autres.

### C#

Pour intégrer des éléments C#, on utilise la directive `@code`.

```C#
@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

### La navigation

#### `@page`

La directive `@page` dans Blazor permet de définir les routes des pages. En ajoutant `@page` en haut d'un composant Razor, ce composant sera affiché quand l'utilisateur navigue vers l'URL spécifiée. Par exemple, `@page "/example"` signifie que le composant sera visible à l'URL `/example`. On peut aussi ajouter plusieurs directives `@page` pour lier un composant à plusieurs URL.

```C#
@page "/example"
@page "/sample"
```

#### `Navigation manager`

Le `Navigation manager` dans Blazor est un service qui permet de gérer la navigation et les URL dans une application Blazor. Il offre des méthodes pour naviguer vers différentes pages, obtenir l'URL actuelle, et réagir aux changements d'URL.

Exemple d'utilisation pour obtenir l'URL actuelle :

```C#
@page "/pizzas"
@inject NavigationManager NavManager

<h1>Buy a Pizza</h1>

<p>I want to order a: @PizzaName</p>

<a href=@HomePageURI>Home Page</a>

@code {
    [Parameter]
    public string PizzaName { get; set; }
    
    public string HomePageURI { get; set; }
    
    // Accéder à l'URL actuelle
    protected override void OnInitialized()
    {
        HomePageURI = NavManager.BaseUri;
    }
}
```

> [!] Notes
> Dans blazor utilisez `NavLink` plutot que `<a>`. C'est simplement une question de design car le lien est mis en surbrillance.
> En utilisant des éléments <a>, nous pouvons gérer manuellement la page qui est active en ajoutant la classe CSS active. Nous allons mettre à jour l’ensemble de la navigation pour utiliser un composant NavLink à la place.
>
> `NavLinkMatch.All` : quand vous utilisez cette valeur, le lien est mis en surbrillance comme lien actif seulement quand son href correspond à l’URL actuelle entière.
> `NavLinkMatch.Prefix` : quand vous utilisez cette valeur, le lien est mis en surbrillance comme lien actif quand son href correspond à la première partie de l’URL actuelle.
>
> ```C#
> <NavLink href=@HomePageURI Match="NavLinkMatch.All">Home Page</NavLink>
> ```

Pour accéder à la chaîne de requête on doit analyser l'URL complet. Pour cela, on utilise `QueryHelpers`.

```C#
@page "/exemple"
@using Microsoft.AspNetCore.WebUtilities // Utiliser ça pour acceder à la chaîne de requête
@inject NavigationManager NavManager

<h1>Buy a Pizza</h1>

<p>I want to order a: @PizzaName</p>

<p>I want to add this topping: @ToppingName</p>

@code {
    [Parameter]
    public string PizzaName { get; set; }
    
    private string ToppingName { get; set; }
    
    //Méthode pour accéder à la chaîne de requête
    protected override void OnInitialized()
    {
        var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("extratopping", out var extraTopping))
        {
            ToppingName = System.Convert.ToString(extraTopping);
        }
    }
}
```

Pour accéder à un autre composant on utilise `NavigationManager.NavigateTo()` :

```C#
@page "/pizzas/{pizzaname}"
@inject NavigationManager NavManager

<h1>Buy a Pizza</h1>

<p>I want to order a: @PizzaName</p>

<button class="btn" @onclick="NavigateToPaymentPage">
    Buy this pizza!
</button>

@code {
    [Parameter]
    public string PizzaName { get; set; }
    
    // Navigation vers une autre page
    private void NavigateToPaymentPage()
    {
        NavManager.NavigateTo("buypizza");
    }
}
```

> [!] Notes
> La chaîne que vous passez à la méthode NavigateTo() est l’URI absolu ou relatif où vous voulez envoyer l’utilisateur. Vérifiez que vous disposez d’un composant configuré à cette adresse. Pour le code ci-dessus, un composant avec la directive @page "/buypizza" va gérer cette route.

### `@rendermode`

La directive `@rendermode InteractiveServer` permet les interactions du client avec la page. En gros, la page est d'abord rendue sur le serveur et envoyée au client, mais les interactions comme les clics de bouton sont gérées en direct via SignalR. Ça rend l'expérience utilisateur plus fluide et réactive sans avoir besoin de recharger toute la page.

### Exemple de page :

```C#
@page "/example"
@rendermode InteractiveServer

<PageTitle>Example</PageTitle>

<h1>Exemple</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

L’application devrait se compiler, mais si vous créez une commande et que vous essayez de valider l’achat, vous obtenez une erreur d’exécution. L’erreur se produit en raison du fait que notre base de données SQLite pizza.db a été créée avant la prise en charge des commandes et des pizzas. Nous devons supprimer le fichier pour qu’une nouvelle base de données puisse être créée correctement.

La méthode OnInitialized s’exécute quand des utilisateurs demandent la page pour la première fois. Elle ne s’exécute pas s’ils demandent la même page avec un paramètre de routage différent. Par exemple, si vous prévoyez que les utilisateurs vont passez de http://www.contoso.com/favoritepizza/hawaiian à http://www.contoso.com/favoritepizza, définissez à la place la valeur par défaut dans la méthode OnParametersSet().

| Composant d’entrée | Rendu en tant que (HTML) |
|--------------------|--------------------------|
| InputCheckbox | `<input type="checkbox">` |
| InputDate<TValue> | `<input type="date">` |
| InputFile | `<input type="file">` |
| InputNumber<TValue> | `<input type="number">` |
| InputRadio<TValue> | `<input type="radio">` |
| InputRadioGroup<TValue> | Groupe de cases d’option enfants |
| InputSelect<TValue> | `<select>` |
| InputText | `<input>` |
| InputTextArea | `<textarea>` |

Chacun de ces éléments dispose d’attributs reconnus par Blazor, comme DisplayName ,qui est utilisé pour associer un élément d’entrée à une étiquette, et @ref, que vous pouvez utiliser pour enregistrer une référence à un champ dans une variable C#. Tous les attributs non Blazor non reconnus sont transmis sans modification au convertisseur HTML. Cela signifie que vous pouvez utiliser des attributs d’élément d’entrée HTML. Par exemple, vous pouvez ajouter les attributs min, max et step à un composant InputNumber pour qu’ils fonctionnent correctement dans le cadre de l’élément <input type="number"> rendu. Dans l’exemple précédent, vous pouvez spécifier le champ d’entrée TemperatureC comme suit :

```html
<EditForm Model=@currentForecast>
    <InputNumber @bind-Value=currentForecast.TemperatureC width="5" min="-100" step="5"></InputNumber>
</EditForm>
```

## Création de bibliothèque de classes 
Les composants des applications Web offrent aux développeurs la possibilité de réutiliser des parties d’une interface utilisateur dans l’ensemble de l’application. Grâce aux bibliothèques de classes Razor, les développeurs peuvent partager et réutiliser ces composants dans de nombreuses applications.

Pour créer le projet, utiliser cette commande :
- `dotnet new razorclasslib -o MyProjectName -f net8.0`

Voici ce qui est inclus dedans :
- Une feuille de style en cascade isolée nommée Component1.razor.css, stockée dans le même dossier que Component1.razor. Le fichier Component1.razor.css est inclus de manière conditionnelle dans une application Blazor qui référence Component1.
- Contenu statique, tel que des images et des fichiers JavaScript, disponible pour une application Blazor au moment de l'exécution et référencé dans Component1. Ce contenu est fourni dans un dossier wwwroot qui se comporte comme un dossier wwwroot dans une application ASP.NET Core ou Blazor.
- Code NET, qui exécute des fonctions se trouvant dans le fichier JavaScript inclus.

Voici le fichier de projet qui vient d'être créé :
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  
  <ItemGroup>
    <SupportedPlatform Include="browser" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="8.0.0" />
  </ItemGroup>

</Project>
```

On conseille de renommer Component1.razor et Component1.razor.css, par exemple par Modal.razor et Modal.razor.css cela permettra de les appelés plus facilement ultérieurement.
Les Components partagé seront donc cela, c'est donc eux qu'il faut modifier en fonction de ce dont on a besoin.

Exemple de composant :
```c#
@if (Show) {

 <div class="dialog-container">
  <div class="dialog">
   <div class="dialog-title">
    <h2>@Title</h2>
   </div>

   <div class="dialog-body">
    @ChildContent
   </div>

   <div class="dialog-buttons">
    <button class="btn btn-secondary mr-auto" @onclick="OnCancel">@CancelText</button>
    <button class="btn btn-success ml-auto" @onclick="OnConfirm">@ConfirmText</button>
   </div>

  </div>
 </div>

}

@code {

 [Parameter]
 public string Title { get; set; }

 [Parameter]
 public string CancelText { get; set; } = "Cancel";

 [Parameter]
 public string ConfirmText { get; set; } = "Ok";

 [Parameter]
 public RenderFragment ChildContent { get; set; }

 [Parameter]
 public bool Show { get; set; }


 [Parameter] public EventCallback OnCancel { get; set; }
 [Parameter] public EventCallback OnConfirm { get; set; }

}
```

> [!] Notes
> Vous pouvez définir le contenu interne du composant par le biais du paramètre `@ChildContent`.
> Vous pouvez contrôler l’état d’affichage de la boîte de dialogue avec le paramètre `Show`.

Dans le projet dans lequel on souhaite utiliser le composant partagé, si :
### il est dans le même dossier que la bibliothèque

On ajoute une référence à la bibliothèque : `dotnet add reference ../MyProjectName`

Pour faciliter la référence on ajoute au fichier *Components/_Import.razor*, l'entrée `@using MyProjectName`

En admettant que l'on a renommé les fichiers du composant de la bibliothèque en `Modal.`, on rajoute le composant au projet avec les balises `<Modal>` et `</Modal>` dans la page souhaité du projet. 
Exemple :
```html
<Modal Title="My first Modal dialog" Show="true">
 <p>
   This is my first modal dialog
 </p>
</Modal>
```

### la bibliothèque est empaqueté

**Dans la bibliothèque :** 
Dans le fichier du projet (*.csproj*), il faut rajouter un moyen d'identifer de package dans la balise `<PropertyGroup>`. 
Voici un exemple de cette balise :
```xml
<PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <PackageId>My.MyProjectName</PackageId>
    <Version>0.1.0</Version>
    <Authors>YOUR NAME</Authors>
    <Company>YOUR COMPANY NAME</Company>
</PropertyGroup>
```
> [!] Infos
> | Champ |	Description | Valeur par défaut |
> |-------|-------------|-------------------|
> | PackageId |	identificateur de package, unique dans l’intégralité du référentiel NuGet. | `AssemblyName` de la bibliothèque |
> | Version	| Numéro de version spécifique sous la forme `Majeure.Mineure.Correctif[-Suffixe]`, où `-Suffixe` définit éventuellement des versions préliminaires. | `1.0.0` |
> | Auteurs	| Auteurs du package. | `AssemblyName` |
> | Company	| Nom de la société responsable de la création et de la publication du package.	| `AssemblyName` |

> [!] Notes
> À l’étape de build, on peut aussi configurer le projet de façon à générer un package de NuGet.
> `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>`
> 
> De nombreuses propriétés facultatives du projet sont configurables :
> - Description appropriée pour l’affichage dans le référentiel NuGet
> - Mention de droits d’auteur
> - Informations de licence
> - Icônes
> - URL du projet

Ensuite, on empaquete le projet dans le même fichier que le fichier de projet (*.csproj*): `dotnet pack`
> [!] Attention
> Cette commande enregistre normalement un fichier nommé *My.MyProjectName.0.1.0.nupkg* dans le dossier *bin/Debug*.
> Il arrive que le fichier *My.MyProjectName.0.1.0.nupkg* s'enregistre dans le dossier *bin/Release*, il faudra donc le déplacer.

**Dans le projet :**
Dans le même dossier que le fichier de projet (*.csproj*), on execute la commande :
`dotnet add package My.MyProjectName -s ..\MyProjectName\bin\Debug`

En admettant que l'on a renommé les fichiers du composant de la bibliothèque en `Modal.`, on rajoute le composant au projet avec les balises `<Modal>` et `</Modal>` dans la page souhaité du projet. 
Exemple :
```html
<Modal Title="My first Modal dialog" Show="true">
 <p>
   This is my first modal dialog
 </p>
</Modal>
```

