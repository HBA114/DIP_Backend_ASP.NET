## Changelog 22.09.2023

- SkiaSharp Nuget Package Updated.
    - Version 2.88.3 to 2.88.6

- Removed unnecessary commented code from Program.cs.

- Updated VS Code settings configuration for workspace.

- Updated ImageController.cs.
    - Removed old style switch-case returns and added more effective and less code styled switch return.

- Updated all dtos as records
    - Fixed Misspelled dto field names. May Break HTTP Requests On Frontend Project.
        - FilterDto: standartDeviation => standardDeviation
        - PreProcessing1Dto: tresholdValue => thresholdValue

- Removed unnecessary empty constructors. (There is a default empty constructor in class with no constructor)
