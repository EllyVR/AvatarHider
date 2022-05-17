# Sleepers VRC Mods<!-- omit in toc -->

These are mods that weren't being maintained anymore, gathered together.
The hope with this repository is that as a community we could still maintain them.
Scroll down to [the table of contents](#table-of-contents) for the list of mods.

This repository was forked from Loukylor's mods after he stopped maintaining them.
The intention with the name change is to make it clear that this project hopes to have multiple people maintaining it instead of a single individual.
It's also a tribute to all the modders who've helped make and maintain these mods but aren't maintaining them anymore - even modders need to sleep eventually.

Please go see [the credits](#credits) for the full list of people who've helped make this all possible!
Or maybe even [get your name added](./CONTRIBUTING.md) to that list?

## Disclaimer<!-- omit in toc -->

VRChat does **not** condone the use of mods.
You **will** be punished if found to be modifying the client.

That being said, there is no anticheat of any sort in the client as of writing this.
There is an API limiter & Photon checks.
The mods listed in this repository avoid triggering those.
The only real way to get punished is to piss of the aforementioned checks, or be reported by a user with evidence of you using a mod.

**These mods are provided as-is without any warranty and we will not be held responsible for anything that using mods may cause**.

## Installation<!-- omit in toc -->

1. Follow the instructions on the [MelonLoader wiki](https://melonwiki.xyz/#/) on installing MelonLoader (MelonLoader is the mod loader which will allow the mods to run).
2. Download the mod(s) you would like to install.
3. Drag'n'drop the downloaded mod(s) into the `Mods` folder in the `VRChat` folder (if the folder isn't there, run the game once).

More detailed instructions and more mods can be found in the [VRChat Modding Group Discord](https://discord.gg/rCqKSvR).

## AvatarHider


Automatically hides avatars based on the distance away from you.
There's no real reason to render avatars that you don't even pay attention to, right?

For the best experience, it is recommended to run this mod with UIExpansionKit.
"Hide Distance" is customizable and can be changed in meters (default is 7 meters).
Friend's avatars are ignored by default, but can be hidden by distance too if needed.
"Exclude Shown Avatars" will ignore to hide a persons avatar if you are showing the avatar.
"Disable Spawn Sounds" will only prevent a spawn sound from replaying when the avatar becomes visable again.

Tip:
If a friend is using an unoptimized avatar and you would like AvatarHider to hide it, disable "Ignore Friends" and enable "Exclude Shown Avatars".
Then show your friends avatars that you would like to be ignored by AvatarHider.
And set the friend with the unoptimized avatar to the "Use Safety Settings" in the QuickMenu.


## VRChatUtilityKit

Migrated methods from VRChatUtilityKit to this mod. 

### For Developers

If you wish to use this mod, please respect the license, LGPL v3. You can read more below.
The source is documented, and the XML file is included in the release.
To utilise the XML file, just put it in the same directory as the copy of the utility kit you are referencing.

### Licensing

This library is licensed under LGPL v3.
This means that you are allowed to reference the library in your code as long as you disclose source and have a license and copyright notice you will be fine.
In the case that you would like to modify or include the library in your mod, you must use the same license as well as state any changes.

We will seek to punish license violations.

There is some code that was originally licensed under GPL v3, however express permission has been granted to license said code under LGPL v3.

## Credits

Here is a long list of the awesome people who have helped make these mods a reality:

- [Loukylor](https://github.com/loukylor) for being the original author of a tons of things here, as this repo was forked from his.
- [knah](https://github.com/knah) for [Join Notifier's](https://github.com/knah/VRCMods) join/leave and asynchronous utilities.
- [DubyaDude](https://github.com/DubyaDude) for [RubyButtonAPI](https://github.com/DubyaDude/RubyButtonAPI) as reference for the button API.
- [Psychloor](https://github.com/Psychloor) for the risky functions check.
- [Brycey92](https://github.com/Brycey92) for AvatarHider contributions.
- [dave-kun](https://github.com/dave-kun) for RememberMe & AvatarHider contributions.
- [ImTiara](https://github.com/ImTiara) for the original version of AvatarHider
- [KortyBoi](https://github.com/KortyBoi) for PlayerList's layout & help with getting some of the information.
- Frostbyte for contributing to PlayerList's optimizations
- [HerpDerpinstine](https://github.com/HerpDerpinstine) for being the original author of RememberMe
- [neitri](https://github.com/netri) for the ["Distance Face Outline" shader](https://github.com/netri/Neitri-Unity-Shaders) that was modified to create the TriggerESP shader.
- Potato for PreviewScroller contributions.
- [Sarayalth](https://github.com/Sarayalth) for a lot of contributions to many of the mods here.
- [ljoonal](https://github.com/ljoonal) for various modifications, cleanup, and management.
- [Nirv](https://github.com/Nirv-git) for taking up maintaining InstanceHistory in [their repository](https://github.com/Nirv-git/VRC-Mods) and more.
- [Adnezz](https://github.com/Adnezz) for taking up maintaining PlayerHistory in [their repository](https://github.com/Adnezz/PlayerList)
- [SDraw](https://github.com/SDraw) for VRCUK fix help

In a way, it's what the "Sleepers" means as the author name.
A reference to everyone who has contributed to these mods in one way or another.

The list would become too long to list everything, but it should hopefully cover at least the biggest contributions.
If you notice that the list is missing someone or something, do create a PR!

[VRCUKRequiredBadge]: https://img.shields.io/badge/VRCUK-Required-important?style=flat
[VRCUKLink]: https://github.com/SleepyVRC/Mods/releases
[UIXRequiredBadge]: https://img.shields.io/badge/UIX-Required-important?style=flat
[UIXOptionalBadge]: https://img.shields.io/badge/UIX-Optional-informational?style=flat
[UIXLink]: https://github.com/knah/VRCMods/
