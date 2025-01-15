# ML-Agents Configuration

This tool generates and manages configurations for ML-Agents training scenarios. It provides models for entities like `Behavior`, `NetworkSettings`, etc and composes a `Configuration` with them. 
To preserve the settings components that contain settings for behaviors and configuration can be added to scene. Settings here work explicitly - file is created without defaults omitted.

## Usage

### Installation

The package can be installed through the UPM by the link `https://github.com/Alexiush/ML-Agents-Configuration.git`. 
It also requires [SerializeReferenceExtensions](https://github.com/mackysoft/Unity-SerializeReferenceExtensions) and [VYaml](https://github.com/hadashiA/VYaml) packages.

### Settings

Default path for generated config can be changed in the project settings under `ML Agents Config` tab.

### Config generation

To generate config open `Window/ML Agents Configuration/Configuration Generator` where all the found behaviors will be present as well as environment-level settings. 
Notice, that settings are not preserved unless there are `BehaviorConfig` and `EnvironmentConfig` components in the scene.

## Samples

**Roller** - RollerBall agent environment ready for generation.

## TODO:
* Behavioral Cloning and Self-Play support
* Optimization

## Credits
[SerializeReferenceExtensions](https://github.com/mackysoft/Unity-SerializeReferenceExtensions)

[VYaml](https://github.com/hadashiA/VYaml)
