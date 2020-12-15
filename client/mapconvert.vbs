' VBScript to convert SOE map files to SEQ map file.

' Run this file from the folder containing the maps to be converted.

' This will overwrite all the *.map files existing in that folder.



strComputer = "."

Set objWMIService = GetObject("Winmgmts:\\" & strComputer & "\root\cimv2")

Set colItems = objWMIService.ExecQuery("Select * From Win32_DesktopMonitor")

Dim intVertical, intHorizontal

Set objExplorer = CreateObject("InternetExplorer.Application")

objExplorer.Navigate "about:blank"

objExplorer.ToolBar = 0

objExplorer.StatusBar = 0

objExplorer.Width = 400

objExplorer.Height = 200

objExplorer.Visible = 1

objExplorer.Document.Title = "Map Conversion in progress ..."

objExplorer.Document.Body.InnerHTML = "Starting Map Conversion."

WScript.Sleep 2000

Call Map_Convert

objExplorer.Document.Body.InnerHTML = "Map Conversion Complete."

Wscript.Sleep 2000

objExplorer.Quit

WScript.quit



' Combines layers and converts black lines to Grey

Sub Map_Convert()



	Dim r, g, b

	Dim x

	Dim sPath

	Dim longname

	Dim tocolor

    Dim mLines



	' Defaults used for Black Lines and Labels.  Set to 0 to have ignored.

	BlackLineColor = 43

	BlackLabelColor = 64

	' Color of labels that start with To....

	tocolor = 13



	' Set these to something other than 0 to use for all, otherwise they are ignored

	AllLineColor = 0

	AllLabelColor = 0



	' Set to 1 for connecting map lines - Will also remove extra sequential line points

	ConnectDots = 1

	Dim szColor(64)

	szColor(1) = "Black"

	szColor(2) = "DarkRed"

	szColor(3) = "Firebrick"

	szColor(4) = "Red"

	szColor(5) = "DarkGreen"

	szColor(6) = "Orange"

	szColor(7) = "DarkOrange"

	szColor(8) = "DarkOrange"

	szColor(9) = "Green"

	szColor(10) = "Chartreuse"

	szColor(11) = "Gold"

	szColor(12) = "Gold"

	szColor(13) = "LimeGreen"

	szColor(14) = "Chartreuse"

	szColor(15) = "Goldenrod"

	szColor(16) = "Yellow"

	szColor(17) = "DarkBlue"

	szColor(18) = "Magenta"

	szColor(19) = "DeepPink"

	szColor(20) = "DeepPink"

	szColor(21) = "DarkCyan"

	szColor(22) = "Grey"

	szColor(23) = "IndianRed"

	szColor(24) = "LightCoral"

	szColor(25) = "SpringGreen"

	szColor(26) = "LightGreen"

	szColor(27) = "DarkKhaki"

	szColor(28) = "Khaki"

	szColor(29) = "SpringGreen"

	szColor(30) = "PaleGreen"

	szColor(31) = "DarkOliveGreen"

	szColor(32) = "Khaki"

	szColor(33) = "MediumBlue"

	szColor(34) = "DarkViolet"

	szColor(35) = "Magenta"

	szColor(36) = "Maroon"

	szColor(37) = "RoyalBlue"

	szColor(38) = "SlateBlue"

	szColor(39) = "Orchid"

	szColor(40) = "HotPink"

	szColor(41) = "Turquoise"

	szColor(42) = "SkyBlue"

	szColor(43) = "LightGrey"

	szColor(44) = "LightPink"

	szColor(45) = "Cyan"

	szColor(46) = "Aquamarine"

	szColor(47) = "DarkSeaGreen"

	szColor(48) = "Beige"

	szColor(49) = "Blue"

	szColor(50) = "Purple"

	szColor(51) = "Purple"

	szColor(52) = "Magenta"

	szColor(53) = "DodgerBlue"

	szColor(54) = "SlateBlue"

	szColor(55) = "MediumPurple"

	szColor(56) = "Orchid"

	szColor(57) = "DeepSkyBlue"

	szColor(58) = "LightBlue"

	szColor(59) = "Plum"

	szColor(60) = "Cyan"

	szColor(61) = "CadetBlue"

	szColor(62) = "PaleTurquoise"

	szColor(63) = "LightCyan"

	szColor(64) = "White"

	

	set longname = CreateObject("Scripting.Dictionary")

	longname.CompareMode = vbTextCompare

	

	longname.Add "qeynos", "South Qeynos" 

	longname.Add "qeynos2", "North Qeynos" 

	longname.Add "qrg", "Surefall Glade" 

	longname.Add "qeytoqrg", "Qeynos Hills" 

	longname.Add "highpass", "Highpass Hold" 

	longname.Add "highkeep", "HighKeep" 

	longname.Add "freportn", "North Freeport" 

	longname.Add "freportw", "West Freeport" 

	longname.Add "freporte", "East Freeport" 

	longname.Add "runnyeye", "Clan RunnyEye" 

	longname.Add "qey2hh1", "West Karana" 

	longname.Add "northkarana", "North Karana" 

	longname.Add "southkarana", "South Karana" 

	longname.Add "eastkarana", "East Karana" 

	longname.Add "beholder", "Gorge of King Xorbb" 

	longname.Add "blackburrow", "BlackBurrow" 

	longname.Add "paw", "Infected Paw" 

	longname.Add "rivervale", "Rivervale" 

	longname.Add "kithicor", "Kithicor Forest" 

	longname.Add "commons", "West Commonlands" 

	longname.Add "ecommons", "East Commonlands" 

	longname.Add "erudnint", "Erudin Palace" 

	longname.Add "erudnext", "Erudin" 

	longname.Add "nektulos", "Nektulos Forest" 

	longname.Add "cshome", "Sunset Home" 

	longname.Add "lavastorm", "Lavastorm Mountains" 

	longname.Add "nektropos", "Nektropos" 

	longname.Add "halas", "Halas" 

	longname.Add "everfrost", "Everfrost Peaks" 

	longname.Add "soldunga", "Solusek's Eye" 

	longname.Add "soldungb", "Nagafen's Lair" 

	longname.Add "misty", "Misty Thicket" 

	longname.Add "nro", "North Ro" 

	longname.Add "sro", "South Ro" 

	longname.Add "befallen", "Befallen" 

	longname.Add "oasis", "Oasis of Marr" 

	longname.Add "tox", "Toxxulia Forest" 

	longname.Add "hole", "The Ruins of Old Paineel" 

	longname.Add "neriaka", "Neriak Foreign Quarter" 

	longname.Add "neriakb", "Neriak Commons" 

	longname.Add "neriakc", "Neriak Third Gate" 

	longname.Add "neriakd", "Neriak Palace" 

	longname.Add "najena", "Najena" 

	longname.Add "qcat", "Qeynos Catacombs" 

	longname.Add "innothule", "Innothule Swamp" 

	longname.Add "feerrott", "The Feerrott" 

	longname.Add "cazicthule", "Cazic-Thule" 

	longname.Add "oggok", "Oggok" 

	longname.Add "rathemtn", "Mountains of Rathe" 

	longname.Add "lakerathe", "Lake Rathetear" 

	longname.Add "grobb", "Gukta" 

	longname.Add "aviak", "Aviak Village" 

	longname.Add "gfaydark", "Greater Faydark" 

	longname.Add "akanon", "Ak'Anon" 

	longname.Add "steamfont", "Steamfont Mountains" 

	longname.Add "lfaydark", "Lesser Faydark" 

	longname.Add "crushbone", "Clan Crushbone" 

	longname.Add "mistmoore", "Castle Mistmoore" 

	longname.Add "kaladima", "Kaladim" 

	longname.Add "felwithea", "Felwithe" 

	longname.Add "felwitheb", "Felwithe" 

	longname.Add "unrest", "Estate of Unrest" 

	longname.Add "kedge", "Kedge Keep" 

	longname.Add "guktop", "Upper Guk" 

	longname.Add "gukbottom", "Lower Guk" 

	longname.Add "kaladimb", "Kaladim" 

	longname.Add "butcher", "Butcherblock Mountains" 

	longname.Add "oot", "Ocean of Tears" 

	longname.Add "cauldron", "Dagnor's Cauldron" 

	longname.Add "airplane", "Plane of Sky" 

	longname.Add "fearplane", "Plane of Fear" 

	longname.Add "permafrost", "Permafrost Keep" 

	longname.Add "kerraridge", "Kerra Isle" 

	longname.Add "paineel", "Paineel" 

	longname.Add "hateplane", "The Plane of Hate" 

	longname.Add "arena", "The Arena" 

	longname.Add "fieldofbone", "The Field of Bone" 

	longname.Add "warslikswood", "Warsliks Wood" 

	longname.Add "soltemple", "Temple of Solusek Ro" 

	longname.Add "droga", "Temple of Droga" 

	longname.Add "cabwest", "West Cabilis" 

	longname.Add "swampofnohope", "Swamp of No Hope" 

	longname.Add "firiona", "Firiona Vie" 

	longname.Add "lakeofillomen", "Lake of Ill Omen" 

	longname.Add "dreadlands", "Dreadlands" 

	longname.Add "burningwood", "Burning Woods" 

	longname.Add "kaesora", "Kaesora" 

	longname.Add "sebilis", "Old Sebilis" 

	longname.Add "citymist", "City of Mist" 

	longname.Add "skyfire", "Skyfire Mountains" 

	longname.Add "frontiermtns", "Frontier Mountains" 

	longname.Add "overthere", "The Overthere" 

	longname.Add "emeraldjungle", "The Emerald Jungle" 

	longname.Add "trakanon", "Trakanon's Teeth" 

	longname.Add "timorous", "Timorous Deep" 

	longname.Add "kurn", "Kurn's Tower" 

	longname.Add "erudsxing", "Erud's Crossing" 

	longname.Add "stonebrunt", "Stonebrunt Mountains" 

	longname.Add "warrens", "The Warrens" 

	longname.Add "karnor", "Karnor's Castle" 

	longname.Add "chardok", "Chardok" 

	longname.Add "dalnir", "Dalnir" 

	longname.Add "charasis", "Howling Stones" 

	longname.Add "cabeast", "East Cabilis" 

	longname.Add "nurga", "Mines of Nurga" 

	longname.Add "veeshan", "Veeshan's Peak" 

	longname.Add "veksar", "Veksar" 

	longname.Add "iceclad", "Iceclad Ocean" 

	longname.Add "frozenshadow", "Tower of Frozen Shadow" 

	longname.Add "velketor", "Velketor's Labyrinth" 

	longname.Add "kael", "Kael Drakkal" 

	longname.Add "skyshrine", "Skyshrine" 

	longname.Add "thurgadina", "Thurgadin" 

	longname.Add "eastwastes", "Eastern Wastes" 

	longname.Add "cobaltscar", "Cobalt Scar" 

	longname.Add "greatdivide", "Great Divide" 

	longname.Add "wakening", "The Wakening Land" 

	longname.Add "westwastes", "Western Wastes" 

	longname.Add "crystal", "Crystal Caverns" 

	longname.Add "necropolis", "Dragon Necropolis" 

	longname.Add "templeveeshan", "Temple of Veeshan" 

	longname.Add "sirens", "Siren's Grotto" 

	longname.Add "mischiefplane", "Plane of Mischief" 

	longname.Add "growthplane", "Plane of Growth" 

	longname.Add "sleeper", "Sleeper's Tomb" 

	longname.Add "thurgadinb", "Icewell Keep" 

	longname.Add "erudsxing2", "Marauder's Mire" 

	longname.Add "shadowhaven", "Shadow Haven" 

	longname.Add "bazaar", "The Bazaar" 

	longname.Add "nexus", "The Nexus" 

	longname.Add "echo", "Echo Caverns" 

	longname.Add "acrylia", "Acrylia Caverns" 

	longname.Add "sharvahl", "Shar Vahl" 

	longname.Add "paludal", "Paludal Caverns" 

	longname.Add "fungusgrove", "Fungus Grove" 

	longname.Add "vexthal", "Vex Thal" 

	longname.Add "sseru", "Sanctus Seru" 

	longname.Add "katta", "Katta Castellum" 

	longname.Add "netherbian", "Netherbian Lair" 

	longname.Add "ssratemple", "Ssraeshza Temple" 

	longname.Add "griegsend", "Grieg's End" 

	longname.Add "thedeep", "The Deep" 

	longname.Add "shadeweaver", "Shadeweaver's Thicket" 

	longname.Add "hollowshade", "Hollowshade Moor" 

	longname.Add "grimling", "Grimling Forest" 

	longname.Add "mseru", "Marus Seru" 

	longname.Add "letalis", "Mons Letalis" 

	longname.Add "twilight", "The Twilight Sea" 

	longname.Add "thegrey", "The Grey" 

	longname.Add "tenebrous", "The Tenebrous Mountains" 

	longname.Add "maiden", "The Maiden's Eye" 

	longname.Add "dawnshroud", "Dawnshroud Peaks" 

	longname.Add "scarlet", "The Scarlet Desert" 

	longname.Add "umbral", "The Umbral Plains" 

	longname.Add "akheva", "Akheva Ruins" 

	longname.Add "arena2", "The Arena" 

	longname.Add "jaggedpine", "The Jaggedpine Forest" 

	longname.Add "nedaria", "Nedaria's Landing" 

	longname.Add "tutorial", "Tutorial Zone" 

	longname.Add "load", "Loading" 

	longname.Add "load2", "Loading" 

	longname.Add "hateplaneb", "The Plane of Hate" 

	longname.Add "shadowrest", "Shadowrest" 

	longname.Add "tutoriala", "The Mines of Gloomingdeep" 

	longname.Add "tutorialb", "The Mines of Gloomingdeep" 

	longname.Add "clz", "Loading" 

	longname.Add "codecay", "Ruins of Lxanvom" 

	longname.Add "pojustice", "Plane of Justice" 

	longname.Add "poknowledge", "Plane of Knowledge" 

	longname.Add "potranquility", "Plane of Tranquility" 

	longname.Add "ponightmare", "Plane of Nightmare" 

	longname.Add "podisease", "Plane of Disease" 

	longname.Add "poinnovation", "Plane of Innovation" 

	longname.Add "potorment", "Plane of Torment" 

	longname.Add "povalor", "Plane of Valor" 

	longname.Add "bothunder", "Torden - The Bastion of Thunder" 

	longname.Add "postorms", "Plane of Storms" 

	longname.Add "hohonora", "Halls of Honor" 

	longname.Add "solrotower", "Solusek Ro's Tower" 

	longname.Add "powar", "Plane of War" 

	longname.Add "potactics", "Drunder - Fortress of Zek" 

	longname.Add "poair", "Eryslai - the Kingdom of Wind" 

	longname.Add "powater", "Reef of Coirnav" 

	longname.Add "pofire", "Doomfire - The Burning Lands" 

	longname.Add "poeartha", "Vegarlson - The Earthen Badlands" 

	longname.Add "potimea", "Plane of Time" 

	longname.Add "hohonorb", "Temple of Marr" 

	longname.Add "nightmareb", "Lair of Terris Thule" 

	longname.Add "poearthb", "Ragrax - Stronghold of the Twelve" 

	longname.Add "potimeb", "Plane of Time" 

	longname.Add "gunthak", "Gulf of Gunthak" 

	longname.Add "dulak", "Dulak's Harbor" 

	longname.Add "torgiran", "Torgiran Mines" 

	longname.Add "nadox", "Crypt of Nadox" 

	longname.Add "hatesfury", "Hate's Fury - The Scorned Maiden" 

	longname.Add "guka", "The Cauldron of Lost Souls" 

	longname.Add "ruja", "The Bloodied Quarries" 

	longname.Add "taka", "The Sunken Library" 

	longname.Add "mira", "The Silent Gallery" 

	longname.Add "mmca", "The Forlorn Caverns" 

	longname.Add "gukb", "The Drowning Crypt" 

	longname.Add "rujb", "The Halls of War" 

	longname.Add "takb", "The Shifting Tower" 

	longname.Add "mirb", "The Maw of the Menagerie" 

	longname.Add "mmcb", "The Dreary Grotto" 

	longname.Add "gukc", "The Ancient Aqueducts" 

	longname.Add "rujc", "The Wind Bridges" 

	longname.Add "takc", "The Fading Temple" 

	longname.Add "mirc", "The Spider Den" 

	longname.Add "mmcc", "The Asylum of Invoked Stone" 

	longname.Add "gukd", "The Mushroom Grove" 

	longname.Add "rujd", "The Gladiator Pits" 

	longname.Add "takd", "The Royal Observatory" 

	longname.Add "mird", "The Hushed Banquet" 

	longname.Add "mmcd", "The Chambers of Eternal Affliction" 

	longname.Add "guke", "The Foreboding Prison" 

	longname.Add "ruje", "The Drudge Hollows" 

	longname.Add "take", "The River of Recollection" 

	longname.Add "mire", "The Frosted Halls" 

	longname.Add "mmce", "The Sepulcher of the Damned" 

	longname.Add "gukf", "The Chapel of the Witnesses" 

	longname.Add "rujf", "The Fortified Lair of the Taskmasters" 

	longname.Add "takf", "The Sandfall Corridors" 

	longname.Add "mirf", "The Forgotten Wastes" 

	longname.Add "mmcf", "The Ritualistic Summoning Grounds" 

	longname.Add "gukg", "The Root Garden" 

	longname.Add "rujg", "The Hidden Vale" 

	longname.Add "takg", "The Balancing Chamber" 

	longname.Add "mirg", "The Heart of the Menagerie" 

	longname.Add "mmcg", "The Cesspits of Putrescence" 

	longname.Add "gukh", "The Accursed Sanctuary" 

	longname.Add "rujh", "The Blazing Forge" 

	longname.Add "takh", "The Sweeping Tides" 

	longname.Add "mirh", "The Morbid Laboratory" 

	longname.Add "mmch", "The Aisles of Blood" 

	longname.Add "ruji", "The Arena of Chance" 

	longname.Add "taki", "The Antiquated Palace" 

	longname.Add "miri", "The Theater of Imprisoned Horrors" 

	longname.Add "mmci", "The Halls of Sanguinary Rites" 

	longname.Add "rujj", "The Barracks of War" 

	longname.Add "takj", "The Prismatic Corridors" 

	longname.Add "mirj", "The Grand Library" 

	longname.Add "mmcj", "The Infernal Sanctuary" 

	longname.Add "chardokb", "The Halls of Betrayal" 

	longname.Add "soldungc", "The Caverns of Exile" 

	longname.Add "abysmal", "Abysmal Sea" 

	longname.Add "natimbi", "Natimbi - The Broken Shores" 

	longname.Add "qinimi", "Qinimi - Court of Nihilia" 

	longname.Add "riwwi", "Riwwi - Coliseum of Games" 

	longname.Add "barindu", "Barindu - Hanging Gardens" 

	longname.Add "ferubi", "Ferubi - Forgotten Temple of Taelosia" 

	longname.Add "snpool", "Sewers of Nihilia - Pool of Sludge" 

	longname.Add "snlair", "Sewers of Nihilia - Lair of Trapped Ones" 

	longname.Add "snplant", "Sewers of Nihilia - Purifying Plant" 

	longname.Add "sncrematory", "Sewers of Nihilia - the Crematory" 

	longname.Add "tipt", "Tipt - Treacherous Crags" 

	longname.Add "vxed", "Vxed - The Crumbling Caverns" 

	longname.Add "yxtta", "Yxtta - Pulpit of Exiles" 

	longname.Add "uqua", "Uqua - The Ocean God Chantry" 

	longname.Add "kodtaz", "Kod'Taz - Broken Trial Grounds" 

	longname.Add "ikkinz", "Ikkinz - Chambers of Destruction" 

	longname.Add "qvic", "Qvic - Prayer Grounds of Calling" 

	longname.Add "inktuta", "Inktu`Ta, The Unmasked Chapel" 

	longname.Add "txevu", "Txevu - Lair of the Elite" 

	longname.Add "tacvi", "Tacvi - Seat of the Slaver" 

	longname.Add "qvicb", "Qvic - the Hiden Vault" 

	longname.Add "wallofslaughter", "Wall of Slaughter" 

	longname.Add "bloodfields", "The Bloodfields" 

	longname.Add "draniksscar", "Dranik's Scar" 

	longname.Add "causeway", "Nobles' Causeway" 

	longname.Add "chambersa", "Muramite Proving Grounds" 

	longname.Add "chambersb", "Muramite Proving Grounds" 

	longname.Add "chambersc", "Muramite Proving Grounds" 

	longname.Add "chambersd", "Muramite Proving Grounds" 

	longname.Add "chamberse", "Muramite Proving Grounds" 

	longname.Add "chambersf", "Muramite Proving Grounds" 

	longname.Add "provinggrounds", "Muramite Proving Grounds" 

	longname.Add "anguish", "Asylum of Anguish" 

	longname.Add "dranikhollowsa", "Dranik's Hollows" 

	longname.Add "dranikhollowsb", "Dranik's Hollows" 

	longname.Add "dranikhollowsc", "Dranik's Hollows" 

	longname.Add "dranikhollowsd", "Dranik's Hollows" 

	longname.Add "dranikhollowse", "Dranik's Hollows" 

	longname.Add "dranikhollowsf", "Dranik's Hollows" 

	longname.Add "dranikhollowsg", "Dranik's Hollows" 

	longname.Add "dranikhollowsh", "Dranik's Hollows" 

	longname.Add "dranikhollowsi", "Dranik's Hollows" 

	longname.Add "dranikhollowsj", "Dranik's Hollows" 

	longname.Add "dranikcatacombsa", "Catacombs of Dranik" 

	longname.Add "dranikcatacombsb", "Catacombs of Dranik" 

	longname.Add "dranikcatacombsc", "Catacombs of Dranik" 

	longname.Add "draniksewersa", "Sewers of Dranik" 

	longname.Add "draniksewersb", "Sewers of Dranik" 

	longname.Add "draniksewersc", "Sewers of Dranik" 

	longname.Add "riftseekers", "Riftseekers' Sanctum" 

	longname.Add "harbingers", "Harbingers' Spire" 

	longname.Add "dranik", "The Ruined City of Dranik" 

	longname.Add "broodlands", "The Broodlands" 

	longname.Add "stillmoona", "Stillmoon Temple" 

	longname.Add "stillmoonb", "The Ascent" 

	longname.Add "thundercrest", "Thundercrest Isles" 

	longname.Add "delvea", "Lavaspinner's Lair" 

	longname.Add "delveb", "Tirranum's Delve"

	longname.Add "thenest", "The Accursed Nest" 

	longname.Add "guildlobby", "Guild Lobby" 

	longname.Add "guildhall", "Guild Hall" 

	longname.Add "barter", "Barter Hall" 

	longname.Add "illsalin", "The Ruins of Illsalin" 

	longname.Add "illsalina", "Imperial Bazaar" 

	longname.Add "illsalinb", "Temple of the Korlach" 

	longname.Add "illsalinc", "The Nargilor Pits" 

	longname.Add "dreadspire", "Dreadspire Keep" 

	longname.Add "dreadspirea", "The Torture Chamber"

	longname.Add "dreadspireb", "The Artifact Room"

	longname.Add "drachnidhive", "The Hive" 

	longname.Add "drachnidhivea", "Living Larder" 

	longname.Add "drachnidhiveb", "Coven of the Skinwalkers" 

	longname.Add "drachnidhivec", "Queen Sendaii's Lair" 

	longname.Add "westkorlach", "Stoneroot Falls" 

	longname.Add "westkorlacha", "Chambers of Xill" 

	longname.Add "westkorlachb", "Caverns of the Lost" 

	longname.Add "westkorlachc", "Lair of the Korlach" 

	longname.Add "eastkorlach", "Undershore" 

	longname.Add "eastkorlacha", "Snarlstone Dens" 

	longname.Add "shadowspine", "Shadowspine" 

	longname.Add "corathus", "Corathus Creep" 

	longname.Add "corathusa", "Sporali Caverns" 

	longname.Add "corathusb", "Corathus Lair" 

	longname.Add "nektulosa", "Shadowed Grove" 

	longname.Add "arcstone", "Arcstone" 

	longname.Add "relic", "Relic"

	longname.Add "skylance", "Skylance"

	longname.Add "devastation", "The Devastation"

	longname.Add "devastationa", "The Seething Wall"

	longname.Add "rage", "Sverag, Stronghold of Rage"

	longname.Add "ragea", "Razorthorn - Tower of Sullon Zek"

	longname.Add "takishruins", "Ruins of Takish-Hiz"

	longname.Add "takishruinsa", "The Root of Ro"

	longname.Add "elddar", "The Elddar Forest"

	longname.Add "elddara", "Tunare's Shrine"

	longname.Add "theater", "Theater of Blood"

	longname.Add "theatera", "Deathknell - Tower of Dissonance"

	longname.Add "freeporteast", "Freeport East"

	longname.Add "freeportwest", "Freeport West"

	longname.Add "freeportsewers", "Freeport Sewers"

	longname.Add "freeportacademy", "Academy of Arcane Sciences"

	longname.Add "freeporttemple", "Temple of Marr"

	longname.Add "freeportmilitia", "Freeport Militia House"

	longname.Add "freeportarena", "Arena"

	longname.Add "freeportcityhall", "City Hall"

	longname.Add "freeporttheater", "Theater"

	longname.Add "freeporthall", "Hall of Truth"

	longname.Add "northro", "North Ro"

	longname.Add "southro", "South Ro"

	longname.Add "crescent", "Crescent Reach" 

	longname.Add "moors", "Blightfire Moors" 

	longname.Add "stonehive", "Stone Hive" 

	longname.Add "mesa", "Koru`kar Mesa" 

	longname.Add "roost", "Blackfeather Roost" 

	longname.Add "steppes", "The Steppes" 

	longname.Add "icefall", "Icefall Glacier" 

	longname.Add "valdeholm", "Valdeholm" 

	longname.Add "frostcrypt", "Frostcrypt - Throne of the Shade King" 

	longname.Add "sunderock", "Sunderock Springs" 

	longname.Add "vergalid", "Vergalid Mines" 

	longname.Add "direwind", "Direwind Cliffs" 

	longname.Add "ashengate", "Ashengate - Reliquary of the Scale" 

	longname.Add "highpasshold", "Highpass Hold" 

	longname.Add "commonlands", "Commonlands" 

	longname.Add "oceanoftears", "Ocean of Tears" 

	longname.Add "kithforest", "Kithicor Forest" 

	longname.Add "befallenb", "Befallen" 

	longname.Add "highpasskeep", "HighKeep" 

	longname.Add "innothuleb", "Innothule Swamp" 

	longname.Add "toxxulia", "Toxxulia Forest" 

	longname.Add "mistythicket", "Misty Thicket" 

	longname.Add "kattacastrum", "Katta Castrum" 

	longname.Add "thalassius", "Thalassius - The Coral Keep" 

	longname.Add "atiiki", "Jewel of Atiiki" 

	longname.Add "zhisza", "Zhisza, the Shissar Sanctuary" 

	longname.Add "silyssar", "Silyssar - New Chelsith" 

	longname.Add "solteris", "Solteris - The Throne of Ro" 

	longname.Add "barren", "Barren Coast" 

	longname.Add "buriedsea", "The Buried Sea" 

	longname.Add "jardelshook", "Jardel's Hook" 

	longname.Add "monkeyrock", "Monkey Rock" 

	longname.Add "suncrest", "Suncrest Isle" 

	longname.Add "deadbone", "Deadbone Reef" 

	longname.Add "blacksail", "Blacksail Folly" 

	longname.Add "maidensgrave", "Maiden's Grave" 

	longname.Add "redfeather", "Redfeather Isle" 

	longname.Add "shipmvp", "The Open Sea" 

	longname.Add "shipmvu", "The Open Sea" 

	longname.Add "shippvu", "The Open Sea" 

	longname.Add "shipuvu", "The Open Sea" 

	longname.Add "shipmvm", "The Open Sea" 

	longname.Add "mechanotus", "Fortress Mechanotus" 

	longname.Add "mansion", "Meldrath's Majestic Mansion" 

	longname.Add "steamfactory", "The Steam Factory" 

	longname.Add "shipworkshop", "S.H.I.P. Workshop" 

	longname.Add "gyrospireb", "Gyrospire Beza" 

	longname.Add "gyrospirez", "Gyrospire Zeka" 

	longname.Add "dragonscale", "Dragonscale Hills" 

	longname.Add "lopingplains", "Loping Plains" 

	longname.Add "hillsofshade", "Hills of Shade" 

	longname.Add "bloodmoon", "Bloodmoon Keep" 

	longname.Add "crystallos", "Crystallos - Lair of the Awakened" 

	longname.Add "guardian", "The Mechamatic Guardian" 

	longname.Add "steamfontmts", "Steamfont Mountains" 

	longname.Add "cryptofshade", "Crypt of Shade" 

	longname.Add "dragonscalea", "Tinmizer's Wunderwerks"

	longname.Add "dragonscaleb", "Deepscar's Den"

	longname.Add "oldfieldofbone", "Field of Scale"

	longname.Add "oldkaesoraa", "Kaesora Library"

	longname.Add "oldkaesorab", "Hatchery Wing"

	longname.Add "oldkurn", "Kurn's Tower"

	longname.Add "oldkithicor", "Bloody Kithicor"

	longname.Add "oldcommons", "Old Commonlands"

	longname.Add "oldhighpass", "Highpass Hold"

	longname.Add "thevoida", "The Void"

	longname.Add "thevoidb", "The Void"

	longname.Add "thevoidc", "The Void"

	longname.Add "thevoidd", "The Void"

	longname.Add "thevoide", "The Void"

	longname.Add "thevoidf", "The Void"

	longname.Add "thevoidg", "The Void"

	longname.Add "oceangreenhills", "Oceangreen Hills"

	longname.Add "oceangreenvillage", "Oceangreen Village"

	longname.Add "oldblackburrow", "Blackburrow"

	longname.Add "bertoxtemple", "Temple of Bertoxxulous"

	longname.Add "discord", "Korafax - Home of the Riders"

	longname.Add "discordtower", "Citadel of the Worldslayer"

	longname.Add "oldbloodfield", "Old Bloodfields"

	longname.Add "precipiceofwar", "The Precipice of War"

	longname.Add "olddranik", "City of Dranik"

	longname.Add "toskirakk", "Toskirakk"

	longname.Add "korascian", "Korascian Warrens"

	longname.Add "rathechamber", "Rathe Council Chambers"

	longname.Add "arttest", "Art Testing Domain" 

	longname.Add "fhalls", "The Forgotten Halls" 

	longname.Add "apprentice", "Designer Apprentice" 

    longname.Add "crafthalls", "Ngreth's Den"
    longname.Add "brellsrest", "Brell's Rest"
    longname.Add "fungalforest", "Fungal Forest"
    longname.Add "underquarry", "The Underquarry"
    longname.Add "coolingchamber", "The Cooling Chamber"
    longname.Add "shiningcity", "Kernagir, The Shining City"
    longname.Add "arthicrex", "Arthicrex"
    longname.Add "foundation", "The Foundation"
    longname.Add "lichencreep", "Lichen Creep"
    longname.Add "pellucid", "Pellucid Grotto"
    longname.Add "stonesnake", "Volska's Husk"
    longname.Add "brellstemple", "Brell's Temple"
    longname.Add "convorteum", "The Convorteum"
    longname.Add "brellsarena", "Brell's Arena"
    longname.Add "weddingchapel", "Wedding Chapel"
    longname.Add "weddingchapeldark", "Wedding Chapel"
    longname.Add "dragoncrypt", "Lair of the Fallen"
    longname.Add "feerrott2", "The Feerrott"
    longname.Add "thulehouse1", "House of Thule"
    longname.Add "thulehouse2", "House of Thule, Upper Floors"
    longname.Add "housegarden", "The Grounds"
    longname.Add "thulelibrary", "The Library"
    longname.Add "well", "The Well"
    longname.Add "fallen", "Erudin Burning"
    longname.Add "morellcastle", "Morell's Castle"
    longname.Add "somnium", "Sanctum Somnium"
    longname.Add "alkabormare", "Al'Kabor's Nightmare"
    longname.Add "miragulmare", "Miragul's Nightmare"
    longname.Add "thuledream", "Fear Itself"
    longname.Add "neighborhood", "Sunrise Hills"
    longname.Add "phylactery", "Miragul's Phylactery"
    longname.Add "phinterior3a1", "House Interior"
    longname.Add "phinterior3a2", "House Interior"
    longname.Add "phinterior3a3", "House Interior"
    longname.Add "phinterior1a1", "House Interior"
    longname.Add "phinterior1a2", "House Interior"
    longname.Add "phinterior1a3", "House Interior"
    longname.Add "phinterior1b1", "Dragon House Interior"
    longname.Add "phinterior1d1", "Dragon House Interior"
    longname.Add "argath", "Argath, Bastion of Illdaera"
    longname.Add "arelis", "Valley of Lunanyn"
    longname.Add "sarithcity", "Sarith, City of Tides"
    longname.Add "rubak", "Rubak Oseka, Temple of the Sea"
    longname.Add "beastdomain", "Beasts' Domain"
    longname.Add "resplendent", "The Resplendent Temple"
    longname.Add "pillarsalra", "Pillars of Alra"
    longname.Add "windsong", "Windsong Sanctuary"
    longname.Add "cityofbronze", "Erillion, City of Bronze"
    longname.Add "sepulcher", "Sepulcher of Order"
    longname.Add "eastsepulcher", "Sepulcher East"
    longname.Add "westsepulcher", "Sepulcher West"
    longname.Add "shadowedmount", "Shadowed Mount"
    longname.Add "guildhalllrg", "Grand Guild Hall"
    longname.Add "guildhallsml", "Greater Guild Hall"
    longname.Add "plhogrinteriors1a1", "One Bedroom House Interior"
    longname.Add "plhogrinteriors1a2", "One Bedroom House Interior"
    longname.Add "plhogrinteriors3a1", "Three Bedroom House Interior"
    longname.Add "plhogrinteriors3a2", "Three Bedroom House Interior"
    longname.Add "plhogrinteriors3b1", "Three Bedroom House Interior"
    longname.Add "plhogrinteriors3b2", "Three Bedroom House Interior"
    longname.Add "plhdkeinteriors1a1", "One Bedroom House Interior"
    longname.Add "plhdkeinteriors1a2", "One Bedroom House Interior"
    longname.Add "plhdkeinteriors1a3", "One Bedroom House Interior"
    longname.Add "plhdkeinteriors3a1", "Three Bedroom House Interior"
    longname.Add "plhdkeinteriors3a2", "Three Bedroom House Interior"
    longname.Add "plhdkeinteriors3a3", "Three Bedroom House Interior"
    longname.Add "guildhall3", "Modest Guild Hall"
    longname.Add "kaelshard", "Kael Drakkel: The King's Madness"
    longname.Add "eastwastesshard", "East Wastes: Zeixshi-Kar's Awakening"
    longname.Add "crystalshard", "The Crystal Caverns: Fragment of Fear"
    longname.Add "shardslanding", "Shard's Landing"
    longname.Add "xorbb", "Valley of King Xorbb"
    longname.Add "breedinggrounds", "The Breeding Grounds"
    longname.Add "eviltree", "Evantil, the Vile Oak"
    longname.Add "grelleth", "Grelleth's Palace, the Chateau of Filth"
    longname.Add "chapterhouse", "Chapterhouse of the Fallen"
    longname.Add "pomischief", "The Plane of Mischief"
    longname.Add "burnedwoods", "The Burned Woods"
 

	Set fso = CreateObject("Scripting.FileSystemObject")

	ForReading = 1



	sPath = MID(Wscript.ScriptFullName,1,InStrRev(Wscript.ScriptFullName,"\")-1)



	Set Folder = fso.GetFolder(sPath)

	Set MapFiles = Folder.Files

	mCount = 0



	For Each myFile in MapFiles

        mLines = 0

		sExtension = Right(myFile, 6)

		if UCase(sExtension) = "_1.TXT" then

			mCount = mCount + 1

	  		strPath = myFile.Path

	  		Set Test = fso.GetFile(strPath)

		  	If Test.Size > 0 Then

				sZone = left(Test.Name, len(Test.Name) - 6)

				szPath = sPath & "\"

				szMapFile = szPath & sZone & ".map"

				objExplorer.Document.Body.InnerHTML = "Converting <b>" & sZone & "</b> from SOE to SEQ format."

		    		Dim fs, soemap, newmap

		    		Set fs = CreateObject("Scripting.FileSystemObject")

		    		Set newmap = fs.CreateTextFile(szMapFile, True, False)

		    		If longname.Exists(sZone) Then

		    			newmap.write (longname.Item(sZone) & "," & sZone & ",0,0") & chr(10)

		    		Else

		    			newmap.write (sZone & " map," & sZone & ",0,0") & chr(10)

		    		End If

				For w = 0 to 2
                    if w = 0 Then
                        sFileName = szPath & sZone & ".txt"
                    Else
    					sFileName = szPath & sZone & "_" & w & ".txt"
                    End If

					If fso.FileExists(sFileName) then

						Set soemap = fs.OpenTextFile(sFileName, 1, False)

						Do While soemap.AtEndOfStream <> True

							PreText = ""

							PreText = soemap.ReadLine

							' Line Handling

							If Left(PreText, 1) = "P" Then

								NextText = Replace(PreText, " ", "")

								PostText = Right(NextText, Len(NextText) - 1)

								LastText = Replace(PostText, ",to_", ",To_")

								L = Split(LastText, ",", -1)

								If UBound(L) > -1 Then

						    			' Have split line string - 9 parameters, 0-8

						    			' Convert Color into a 1-64 index

						    			r = DoColor(CLng(L(3)))

						    			g = DoColor(CLng(L(4)))

						    			b = DoColor(CLng(L(5)))

						    			Dim sTextString

						    			sTextString = Replace(L(7), "_", " ")

						    			iColorInd = (r) + (g * 4) + (b * 16) + 1

						    			' Convert Blacks to White for labels if set

									If BlackLabelColor > 0 Then

								    		If iColorInd = 1 Then iColorInd = BlackLabelColor

									End If

									If Len(sTextString) > 3 Then

										If Left(sTextString,3) = "To " then iColorInd = tocolor

									End If

									If AllLabelColor > 0 then iColorInd = AllLabelColor

									v = s = t = 0

						    			v = -1 * CLng(L(0))

						    			s = -1 * CLng(L(1))
									t = 1 * CLng(L(2))

						    			MapLabelOut = "P," & sTextString & "," & szColor(iColorInd) & "," & v & "," & s & "," & t

						    			newmap.write (MapLabelOut) & chr(10)

								End If

						  	End If

					  		If Left(PreText, 1) = "L" Then

								PostText = Replace(PreText, "L", "")

								LastText = Replace(PostText, " ", "")

								L = Split(LastText, ",", -1)

								If UBound(L) > -1 Then

									' Have split line string - 9 parameters, 0-8

									' Convert Color into a 1-64 index

									r = DoColor(CLng(L(6)))

									g = DoColor(CLng(L(7)))

						    			b = DoColor(CLng(L(8)))

							    		iColorInd = (r) + (g * 4) + (b * 16) + 1

						    			' Convert Blacks to Grey if set

									If BlackLineColor > 0 Then

							    			If iColorInd = 1 Then iColorInd = BlackLineColor

									End If

									If AllLineColor > 0 then iColorInd = AllLineColor

									x1 = y1 = z1 = x2 = y2 = z2 = 0

						    			x1 = -1 * CLng(L(0))

						    			y1 = -1 * CLng(L(1))

						    			z1 = 10 * CLng(L(2))

						    			x2 = -1 * CLng(L(3))

						    			y2 = -1 * CLng(L(4))

						    			z2 = 10 * CLng(L(5))

						    			maplineout = "M,line," & szColor(iColorInd) & ",2," & x1 & "," & y1 & "," & z1 & "," & x2 & "," & y2 & "," & z2

						    			newmap.write (maplineout) & chr(10)

                                        mLines = mLines + 1

								End If

					 	 	End If

				    		Loop

						soemap.Close

					End If

                    If (w = 0 and mLines > 0) Then

                        w = 2

                    End If

				Next

				newmap.Close

				If ConnectDots = 1 Then

					objExplorer.Document.Body.InnerHTML = "Converting <b>" & sZone & "</b> from SOE to SEQ format.<br>Connecting map lines."

					Call Remove_Dots (szMapFile, sZone)

		  			Call Connect_The_Dots (szMapFile, sZone)

				End If

			End If

		End If

	Next

	if mCount = 0 then

		objExplorer.Document.Body.InnerHTML = "No Maps Found to Process.<br>Run this file from the folder containing maps."

		WScript.Sleep 5000

	End If

End Sub



Function DoColor(iColor)

	If iColor < 69 Then

		DoColor = 0

	Else

		If iColor < 160 Then

			DoColor = 1

		Else

			If iColor < 240 Then

				DoColor = 2

			Else

		    		DoColor = 3

			End If

		End If

	End If

End Function



Sub Connect_The_Dots(FileName,sZone)

	Dim Outline, PreText, NextLine

	Dim fso

	' Set these for connecting lines.  If it is less than this between points, it is considered equivalent

	xyCoord = 2

	zCoord = 2

	Set fso = CreateObject("Scripting.FileSystemObject")

	sBackup = FileName & "~"

	sMapFile = FileName

	optsnum = 1

	lastchance = 0

	Do While optsnum > 0

		Set MyFile = fso.GetFile(FileName)

		MyFile.Copy(sBackup)

		optsnum = 0

		Dim fs, oldmap, newmap

		Set fs = CreateObject("Scripting.FileSystemObject")

		' Create New Map for Writing Optimized Map

		Set newmap = fs.CreateTextFile(sMapFile, True, False)

		' Open map for reading (backup file)

		Set oldmap = fs.OpenTextFile(sBackup, 1, False)

		If lastchance = 1 Then

			Do

				If oldmap.AtEndOfStream <> True Then

					PreText = ""

					PreText = oldmap.ReadLine

					newmap.write PreText & chr(10)

				Else

					Exit Do

				End If

			Loop Until Left(PreText,2) = "M,"

		End If

		Do While oldmap.AtEndOfStream <> True

			PreText = ""

			PreText = oldmap.ReadLine

			' Only do lines

			If Left(PreText, 2) = "M," Then

				x1 = y1 = z1 = x2 = y2 = z2 = nx1 = ny1 = nz1 = nx2 = ny2 = nz2 = 0

				L = Split(PreText, ",", -1)

				'Find number of coordinates in line

				num = (UBound(L) - 3) / 3

				pColor = L(2)

				x1 = CLng(L(4))

				y1 = CLng(L(5))

				z1 = CLng(L(6))

				x2 = CLng(L(num * 3 + 1))

				y2 = CLng(L(num * 3 + 2))

				z2 = CLng(L(num * 3 + 3))

				If oldmap.AtEndOfStream = True Then

					' At end of file, so write line and be done

					newmap.write PreText & chr(10)

			  	Else

					' Read next line, see if can add coordinates

					NextLine = oldmap.ReadLine

					If Left(NextLine, 2) = "M," Then

						M = Split(NextLine, ",", -1)

						nnum = (UBound(M) - 3) / 3

						nColor = L(2)

						nx1 = CLng(M(4))

						ny1 = CLng(M(5))

						nz1 = CLng(M(6))

						nx2 = CLng(M(nnum * 3 + 1))

						ny2 = CLng(M(nnum * 3 + 2))

						nz2 = CLng(M(nnum * 3 + 3))

						If (pColor = nColor) And (Abs(nx2 - x1) <= xyCoord) And (Abs(ny2 - y1) <= xyCoord) And (Abs(nz2 - z1) <= zCoord) Then

							optsnum = optsnum + 1

							'put 2nd point before first

							Outline = L(0) & "," & L(1) & "," & L(2) & "," & (num + nnum - 1)

							For tl = 1 To nnum

								Outline = Outline & "," & M(tl * 3 + 1) & "," & M(tl * 3 + 2) & "," & M(tl * 3 + 3)

							Next

							For tl = 2 To num

								Outline = Outline & "," & L(tl * 3 + 1) & "," & L(tl * 3 + 2) & "," & L(tl * 3 + 3)

							Next

							newmap.write Outline & chr(10)

						Else

							If (pColor = nColor) And (Abs(nx1 - x2) <= xyCoord) And (Abs(ny1 - y2) <= xyCoord) And (Abs(nz1 - z2) <= zCoord) Then

								optsnum = optsnum + 1

								' put 2nd point after first

								Outline = L(0) & "," & L(1) & "," & L(2) & "," & (num + nnum - 1)

								For tl = 1 To num

						    			Outline = Outline & "," & L(tl * 3 + 1) & "," & L(tl * 3 + 2) & "," & L(tl * 3 + 3)

								Next

								For tl = 2 To nnum

									Outline = Outline & "," & M(tl * 3 + 1) & "," & M(tl * 3 + 2) & "," & M(tl * 3 + 3)

								Next

								newmap.write Outline & chr(10)

					  		Else

								'   check lines back to back

								If (pColor = nColor) And (Abs(nx1 - x1) <= xyCoord) And (Abs(ny1 - y1) <= xyCoord) And (Abs(nz1 - z1) <= zCoord) Then

						    			optsnum = optsnum + 1

						    			' put 2nd point after first

						    			Outline = L(0) & "," & L(1) & "," & L(2) & "," & (num + nnum - 1)

									For tl = nnum To 1 Step -1

										Outline = Outline & "," & M(tl * 3 + 1) & "," & M(tl * 3 + 2) & "," & M(tl * 3 + 3)

									Next

									For tl = 2 To num

										Outline = Outline & "," & L(tl * 3 + 1) & "," & L(tl * 3 + 2) & "," & L(tl * 3 + 3)

									Next

									newmap.write Outline & chr(10)

								Else

									If (pColor = nColor) And (Abs(nx2 - x2) <= xyCoord) And (Abs(ny2 - y2) <= xyCoord) And (Abs(nz2 - z2) <= zCoord) Then

										optsnum = optsnum + 1

										' put 2nd point after first

										Outline = L(0) & "," & L(1) & "," & L(2) & "," & (num + nnum - 1)

										For tl = 1 To num

											Outline = Outline & "," & L(tl * 3 + 1) & "," & L(tl * 3 + 2) & "," & L(tl * 3 + 3)

										Next

										For tl = (nnum - 1) To 1 Step -1

											Outline = Outline & "," & M(tl * 3 + 1) & "," & M(tl * 3 + 2) & "," & M(tl * 3 + 3)

										Next

										newmap.write Outline & chr(10)

									Else

										newmap.write PreText & chr(10)

										newmap.write NextLine & chr(10)

									End If

								End If

							End If

						End If

					Else

						newmap.write PreText & chr(10)

						newmap.write NextLine & chr(10)

					End If

				End If

			Else

				newmap.write PreText & chr(10)

			End If

		Loop

		objExplorer.Document.Body.InnerHTML = "Converting <b>" & sZone & "</b> from SOE to SEQ format.<br>Connecting map lines.<br>This Pass: Lines Connected - " & optsnum

		If optsnum > 0 Then lastchance = 0

		If (optsnum = 0) and (lastchance = 0) Then

			lastchance = 1

			optsnum = 1

		End If

		newmap.Close

		oldmap.Close

	Loop

	Set MyFile = fso.GetFile(sBackup)

	MyFile.Delete

End Sub



Sub Remove_Dots(FileName,sZone)

	Dim Outline, PreText, NextLine

	Dim fso

	' Set these for connecting lines.  If it is less than this between points, it is considered equivalent

	Set fso = CreateObject("Scripting.FileSystemObject")

	sBackup = FileName & "~"

	sMapFile = FileName

	optsnum = 1

	lastchance = 0

	Do While optsnum > 0

		Set MyFile = fso.GetFile(FileName)

		MyFile.Copy(sBackup)

		optsnum = 0

		Dim fs, oldmap, newmap

		Set fs = CreateObject("Scripting.FileSystemObject")

		' Create New Map for Writing Optimized Map

		Set newmap = fs.CreateTextFile(sMapFile, True, False)

		' Open map for reading (backup file)

		Set oldmap = fs.OpenTextFile(sBackup, 1, False)

		If lastchance = 1 Then

			Do

				If oldmap.AtEndOfStream <> True Then

					PreText = ""

					PreText = oldmap.ReadLine

					newmap.write PreText & chr(10)

				Else

					Exit Do

				End If

			Loop Until Left(PreText,2) = "M,"

		End If

		Do While oldmap.AtEndOfStream <> True

			PreText = ""

			PreText = oldmap.ReadLine

			' Only do lines

			If Left(PreText, 2) = "M," Then

				x1 = y1 = z1 = x2 = y2 = z2 = nx1 = ny1 = nz1 = nx2 = ny2 = nz2 = 0

				L = Split(PreText, ",", -1)

				'Find number of coordinates in line

				num = (UBound(L) - 3) / 3

				pColor = L(2)

				x1 = CLng(L(4))

				y1 = CLng(L(5))

				z1 = CLng(L(6))

				x2 = CLng(L(num * 3 + 1))

				y2 = CLng(L(num * 3 + 2))

				z2 = CLng(L(num * 3 + 3))

				vx1 = CDbl(L(4))

				vy1 = CDbl(L(5))

				vz1 = CDbl(L(6))

				vx2 = CDbl(L(num * 3 + 1))

				vy2 = CDbl(L(num * 3 + 2))

				vz2 = CDbl(L(num * 3 + 3))

				If oldmap.AtEndOfStream = True Then

					' At end of file, so write line and be done

					newmap.write PreText & chr(10)

			  	Else

					' Read next line, see if can add coordinates

					NextLine = oldmap.ReadLine

					dotremoved = 0

					If Left(NextLine, 2) = "M," Then

						M = Split(NextLine, ",", -1)

						nnum = (UBound(M) - 3) / 3

						nColor = L(2)

						nx1 = CLng(M(4))

						ny1 = CLng(M(5))

						nz1 = CLng(M(6))

						nx2 = CLng(M(nnum * 3 + 1))

						ny2 = CLng(M(nnum * 3 + 2))

						nz2 = CLng(M(nnum * 3 + 3))

						nvx1 = CDbl(M(4))

						nvy1 = CDbl(M(5))

						nvz1 = CDbl(M(6))

						nvx2 = CDbl(M(nnum * 3 + 1))

						nvy2 = CDbl(M(nnum * 3 + 2))

						nvz2 = CDbl(M(nnum * 3 + 3))

						If (pColor = nColor) Then

							If ((nx2 - x1) = 0) And ((ny2 - y1) = 0) And ((nz2 - z1) = 0) Then

								' points are common, see if can remove one

								temp1 = ((vx2 - vx1) * (vx2 - vx1)) + ((vy2 - vy1) * (vy2 - vy1)) + ((vz2 - vz1) * (vz2 - vz1))

								temp2 = ((nvx2 - nvx1) * (nvx2 - nvx1)) + ((nvy2 - nvy1) * (nvy2 - nvy1)) + ((nvz2 - nvz1) * (nvz2 - nvz1))

								If (temp1 > 0) and (temp2 > 0) Then

									V1 = Sqr(temp1)

									V2 = Sqr(temp2)

									va = (vx2 - vx1) / V1

									vb = (vy2 - vy1) / V1

									vc = (vz2 - vz1) / V1

									nva = (nvx2 - nvx1) / V2

									nvb = (nvy2 - nvy1) / V2

									nvc = (nvz2 - nvz1) / V2

									if (va * nva + vb * nvb + vc * nvc) > 0.9999 Then

									'if (va = nva) and (vb = nvb) and (vc = nvc) Then

										' Have unit vectors of two lines in same direction

										' Remove the point

										optsnum = optsnum + 1

										'put 2nd point before first

										Outline = L(0) & "," & L(1) & "," & L(2) & ",2"

										Outline = Outline & "," & M(4) & "," & M(5) & "," & M(6)

										Outline = Outline & "," & L(7) & "," & L(8) & "," & L(9)

										dotremoved = 1

									End If

								End If

							End If

						Else

							If ((nx1 - x2) = 0) And ((ny1 - y2) = 0) And ((nz1 - z2) = 0) Then

								temp1 = ((vx2 - vx1) * (vx2 - vx1)) + ((vy2 - vy1) * (vy2 - vy1)) + ((vz2 - vz1) * (vz2 - vz1))

								temp2 = ((nvx2 - nvx1) * (nvx2 - nvx1)) + ((nvy2 - nvy1) * (nvy2 - nvy1)) + ((nvz2 - nvz1) * (nvz2 - nvz1))

								If (temp1 > 0) and (temp2 > 0) Then

									V1 = Sqr(temp1)

									V2 = Sqr(temp2)

									va = (vx2 - vx1) / V1

									vb = (vy2 - vy1) / V1

									vc = (vz2 - vz1) / V1

									nva = (nvx2 - nvx1) / V2

									nvb = (nvy2 - nvy1) / V2

									nvc = (nvz2 - nvz1) / V2

									if (va * nva + vb * nvb + vc * nvc) > 0.9999 Then

									'if (va = nva) and (vb = nvb) and (vc = nvc) Then

										' Have unit vectors of two lines in same direction

										' Remove the point

										optsnum = optsnum + 1

										Outline = L(0) & "," & L(1) & "," & L(2) & ",2"

						    				Outline = Outline & "," & L(4) & "," & L(5) & "," & L(6)

										Outline = Outline & "," & M(7) & "," & M(8) & "," & M(9)

										dotremoved = 1

									End if

								End if

					  		Else

								'   check lines back to back

								If ((nx1 - x1) = 0) And ((ny1 - y1) = 0) And ((nz1 - z1) = 0) Then

									temp1 = ((vx2 - vx1) * (vx2 - vx1)) + ((vy2 - vy1) * (vy2 - vy1)) + ((vz2 - vz1) * (vz2 - vz1))

									temp2 = ((nvx2 - nvx1) * (nvx2 - nvx1)) + ((nvy2 - nvy1) * (nvy2 - nvy1)) + ((nvz2 - nvz1) * (nvz2 - nvz1))

									If (temp1 > 0) and (temp2 > 0) Then

										V1 = Sqr(temp1)

										V2 = Sqr(temp2)

										va = (vx1 - vx2) / V1

										vb = (vy1 - vy2) / V1

										vc = (vz1 - vz2) / V1

										nva = (nvx2 - nvx1) / V2

										nvb = (nvy2 - nvy1) / V2

										nvc = (nvz2 - nvz1) / V2

										if (va * nva + vb * nvb + vc * nvc) > 0.9999 Then

										'if (va = nva) and (vb = nvb) and (vc = nvc) Then

											' Have unit vectors of two lines in same direction

											' Remove the point

						    					optsnum = optsnum + 1

						    					Outline = L(0) & "," & L(1) & "," & L(2) & ",2"

											Outline = Outline & "," & M(7) & "," & M(8) & "," & M(9)

											Outline = Outline & "," & L(7) & "," & L(8) & "," & L(9)

											dotremoved = 1

										End If

									End If

								Else

									If ((nx2 - x2) = 0) And ((ny2 - y2) = 0) And ((nz2 - z2) = 0) Then

										temp1 = ((vx2 - vx1) * (vx2 - vx1)) + ((vy2 - vy1) * (vy2 - vy1)) + ((vz2 - vz1) * (vz2 - vz1))

										temp2 = ((nvx2 - nvx1) * (nvx2 - nvx1)) + ((nvy2 - nvy1) * (nvy2 - nvy1)) + ((nvz2 - nvz1) * (nvz2 - nvz1))

										If (temp1 > 0) and (temp2 > 0) Then

											V1 = Sqr(temp1)

											V2 = Sqr(temp2)

											va = (vx2 - vx1) / V1

											vb = (vy2 - vy1) / V1

											vc = (vz2 - vz1) / V1

											nva = (nvx1 - nvx2) / V2

											nvb = (nvy1 - nvy2) / V2

											nvc = (nvz1 - nvz2) / V2

											if (va * nva + vb * nvb + vc * nvc) > 0.9999 Then

											'if (va = nva) and (vb = nvb) and (vc = nvc) Then

												optsnum = optsnum + 1

												Outline = L(0) & "," & L(1) & "," & L(2) & ",2"

												Outline = Outline & "," & L(4) & "," & L(5) & "," & L(6)

												Outline = Outline & "," & M(4) & "," & M(5) & "," & M(6)

												dotremoved = 1

											End If

										End If

									End If

								End If

							End If

						End If

					End If

					If dotremoved = 1 Then

						newmap.write Outline & chr(10)

					Else

						newmap.write PreText & chr(10)

						newmap.write NextLine & chr(10)

					End If

				End If

			Else

				newmap.write PreText & chr(10)

			End If

		Loop

		objExplorer.Document.Body.InnerHTML = "Converting <b>" & sZone & "</b> from SOE to SEQ format.<br>Connecting map lines.<br>This Pass: Points Removed - " & optsnum

		If optsnum > 0 Then lastchance = 0

		If (optsnum = 0) and (lastchance = 0) Then

			lastchance = 1

			optsnum = 1

		End If

		newmap.Close

		oldmap.Close

	Loop

	Set MyFile = fso.GetFile(sBackup)

	MyFile.Delete

End Sub