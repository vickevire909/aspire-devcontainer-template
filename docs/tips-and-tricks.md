## Refactoring C# with Regex find and replace

| Action                                                              | Find regex                                                           | Replace with          |
| ------------------------------------------------------------------- | -------------------------------------------------------------------- | --------------------- |
| **Update all `namespace` and `using` statements after moving code** | `(?<=using \|namespace )My\.Old\.Namespace`                          | `My.New.Namespace`    |
| **Change project reference path**                                   | `[\/\\]Path\.To\.My\.Project[\/\\]Path\.To\.My\.Project(?=\.csproj)` | `New.Path.To.Project` |
