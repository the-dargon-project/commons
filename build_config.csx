var projects = Projects("commons");

Export.Solution(
   Name: "Dargon Commons",
   Commands: new ICommand[] {
      Build.Projects(projects),
      Test.Projects(projects)
   }
);