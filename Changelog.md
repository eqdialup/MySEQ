# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).
 
 
## [2.5.0] - 2021-02-19

### Added (Client)
- Added 5 more adhoc lookups to the map.
- Added ability to filter as well as lookup via the adhoc lookups. L is lookup, F is filter. Boxes are interpreted left to right. So if you lookup all goblins in slot 1, then filter all scouts in slot 2, it will show only the goblins who are not scouts.
- Added level searching to adhoc lookups. L:xx is the lookup/filter
- Added setting to control display of lookedup mobs. Bottom of the view menu. If Lookup text is checked, then all mob targeted by a lookup will display something next to the dot. If Lookup Name/Number isn't checked, then it will show the mob's name. If Lookup Name/Number is checked, then it will show 1 through 6 depending on which slot in the lookups the mob matched on.
- Added filters for primary and offhand. These are zone filters only, and go in the zone filter file. Mobs that are found with the item in the respective slot, will be treated as hunt mobs. That moves them to the top of the spawn list, and marks them with a flashing circle on the map. Filters need to look like this:
	<section name="Primary">
		<oldfilter><regex>NamearkBook</regex></oldfilter>
	</section>
	<section name="Offhand">
		<oldfilter><regex>Name:Shiverback</regex></oldfilter>
	</section>
- Added aways on top toggle. Bottom of the view menu.

### Changed (Client)
- Added special handling of a_tainted_egg in the Vish encounter. It now acts like a normal mob and can be added to lookups/hunt mobs etc.

### Changed (Server)
- Changed server args to be compatible with VS2019
- Changed the debug buffer from 2048 to 8192
