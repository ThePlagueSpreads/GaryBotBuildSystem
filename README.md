# Gary Bot Build System
A system designed for fully automatic building and deployment of mods that use a C# solution and Asset Bundles. Builds assemblies and assets. Designed to run on a Windows server.

Features Discord and Google Drive integration.

> [!NOTE]
> Setup can be time-consuming due to the number of dependencies and required services. For instance, Blender must be installed on your machine to compile .blend files in Asset Bundles.

## Setup Instructions
1. Clone the repository.
2. Open the solution file with Visual Studio v18+ (.NET 10 is required).
3. Build the solution.
4. Create an `appsettings.json` file in the same folder as the .exe, and configure it as needed. Some parts will make sense later. See the `SAMPLE-appsettings.json` file from the repo root for a starting point.
6. Using the Google Cloud Console, create a new project, set up OAuth 2.0, and obtain the `credentials.json` file. Place that file in the same folder as the .exe file. Also enable the Google Drive API feature.
7. Create a folder in the Google Drive account associated with the Google Cloud API and use the ID from the URL as the `FolderId` in the `appsettings.json` file. They are around 33 characters and can include some non-alphanumeric symbols.
8. Create your Discord bot and feed the token into the configuration.
9. Run the program and fix any issues you run into. Your browser may have to open to authorize Git and OAuth 2.0. This is normal!

## Limitations
- Does not support other configurations for mods (e.g. only one repo with no assets).
- Platforms can be configured, but the system does not support building for multiple platforms remotely.
- DOES allow zipping and uploading multiple output files (e.g. in the case of a second or even third mod project under the same solution).
