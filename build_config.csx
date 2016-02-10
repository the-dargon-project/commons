var projects = Projects("commons");
var dependencies = Projects("ryu");

Export.Solution(
   Name: "Dargon Commons",
   Commands: new ICommand[] {
      Build.Projects(projects, dependencies),
      Test.Projects(projects)
   }
);
