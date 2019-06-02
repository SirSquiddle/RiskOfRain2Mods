##Better Medkit

Improves the Medkit to add a reduction per stack to the delay after which you receive the healing.

It works this way :

	- Delay = 1.1 * 0.95^(MedkitStacks-1)
	
You need 17 stacks to reach a delay inferior to 0.5s for exemple.
	
##Installation

Drop `BetterMedkit.dll` into `\BepInEx\plugins\`	

## Changelog
	- v1.0.0
		- Release