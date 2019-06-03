##Better Medkit

Improves the Medkit to add a reduction per stack to the delay after which you receive the healing.
The reduction is capped to half its delay to avoid turning dots into regens.

It works this way :

	- Delay = 0.55 + 0.55 * 0.95^(MedkitStacks-1)
	
You need 35 stacks to reach a delay inferior to 0.56s for exemple.
	
##Installation

Drop `BetterMedkit.dll` into `\BepInEx\plugins\`	

## Changelog
	- v1.0.0
		- Release
	- v1.0.1
		- Balance patch to add a cap to the delay reduction